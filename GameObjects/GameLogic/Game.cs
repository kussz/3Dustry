﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameObjects.Drawing;
using GameObjects.Entities;
using GameObjects.Interfaces;
using GameObjects.Resources;
using SharpDX;

using SharpDX.Direct3D11;
using SharpDX.DirectWrite;

using SharpDX.Windows;

namespace GameObjects.GameLogic
{
    public class Game : IDisposable
    {
        private RenderForm _renderForm;
        private const float MOVE_STEP = 10f;
        private MeshObject[]? _groundCompound;
        private MeshObject[]? _oreCompound;
        private Camera _camera;

        private Loader _loader;
        Building? _building;
        Building? _closestBuilding;
        List<Building> _buildings;
        Building?[,] _buildingMap;
        Tile[][,]? _fullMap;
        bool[,]? _collideMap;
        private DirectX3DGraphics _directX3DGraphics;
        private Renderer _renderer;
        private InputHandler _inputHandler;
        private TimeHelper _timeHelper;
        private bool _isDataLoaded = false;
        public RenderForm MainForm { get { return _renderForm; } }

        private Menu _menu;

        public Game()
        {
            _renderForm = new RenderForm("My Game");
            _renderForm.UserResized += RenderFormResizedCallback;

            // Инициализация графической системы
            _directX3DGraphics = new DirectX3DGraphics(_renderForm);
            // Создание рендера
            _renderer = new Renderer(_directX3DGraphics);
            _renderer.CreateConstantBuffer();
            _loader = new Loader(_directX3DGraphics);
            //_menu = _loader.MakeTileSquare(new Vector4(0, 0, 0, 1));
            MenuTile.Configure(_loader, _directX3DGraphics.Device);
            ResourceTile.Configure(_loader);
            TextureStorage.Configure(_directX3DGraphics);
            Entity.Configure(_loader);
            _menu = new Menu();
            // Вспомогательные компоненты
            _inputHandler = new InputHandler();
            _buildings = new List<Building>();

            // Создание базовой камеры
            _camera = new Camera(new Vector4(0, 5.0f, 0, 1.0f));

            // Таймер
            _timeHelper = new TimeHelper();
            Cursor.Hide();
        }
        public async Task LoadGameDataAsync()
        {
            await Task.Run(Load);

        }
        private void Load()
        {
            MapLoader mapLoader = new MapLoader(_directX3DGraphics, _loader);

            // Загрузка карты и других данных
            _fullMap = mapLoader.LoadMap("map2");
            _collideMap = mapLoader.GetCollideMap(_fullMap[0]);
            _buildingMap = new Building[_fullMap[0].GetLength(0), _fullMap[0].GetLength(1)];
            _groundCompound = mapLoader.GetCompoundMap(_fullMap[0], 0);
            _oreCompound = mapLoader.GetCompoundMap(_fullMap[1], 0f);
            //TextureStorage.SetTextureHolder(EntityType.Miner, new TextureHolder(_directX3DGraphics.Device, "Assets\\Entities\\Miner\\"));
            // Создаем начальную сущность
            _building = EntityFactory.CreateBuilding(1, new Copper(0));
            //_entity = new Core(_loader, new Vector2(0, 0));
            _camera.Position = new Vector4(_fullMap[0].GetLength(0) / 2, 5.0f, _fullMap[0].GetLength(1) / 2, 1.0f);
            _timeHelper.Update();
            _isDataLoaded = true;
            
        }

        public void RenderFormResizedCallback(object sender, EventArgs args)
        {
            _directX3DGraphics.Resize();
            _camera.Aspect = _renderForm.ClientSize.Width /
                (float)_renderForm.ClientSize.Height;
        }

        private bool _firstRun = true;

        public void RenderLoopCallback()
        {
            if (_firstRun)
            {
                RenderFormResizedCallback(this, EventArgs.Empty);
                _firstRun = false;
            }
            if (!_isDataLoaded)
            {
                
                _renderForm.Text = "Loading...";
                _renderer.LoadingRender();
                
                _renderer.EndRender();
                return;
            }
            _timeHelper.Update();
            AlignCursorToCenter();
            ProceedInputs();

            //_renderForm.Text = "FPS: " + _timeHelper.FPS.ToString();
            //_renderForm.Text = _timeHelper.DeltaT.ToString();
            _renderForm.Text = "X: " + _camera.Position.X + " Y: " + _camera.Position.Y + " Z: " + _camera.Position.Z;

            
            
            Vector3 hitPoint = _camera.IntersectRayWithPlane(40f, 0);
            Vector3 hitPointDiscrete = new Vector3((float)Math.Round(hitPoint.X-_building.Size.X/2%1)+ _building.Size.X / 2 % 1, 0, (float)Math.Round(hitPoint.Z - _building.Size.X / 2 % 1)+ _building.Size.X / 2 % 1);
            _building.Mesh.MoveTo(hitPointDiscrete);
            _closestBuilding = GetClosestBuilding();

            Render(_camera.GetViewMatrix(), _camera.GetProjectionMatrix());


            if (_inputHandler.LeftMouseClick && IsBuildable(_building))
            {
                Build(_building);
            }
            if (_inputHandler.RightMouseClick)
            {
                Destroy(_closestBuilding);
            }
            
        }
        private Building? GetClosestBuilding()
        {
            return _buildings
            .Select(entity => new
            {
                Entity = entity,
                IntersectionPoint = entity.IntersectWithLook(_camera, 40)
            })
            .Where(x => x.IntersectionPoint != Vector3.Zero)
            .OrderBy(x => Vector3.Distance((Vector3)_camera.Position, x.IntersectionPoint))
            .Select(x => x.Entity)
            .FirstOrDefault();
        }
        private void AlignCursorToCenter()
        {
            var center = _renderForm.PointToScreen(new System.Drawing.Point(
                    _renderForm.Width / 2,
                    _renderForm.Height / 2
                ));
            Cursor.Position = center;
        }
        private void ProceedInputs()
        {
            _inputHandler.Update();
            if (_inputHandler.Escape)
                Environment.Exit(0);
            if(_inputHandler.SelectionChanged) {
                _inputHandler.SelectionChanged = false;
                _menu.SetSelectedCell(_inputHandler.HotbarSelection);
                _building = EntityFactory.CreateBuilding(_inputHandler.HotbarSelection, new Copper(0));
            }
            float xstep = 0;
            float ystep = 0;
            float zstep = 0;
            float step = MOVE_STEP * (_inputHandler.Shift ? 2 : 1);
            TextureHolder.Update(_timeHelper.DeltaT);
            if (_inputHandler.Forward)
                ystep += step * _timeHelper.DeltaT;
            if (_inputHandler.Backward)
                ystep -= step * _timeHelper.DeltaT;
            if (_inputHandler.Left)
                xstep += step * _timeHelper.DeltaT;
            if (_inputHandler.Right)
                xstep -= step * _timeHelper.DeltaT;
            if (_inputHandler.Left)
                xstep += step * _timeHelper.DeltaT;
            if (_inputHandler.Right)
                xstep -= step * _timeHelper.DeltaT;
            if (_inputHandler.Up)
                zstep += step * _timeHelper.DeltaT;
            if (_inputHandler.Down)
                zstep -= step * _timeHelper.DeltaT;
            if(_inputHandler.R)
                if (_building is IRotatable conv)
                    conv.Rotate();
            _camera.MoveBy(ystep * (float)Math.Sin(_camera._yaw), zstep, ystep * (float)Math.Cos(_camera._yaw));
            _camera.MoveBy(xstep * (float)Math.Sin(_camera._yaw - Math.PI / 2), zstep, xstep * (float)Math.Cos(_camera._yaw - Math.PI / 2));
            _camera.PitchBy(_inputHandler.MouseY);
            _camera.YawBy(_inputHandler.MouseX);
        }
        private void Render(Matrix viewMatrix, Matrix projectionMatrix)
        {
            _renderer.BeginRender();
            _renderer.SetCameraPosition(_camera.Position);
            _renderer.SetTransparent(-1);
            _renderer.SetSelected(false);
            _renderer.RenderCompound(_groundCompound,viewMatrix,projectionMatrix);
            _directX3DGraphics.DisableDepthTest();
            _renderer.RenderCompound(_oreCompound,viewMatrix,projectionMatrix);
            _directX3DGraphics.EnableDepthTest();
            _renderer.SetBuilding(true);
            foreach (var entity in _buildings)
            {
                entity.Produce(_timeHelper.DeltaT);
                _renderer.RenderEntity(entity, viewMatrix, projectionMatrix, entity == _closestBuilding);
                

            }
            _renderer.SetBuilding(false);
            _renderer.SetSelected(false);
            foreach (var conveyor in _buildings.OfType<Conveyor>())
            {
                _renderer.RenderConveyorTiles(conveyor, viewMatrix, projectionMatrix);
            }
            _renderer.SetMain(true);
            if (IsBuildable(_building))
            {
                _renderer.RenderEntity(_building, viewMatrix, projectionMatrix,false);
            }
            _renderer.SetMain(false);
            _renderer.SetSelected(false);
            float aspect = (float)_renderForm.ClientSize.Width / _renderForm.ClientSize.Height;
            _renderer.RenderMenuItem(_menu.Hotbar,aspect);
            _renderer.RenderMenuItem(_menu.SelectedCell,aspect);
            _renderer.RenderMenuItem(_menu.CrossHair,aspect);
            _renderer.EndRender();
        }
        public void Run()
        {
            _ = LoadGameDataAsync();
            RenderLoop.Run(_renderForm, RenderLoopCallback);
        }

        public void Dispose()
        {
            _renderer.Dispose();
            _directX3DGraphics.Dispose();
        }
        private bool IsBuildable(Building? entity)
        {
            if(entity!=null)
            {

                if (!IsInsideBounds(entity))
                    return false;
                for (int i = (int)entity.Mesh.Position.Z - (int)entity.Size.X / 2; i < (int)entity.Mesh.Position.Z + entity.Size[0] / 2; i++)
                    for (int j = (int)entity.Mesh.Position.X - (int)entity.Size.X / 2; j < (int)entity.Mesh.Position.X + entity.Size[0] / 2; j++)
                    {
                        if (_collideMap![i,j]==false)
                            return false;
                    }
                return true;
            }
            return false;
        }
        private void Build(Building entity)
        {
            int rot = 0;
            if(entity is IRotatable ro)
                rot=ro.GetAngle();
            entity = EntityFactory.CreateBuilding(entity, GetPerspectiveResource(entity),rot);
            entity.Activate();
            ChangeCollisionMap(entity,false);
            //_entityMap![(int)entity.Position.Y, (int)entity.Position.X] = entity;
            _buildings.Add(entity);
            if(entity is IPassable conveyor)
            {
                conveyor.BindNextEntities(_buildingMap);
            }
            //_entity = new Core(_loader, new Vector2(0, 0));
            _building = EntityFactory.CreateBuilding(_inputHandler.HotbarSelection,new Copper(0));
            if (_building is IRotatable rot1 && entity is IRotatable rot2)
            {
                rot1.SetAngle(rot2.GetAngle());
            }
            
        }

        private void Destroy(Building? entity)
        {
            if (entity!=null)
            {
                //_entityMap[(int)entity.Position.Y, (int)entity.Position.X] = null;
                _buildings.Remove(entity);
                ChangeCollisionMap(entity, true);
                entity.Dispose();
            }
        }
        private void ChangeCollisionMap(Building entity, bool value)
        {
            for (int i = (int)entity.Position.Y - (int)entity.Size.X / 2; i < (int)entity.Position.Y + entity.Size[0] / 2; i++)
                for (int j = (int)entity.Position.X - (int)entity.Size.X / 2; j < (int)entity.Position.X + entity.Size[0] / 2; j++)
                {
                    _collideMap![i, j] = value;
                    if (!value)
                        _buildingMap[i, j] = entity;
                    else
                        _buildingMap[i, j] = null;
                }
        }
        private bool IsInsideBounds(Building entity)
        {
            if ((int)entity.Mesh.Position.X-(int)entity.Size.X/2 < 0 || (int)entity.Mesh.Position.Z - (int)entity.Size.X/2 < 0)
                return false;
            if ((int)entity.Mesh.Position.Z + entity.Size[0]/2 > _buildingMap!.GetLength(0) ||
                (int)entity.Mesh.Position.X + entity.Size[0]/2 > _buildingMap.GetLength(1))
                return false;
            return true;
        }
        private GameResource GetPerspectiveResource(Building entity)
        {
            Inventory resources = new Inventory();
            for (int i = (int)entity.Mesh.Position.Z-(int)entity.Size.X/2; i < (int)entity.Mesh.Position.Z + entity.Size[0]/2; i++)
                for (int j = (int)entity.Mesh.Position.X - (int)entity.Size.X / 2; j < (int)entity.Mesh.Position.X + entity.Size[0]/2; j++)
                    if(_fullMap[1][i, j]!=Tile.None)
                        resources.Add(ResourceFactory.CreateResource(_fullMap[1][i, j], 1));
            return resources.GetMostPerspective();

        }

    }
}
