using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameObjects.Drawing;
using GameObjects.Entities;
using SharpDX;

using SharpDX.Direct3D11;
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

        Entity entity;
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
            Loader loader = new Loader(_directX3DGraphics);
            entity = new Core(loader);
            MapLoader mapLoader = new MapLoader(_directX3DGraphics, loader);
            Tile[][,] fullmap = mapLoader.LoadMap("map2");
            _groundCompound = mapLoader.GetCompoundMap(fullmap[0], 0);
            _oreCompound = mapLoader.GetCompoundMap(fullmap[1], 0.1f);
            _camera = new Camera(new Vector4(0.5f, 2.0f, -5.0f, 1.0f));
            _timeHelper = new TimeHelper();
            loader.Dispose();
            loader = null;
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
            //if (_inputHandler.Aplus)
            //    astep += numStep;
            //if (_inputHandler.Aminus)
            //    astep -= numStep;
            //if (_inputHandler.Bplus)
            //    bstep += numStep;
            //if (_inputHandler.Bminus)
            //    bstep -= numStep;
            //_a += astep;
            //_b += bstep;
            //if (_a < 0.1f)
            //    _a = 0.1f;
            //if (_b < 0.1f)
            //    _b = 0.1f;
            _camera.MoveBy(ystep * (float)Math.Sin(_camera._yaw), zstep, ystep * (float)Math.Cos(_camera._yaw));
            _camera.MoveBy(xstep * (float)Math.Sin(_camera._yaw - Math.PI / 2), zstep, xstep * (float)Math.Cos(_camera._yaw - Math.PI / 2));
            _camera.PitchBy(_inputHandler.MouseY);
            _camera.YawBy(_inputHandler.MouseX);
            _timeHelper.Update();
            //_renderForm.Text = "FPS: " + _timeHelper.FPS.ToString();
            
            Matrix viewMatrix = _camera.GetViewMatrix();
            Matrix projectionMatrix = _camera.GetProjectionMatrix();
            Vector3 hitPoint = _camera.IntersectRayWithPlane(_directX3DGraphics.DeviceContext.Rasterizer.GetViewports<Viewport>()[0]);
            entity.Mesh.Position=new Vector4(hitPoint.X,0, hitPoint.Z,1);
            _renderForm.Text = "X: " + _camera.Yaw + " Y: " + _camera.Pitch + " Z: " + hitPoint.Z;
            _renderer.BeginRender();

            _renderer.SetPerObjectConstantBuffer(_a, _b);
            foreach (var comp in _groundCompound)
            {
                _directX3DGraphics.DeviceContext.PixelShader.SetShaderResources(0, comp.Texture);
                _renderer.UpdatePerObjectConstantBuffers(comp.GetWorldMatrix(),
                    viewMatrix, projectionMatrix);
                _renderer.RenderMeshObject(comp);

            }
            foreach (var comp in _oreCompound)
            {
                _directX3DGraphics.DeviceContext.PixelShader.SetShaderResources(0, comp.Texture);
                _renderer.UpdatePerObjectConstantBuffers(comp.GetWorldMatrix(),
                    viewMatrix, projectionMatrix);
                _renderer.RenderMeshObject(comp);

            }
            ShaderResourceView texture = Dictionaries.GetTexture(entity.Type);
            if(texture == null)
            {
                Dictionaries.SetTexture(entity.Type,TextureLoader.GetEntityTexture(_directX3DGraphics,entity.Type));
                texture = Dictionaries.GetTexture(entity.Type);
            }
            _directX3DGraphics.DeviceContext.PixelShader.SetShaderResources(0, texture);
            _renderer.UpdatePerObjectConstantBuffers(entity.Mesh.GetWorldMatrix(),
                viewMatrix, projectionMatrix);
            _renderer.RenderMeshObject(entity.Mesh);



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
    }
}
