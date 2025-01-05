using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameObjects.Drawing;
using GameObjects.Entities;
using SharpDX;

using SharpDX.Direct3D11;
using SharpDX.DirectWrite;
using SharpDX.DXGI;
using SharpDX.Windows;

namespace GameObjects.GameLogic
{
    public class Game : IDisposable
    {
        private RenderForm _renderForm;
        private const float MOVE_STEP = 0.01f;
        private MeshObject _cube;
        private MeshObject[,] _cubes;
        private MeshObject[] _groundCompound;
        private MeshObject[] _oreCompound;
        private Camera _camera;

        private Loader _loader;
        Entity _entity;
        List<Entity> _entities;
        Entity[,] _entityMap;
        Tile[][,] _fullMap;
        bool[,] _collideMap;
        private DirectX3DGraphics _directX3DGraphics;
        private Renderer _renderer;
        private InputHandler _inputHandler;
        private float _a = 1;
        private float _b = 1;
        private float _x;
        private float _y;
        private TimeHelper _timeHelper;

        public Game()
        {
            _renderForm = new RenderForm();
            _renderForm.UserResized += RenderFormResizedCallback;
            _directX3DGraphics = new DirectX3DGraphics(_renderForm);

            _renderer = new Renderer(_directX3DGraphics);
            _renderer.CreateConstantBuffer();
            _inputHandler = new InputHandler();
            _loader = new Loader(_directX3DGraphics);
            _entity = new Core(_loader,new Vector2(0,0));
            MapLoader mapLoader = new MapLoader(_directX3DGraphics, _loader);
            _fullMap = mapLoader.LoadMap("map2");
            _collideMap = mapLoader.GetCollideMap(_fullMap[0]);
            _entityMap=new Entity[_fullMap[0].GetLength(0), _fullMap[0].GetLength(1)];
            _entities = new List<Entity>();
            _groundCompound = mapLoader.GetCompoundMap(_fullMap[0], 0);
            _oreCompound = mapLoader.GetCompoundMap(_fullMap[1], 0f);
            _camera = new Camera(new Vector4(_fullMap[0].GetLength(0)/2, 5.0f, _fullMap[0].GetLength(1)/2, 1.0f));
            _timeHelper = new TimeHelper();
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
            var center = _renderForm.PointToScreen(new System.Drawing.Point(
                    _renderForm.Width / 2,
                    _renderForm.Height / 2
                ));
            Cursor.Position = center;
            Cursor.Hide();

            if (_inputHandler.Escape)
                Environment.Exit(0);
            if (_firstRun)
            {
                RenderFormResizedCallback(this, EventArgs.Empty);
                _firstRun = false;
            }
            float xstep = 0;
            float ystep = 0;
            float zstep = 0;
            float astep = 0;
            float bstep = 0;
            float step = MOVE_STEP * (_inputHandler.Shift ? 2 : 1);
            float numStep = 0.05f;
            _inputHandler.Update();
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
            if(_inputHandler.MouseClick&&IsBuildable(_entity))
            {
                Build(_entity);
            }
            _camera.MoveBy(ystep * (float)Math.Sin(_camera._yaw), zstep, ystep * (float)Math.Cos(_camera._yaw));
            _camera.MoveBy(xstep * (float)Math.Sin(_camera._yaw - Math.PI / 2), zstep, xstep * (float)Math.Cos(_camera._yaw - Math.PI / 2));
            _camera.PitchBy(_inputHandler.MouseY);
            _camera.YawBy(_inputHandler.MouseX);
            _timeHelper.Update();
            //_renderForm.Text = "FPS: " + _timeHelper.FPS.ToString();
            
            Matrix viewMatrix = _camera.GetViewMatrix();
            Matrix projectionMatrix = _camera.GetProjectionMatrix();
            Vector3 hitPoint = _camera.IntersectRayWithPlane(40f, _entity.Size[1]);
            Vector3 hitPointDiscrete = new Vector3((float)Math.Floor(hitPoint.X - _entity.Size.X / 2), 0, (float)Math.Floor(hitPoint.Z - _entity.Size.Y / 2));
            _entity.Mesh.MoveTo(hitPointDiscrete);


            _renderForm.Text = "X: " + _entity.Mesh.Position.X + " Y: " + _camera.Pitch + " Z: " + _entity.Mesh.Position.Z;
            _renderer.BeginRender();
            _renderer.SetPerObjectConstantBuffer(0);
            foreach (var comp in _groundCompound)
            {
                _directX3DGraphics.DeviceContext.PixelShader.SetShaderResources(0, comp.Texture);
                _renderer.UpdatePerObjectConstantBuffers(comp.GetWorldMatrix(),
                    viewMatrix, projectionMatrix);
                _renderer.RenderMeshObject(comp);

            }
            _directX3DGraphics.DisableDepthTest();
            foreach (var comp in _oreCompound)
            {
                _directX3DGraphics.DeviceContext.PixelShader.SetShaderResources(0, comp.Texture);
                _renderer.UpdatePerObjectConstantBuffers(comp.GetWorldMatrix(),
                    viewMatrix, projectionMatrix);
                _renderer.RenderMeshObject(comp);

            }
            _directX3DGraphics.EnableDepthTest();

            foreach (var entity in _entities)
            {
                ShaderResourceView textur = Dictionaries.GetTexture(_entity.Type);
                if (textur == null)
                {
                    Dictionaries.SetTexture(_entity.Type, TextureLoader.GetEntityTexture(_directX3DGraphics, _entity.Type));
                    textur = Dictionaries.GetTexture(_entity.Type);
                }
                _renderer.SetPerObjectConstantBuffer(Convert.ToInt32(!entity.IsBuilt));
                _directX3DGraphics.DeviceContext.PixelShader.SetShaderResources(0, textur);
                _renderer.UpdatePerObjectConstantBuffers(entity.Mesh.GetWorldMatrix(),
                    viewMatrix, projectionMatrix);
                _renderer.RenderMeshObject(entity.Mesh);
                
            }
            ShaderResourceView texture = Dictionaries.GetTexture(_entity.Type);
            if(texture == null)
            {
                Dictionaries.SetTexture(_entity.Type,TextureLoader.GetEntityTexture(_directX3DGraphics,_entity.Type));
                texture = Dictionaries.GetTexture(_entity.Type);
            }
            if(IsBuildable(_entity))
            {
                _directX3DGraphics.DeviceContext.PixelShader.SetShaderResources(0, texture);
                _renderer.SetPerObjectConstantBuffer(1);
                _renderer.UpdatePerObjectConstantBuffers(_entity.Mesh.GetWorldMatrix(),
                    viewMatrix, projectionMatrix);
                _renderer.RenderMeshObject(_entity.Mesh);
            }
            



            //foreach (var cube in _cubes)
            //{
            //    _renderer.UpdatePerObjectConstantBuffers(cube.GetWorldMatrix(),
            //    viewMatrix, projectionMatrix);
            //    _renderer.RenderMeshObject(cube);

            //}
            //_renderer.RenderMeshObject(_cube);

            _renderer.EndRender();
        }

        public void Run()
        {
            RenderLoop.Run(_renderForm, RenderLoopCallback);
        }

        public void Dispose()
        {
            _renderer.Dispose();
            _directX3DGraphics.Dispose();
        }
        private bool IsBuildable(Entity entity)
        {
            Vector3 lookingPoint = (Vector3)entity.Mesh.Position;
            if((int)lookingPoint.X<0||(int)lookingPoint.Z<0)
                return false;
            if ((int)lookingPoint.Z + entity.Size[0] > _entityMap.GetLength(0) ||
                (int)lookingPoint.X + entity.Size[0] > _entityMap.GetLength(1))
                return false;
            for (int i = (int)entity.Mesh.Position.Z; i < (int)entity.Mesh.Position.Z+ entity.Size[0];i++)
                for(int j = (int)entity.Mesh.Position.X; j < (int)entity.Mesh.Position.X+ entity.Size[0];j++)
                {
                    if (_collideMap[i,j]==false)
                        return false;
                }
            return true;
        }
        private void Build(Entity entity)
        {
            entity.IsBuilt = true;
            entity.Position = new(entity.Mesh.Position.X, entity.Mesh.Position.Z);
            for(int i=(int)entity.Position.Y;i< (int)entity.Position.Y + entity.Size[0];i++)
                for(int j=(int)entity.Position.X;j< (int)entity.Position.X + entity.Size[0];j++)
                    _collideMap[i,j] = false;
            _entityMap[(int)entity.Position.Y, (int)entity.Position.X] = entity;
            _entities.Add(entity);
            _entity = new Core(_loader, new Vector2(0, 0));
        }




    }
}
