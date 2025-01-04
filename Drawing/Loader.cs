using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct3D11;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Buffer11 = SharpDX.Direct3D11.Buffer;

namespace SimpleDXApp
{
    public class Loader : IDisposable
    {
        private DirectX3DGraphics _directX3DGraphics;

        public Loader(DirectX3DGraphics directX3DGraphics)
        {
            _directX3DGraphics = directX3DGraphics;
        }
        public MeshObject MakeTileSquare(Vector4 position)
        {
            Renderer.VertexDataStruct[] vertices = new Renderer.VertexDataStruct[4]
            {
                new Renderer.VertexDataStruct
                {
                    position = new Vector4(position.X,0,position.Z,1.0f),
                    texCoord= new Vector2(0,0),
                },
                new Renderer.VertexDataStruct
                {
                    position = new Vector4(position.X+1,0,position.Z,1.0f),
                    texCoord= new Vector2(1,0),
                },
                new Renderer.VertexDataStruct
                {
                    position = new Vector4(position.X,0,position.Z+1,1.0f),
                    texCoord= new Vector2(0,1),
                },
                new Renderer.VertexDataStruct
                {
                    position = new Vector4(position.X+1,0,position.Z+1,1.0f),
                    texCoord= new Vector2(1,1),
                },
            };
            uint[] indices = new uint[]
            {
                0, 1, 2,    1,3,2,
            };
            Texture2D texture = TextureLoader.LoadTexture(_directX3DGraphics.Device, "Assets/Tiles/sand.png");
            ShaderResourceView textureView = new ShaderResourceView(_directX3DGraphics.Device, texture);
            var samplerDescription = new SamplerStateDescription()
            {
                Filter = Filter.MinMagMipLinear, // Линейная фильтрация
                AddressU = TextureAddressMode.Mirror, // Зацикливание текстуры по оси U
                AddressV = TextureAddressMode.Mirror, // Зацикливание текстуры по оси V
                AddressW = TextureAddressMode.Mirror, // Зацикливание текстуры по оси W
                ComparisonFunction = Comparison.Never,
                MinimumLod = 0,
                MaximumLod = float.MaxValue
            };
            var samplerState = new SamplerState(_directX3DGraphics.Device, samplerDescription);
            _directX3DGraphics.DeviceContext.PixelShader.SetShaderResources(0, textureView);
            _directX3DGraphics.DeviceContext.PixelShader.SetSampler(0, samplerState);
            return new MeshObject(_directX3DGraphics, position,
                0, 0, 0, vertices, indices);
        }
        public MeshObject MakeCube(Vector4 position, float yaw, float pitch, float roll)
        {
            Renderer.VertexDataStruct[] vertices =
               new Renderer.VertexDataStruct[8]
               {

                    new Renderer.VertexDataStruct // top 0
                    {
                        position = new Vector4(-1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.0f, 1.0f) //yellow
                    },
                    new Renderer.VertexDataStruct // top 1
                    {
                        position = new Vector4(-1.0f, 1.0f, -1.0f, 1.0f),
                        texCoord = new Vector2(0.0f, 0.0f) //red
                    },
                    new Renderer.VertexDataStruct // top 2
                    {
                        position = new Vector4(1.0f, 1.0f, -1.0f, 1.0f),
                        texCoord = new Vector2(1.0f, 0.0f)//cyan
                    },
                    new Renderer.VertexDataStruct // top 3
                    {
                        position = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(1.0f,1.0f)//magenta
                    },
                    new Renderer.VertexDataStruct // bottom 4
                    {
                        position = new Vector4(-1.0f, -1.0f, -1.0f, 1.0f),
                        texCoord = new Vector2(0.5f, 0.0f)//green
                    },
                    new Renderer.VertexDataStruct // bottom 5
                    {
                        position = new Vector4(-1.0f, -1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.0f, 0.5f)//blue
                    },
                    new Renderer.VertexDataStruct // bottom 6
                    {
                        position = new Vector4(1.0f, -1.0f, 1.0f, 1.0f), 
                        texCoord = new Vector2(0.5f, 1.0f)//black
                    },
                    new Renderer.VertexDataStruct // bottom 7
                    {
                        position = new Vector4(1.0f, -1.0f, -1.0f, 1.0f),
                        texCoord = new Vector2(1f, 0.5f)//white
                    }
               };
            uint[] indices = new uint[]
            {
                0, 1, 2,    0,2,3,
                4, 5,6,      4,6,7,
                3,7,6,
                0,3,6,
                3,2,7,
                0,6,5,
                2,4,7,
                1,4,2,
                1,5,4,
                1,0,5
            };
            // Создание вершинного буфера с данными

            Texture2D texture = TextureLoader.LoadTexture(_directX3DGraphics.Device, "Assets/Tiles/sand.png");
            ShaderResourceView textureView = new ShaderResourceView(_directX3DGraphics.Device, texture);
            var samplerDescription = new SamplerStateDescription()
            {
                Filter = Filter.MinMagMipLinear, // Линейная фильтрация
                AddressU = TextureAddressMode.Mirror, // Зацикливание текстуры по оси U
                AddressV = TextureAddressMode.Mirror, // Зацикливание текстуры по оси V
                AddressW = TextureAddressMode.Mirror, // Зацикливание текстуры по оси W
                ComparisonFunction = Comparison.Never,
                MinimumLod = 0,
                MaximumLod = float.MaxValue
            };
            var samplerState = new SamplerState(_directX3DGraphics.Device, samplerDescription);
            _directX3DGraphics.DeviceContext.PixelShader.SetShaderResources(0, textureView);
            _directX3DGraphics.DeviceContext.PixelShader.SetSampler(0, samplerState);
            return new MeshObject(_directX3DGraphics, position,
                yaw, pitch, roll, vertices, indices);
        }
        public Vector4 RotVector(Vector3 vec, float angle)
        {
            Matrix3x3 mat = new Matrix3x3(Convert.ToSingle(Math.Cos(angle)), 0, Convert.ToSingle(Math.Sin(angle)), 0, 1, 0, Convert.ToSingle(-Math.Sin(angle)), 0, Convert.ToSingle(Math.Cos(angle)));

            vec = Vector3.Transform(vec, mat);
            return new Vector4(vec, 1);
        }
        public MeshObject MakePlane(Vector4 position, float yaw, float pitch, float roll, uint n)
        {
            n++;
            Renderer.VertexDataStruct[] vertices = FillPlane(n, 20.0f / n);
            uint[] indices = FillIndices(n);
            return new MeshObject(_directX3DGraphics, position,
                yaw, pitch, roll, vertices, indices);
        }
        private Renderer.VertexDataStruct[] FillPlane(uint n, float step)
        {
            var verts = new Renderer.VertexDataStruct[n * n];
            float size = (n - 1) * step;
            float curX = -size / 2;
            float curY = -size / 2;
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    verts[n * i + j] = new Renderer.VertexDataStruct() { position = new Vector4(curX + i * step, 0, curY + j * step, 1), texCoord = new Vector2(0, 0) };
            return verts;
        }
        private uint[] FillIndices(uint n)
        {
            List<uint> indices = new List<uint>();
            uint ptr = 0;
            for (uint j = 0; j < (n - 1) * ((n - 1) * 6); j += (n - 1) * 6)
            {

                for (uint i = 0; i < (n - 1) * 6; i += 6)
                {
                    indices.Add(i / 6 + ptr);
                    indices.Add(i / 6 + n + ptr);
                    indices.Add(i / 6 + n + 1 + ptr);
                    indices.Add(i / 6 + ptr);
                    indices.Add(i / 6 + n + 1 + ptr);
                    indices.Add(i / 6 + 1 + ptr);
                }
                ptr += n;
            }
            return indices.ToArray();
        }
        public MeshObject MakeAntiprism(Vector4 position, float yaw, float pitch, float roll)
        {
            float ang = Convert.ToSingle(Math.Sqrt(2) / 2);
            Renderer.VertexDataStruct[] vertices =
                new Renderer.VertexDataStruct[8]
                {

                    new Renderer.VertexDataStruct // top 0
                    {
                        position = new Vector4(-1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(0.0f, 1.0f) //yellow
                    },
                    new Renderer.VertexDataStruct // top 1
                    {
                        position = new Vector4(-1.0f, 1.0f, -1.0f, 1.0f),
                        texCoord = new Vector2(0.0f, 0.0f) //red
                    },
                    new Renderer.VertexDataStruct // top 2
                    {
                        position = new Vector4(1.0f, 1.0f, -1.0f, 1.0f),
                        texCoord = new Vector2(1.0f, 0.0f)//cyan
                    },
                    new Renderer.VertexDataStruct // top 3
                    {
                        position = new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                        texCoord = new Vector2(1.0f,1.0f)//magenta
                    },
                    new Renderer.VertexDataStruct // bottom 4
                    {
                        position = RotVector( new Vector3(-1.0f, -1.0f, -1.0f), ang),
                        texCoord = new Vector2(0.5f, 0.0f)//green
                    },
                    new Renderer.VertexDataStruct // bottom 5
                    {
                        position = RotVector(new Vector3(-1.0f, -1.0f, 1.0f), ang),
                        texCoord = new Vector2(0.0f, 0.5f)//blue
                    },
                    new Renderer.VertexDataStruct // bottom 6
                    {
                        position = RotVector( new Vector3(1.0f, -1.0f, 1.0f), ang),
                        texCoord = new Vector2(0.5f, 1.0f)//black
                    },
                    new Renderer.VertexDataStruct // bottom 7
                    {
                        position = RotVector(new Vector3(1.0f, -1.0f, -1.0f), ang),
                        texCoord = new Vector2(1f, 0.5f)//white
                    }
                };
            uint[] indices = new uint[]
            {
                0, 1, 2,    0,2,3,
                4, 5,6,      4,6,7,
                3,7,6,
                0,3,6,
                3,2,7,
                0,6,5,
                2,4,7,
                1,4,2,
                1,5,4,
                1,0,5
            };
            // Создание вершинного буфера с данными

            Texture2D texture = TextureLoader.LoadTexture(_directX3DGraphics.Device, "D:\\work\\Course 3\\Term 1\\ПГиЗ\\lab2\\zay.jpg");
            ShaderResourceView textureView = new ShaderResourceView(_directX3DGraphics.Device, texture);
            var samplerDescription = new SamplerStateDescription()
            {
                Filter = Filter.MinMagMipLinear, // Линейная фильтрация
                AddressU = TextureAddressMode.Mirror, // Зацикливание текстуры по оси U
                AddressV = TextureAddressMode.Mirror, // Зацикливание текстуры по оси V
                AddressW = TextureAddressMode.Mirror, // Зацикливание текстуры по оси W
                ComparisonFunction = Comparison.Never,
                MinimumLod = 0,
                MaximumLod = float.MaxValue
            };
            var samplerState = new SamplerState(_directX3DGraphics.Device, samplerDescription);
            _directX3DGraphics.DeviceContext.PixelShader.SetShaderResources(0, textureView);
            _directX3DGraphics.DeviceContext.PixelShader.SetSampler(0, samplerState);
            return new MeshObject(_directX3DGraphics, position,
                yaw, pitch, roll, vertices, indices);
        }

        public void Dispose()
        {

        }
    }
}
