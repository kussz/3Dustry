using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.DXGI;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using SharpDX.Direct3D;
using Buffer11 = SharpDX.Direct3D11.Buffer;
using Device11 = SharpDX.Direct3D11.Device;
using SharpDX.Windows;
using SharpDX.DirectWrite;
using SharpDX.Direct2D1.Effects;
using GameObjects.Entities.Buildings.Abstract;
using GameObjects.Entities.Buildings.Concrete;

namespace GameObjects.Drawing
{
    public class Renderer : IDisposable
    {
        private float color = 1;
        [StructLayout(LayoutKind.Sequential)]
        public struct VertexDataStruct
        {
            public Vector4 position;
            public Vector2 texCoord;
            public Vector3 normal;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PerObjectConstantBuffer
        {
            public Matrix worldMatrix;
            public Matrix worldViewProjectionMatrix;
            public float isTransparent;
            public Vector3 campos;
            public int mainBuilding;
            public int isSelected;
            public int buildify;
            public float _padding;
        }
        public struct ConstantBuffer
        {
            public float Alpha;
        }
        private DirectX3DGraphics _directX3DGraphics;
        private Device11 _device;
        private DeviceContext _deviceContext;

        private VertexShader _vertexShader;
        private PixelShader _pixelShader;
        private ShaderSignature _shaderSignature;
        private InputLayout _inputLayout;

        private PerObjectConstantBuffer _perObjectConstantBuffer;
        private Buffer11? _perObjectConstantBufferObject;

        public Renderer()
        {
            _directX3DGraphics = DirectX3DGraphics.Instance;
            _device = _directX3DGraphics.Device;
            _deviceContext = _directX3DGraphics.DeviceContext;
            CompilationResult vertexShaderByteCode =
                ShaderBytecode.CompileFromFile("vertex.hlsl",
                "vertexShader", "vs_5_0");
            _vertexShader = new VertexShader(_device, vertexShaderByteCode);
            CompilationResult pixelShaderByteCode =
                ShaderBytecode.CompileFromFile("pixel.hlsl",
                "pixelShader", "ps_5_0");
            _pixelShader = new PixelShader(_device, pixelShaderByteCode);

            InputElement[] inputElements = new[]
            {
                new InputElement("POSITION", 0, Format.R32G32B32A32_Float,
                    0, 0),
                new InputElement("TEXCOORD", 0, Format.R32G32_Float,
                    16, 0),
                new InputElement("NORMAL",0,Format.R32G32B32_Float,24,0)
            };

            _shaderSignature = ShaderSignature.GetInputSignature(vertexShaderByteCode);

            _inputLayout = new InputLayout(_device, _shaderSignature, inputElements);

            Utilities.Dispose(ref vertexShaderByteCode);
            Utilities.Dispose(ref pixelShaderByteCode);

            _deviceContext.InputAssembler.InputLayout = _inputLayout;
            _deviceContext.VertexShader.Set(_vertexShader);
            _deviceContext.PixelShader.Set(_pixelShader);

        }

        public void CreateConstantBuffer()
        {
            _perObjectConstantBufferObject = new Buffer11(
                _device,
                Utilities.SizeOf<PerObjectConstantBuffer>(),
                ResourceUsage.Dynamic,
                BindFlags.ConstantBuffer,
                CpuAccessFlags.Write,
                ResourceOptionFlags.None,
                0);
        }
        public void SetMain(bool isMain)
        {
            _perObjectConstantBuffer.mainBuilding = Convert.ToInt32(isMain);
        }
        public void SetTransparent(float isTransparent)
        {
            _perObjectConstantBuffer.isTransparent = isTransparent;
        }
        public void SetCameraPosition(Vector4 camerapos)
        {
            _perObjectConstantBuffer.campos = new Vector3(camerapos.X, camerapos.Y, camerapos.Z);
        }
        public void SetBuilding(bool isBuilding)
        {
            _perObjectConstantBuffer.buildify = Convert.ToInt32(isBuilding);
        }
        public void SetSelected(bool isSelected)
        {
            int isSel = Convert.ToInt32(isSelected);
            _perObjectConstantBuffer.isSelected = isSel;
        }
        public void BeginLoadingRender()
        {
            _directX3DGraphics.ClearBuffers(new Color(new Vector3(color, color, color)));
            color -= 0.005f;

        }
        public void BeginRender()
        {
            _directX3DGraphics.ClearBuffers(Color.Black);
        }

        public void UpdatePerObjectConstantBuffers(Matrix world, Matrix view,
            Matrix projection)
        {
            _perObjectConstantBuffer.worldMatrix = world;
            _perObjectConstantBuffer.worldMatrix.Transpose();

            _perObjectConstantBuffer.worldViewProjectionMatrix =
                Matrix.Multiply(Matrix.Multiply(world, view), projection);
            _perObjectConstantBuffer.worldViewProjectionMatrix.Transpose();
            DataStream dataStream;
            _deviceContext.MapSubresource(
                _perObjectConstantBufferObject,
                MapMode.WriteDiscard,
                SharpDX.Direct3D11.MapFlags.None,
                out dataStream);
            dataStream.Write(_perObjectConstantBuffer);
            _deviceContext.UnmapSubresource(_perObjectConstantBufferObject, 0);
            _deviceContext.VertexShader.SetConstantBuffer(0, _perObjectConstantBufferObject);
        }

        public void RenderMeshObject(MeshObject meshObject)
        {
            _deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
            _deviceContext.InputAssembler.SetVertexBuffers(0,
                meshObject.VertexBufferBinding);
            _deviceContext.InputAssembler.SetIndexBuffer(meshObject.IndicesBufferObject,
                Format.R32_UInt, 0);
            _deviceContext.DrawIndexed(meshObject.IndicesCount, 0, 0);
        }

        public void EndRender()
        {
            _directX3DGraphics.SwapChain.Present(1, PresentFlags.Restart);
        }

        public void Dispose()
        {
            Utilities.Dispose(ref _perObjectConstantBufferObject);
            Utilities.Dispose(ref _inputLayout);
            Utilities.Dispose(ref _shaderSignature);
            Utilities.Dispose(ref _pixelShader);
            Utilities.Dispose(ref _vertexShader);
        }

        public void RenderText(TextObject text,float aspect)
        {
            _directX3DGraphics.DeviceContext.PixelShader.SetShaderResources(0, text.Texture);
            SetTransparent(-1);
            UpdatePerObjectConstantBuffers(text.GetWorldMatrix(), Matrix.Identity, Matrix.OrthoLH(aspect * 2f, 2f, 0.1f, 100.0f));
            RenderMeshObject(text);
        }
        public void RenderMenuItem(MenuTile tile,float aspect)
        {
            _directX3DGraphics.DeviceContext.PixelShader.SetShaderResources(0, tile.Mesh.Texture);
            SetTransparent(-1);
            UpdatePerObjectConstantBuffers(tile.Mesh.GetWorldMatrix(), Matrix.Identity, Matrix.OrthoLH(aspect * 2f, 2f, 0.1f, 100.0f));
            RenderMeshObject(tile.Mesh);
        }
        public void RenderEntity(Building building,Matrix viewMatrix,Matrix projectionMatrix,bool isSelected,float DeltaT)
        {
            SetSelected(isSelected);
            SetTransparent(building.BuildProgress);
            _directX3DGraphics.DeviceContext.PixelShader.SetShaderResources(0, building.TextureHolder.GetCurrentFrame(building.State));
            UpdatePerObjectConstantBuffers(building.Mesh.GetWorldMatrix(),
                viewMatrix, projectionMatrix);
            RenderMeshObject(building.Mesh);
        }
        public void RenderCompound(MeshObject[] compound, Matrix viewMatrix, Matrix projectionMatrix)
        {
            foreach (var comp in compound!)
            {
                _directX3DGraphics.DeviceContext.PixelShader.SetShaderResources(0, comp.Texture);
                UpdatePerObjectConstantBuffers(comp.GetWorldMatrix(),
                    viewMatrix, projectionMatrix);
                RenderMeshObject(comp);

            }
        }
        public void RenderConveyorTiles(Conveyor conveyor, Matrix viewMatrix, Matrix projectionMatrix)
        {
            foreach ( var tile in conveyor.Resources)
            {
                _directX3DGraphics.DeviceContext.PixelShader.SetShaderResources(0, tile.Texture);
                UpdatePerObjectConstantBuffers(tile.Mesh.GetWorldMatrix(),
                    viewMatrix, projectionMatrix);
                RenderMeshObject(tile.Mesh);
            }
        }
    }
}
