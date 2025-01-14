using SharpDX;
//using SharpDX.Direct2D1.Effects;
using SharpDX.Direct3D11;
using SharpDX.WIC;

namespace GameObjects.Drawing
{
    public class Loader : IDisposable
    {
        private DirectX3DGraphics _directX3DGraphics;

        public Loader(DirectX3DGraphics directX3DGraphics)
        {
            _directX3DGraphics = directX3DGraphics;
        }
        public MeshObject MakeMenuTile(float aspect,float size,Vector2 position)
        {
            float aspectX;
            float aspectY;
            //size /= 2;
            if(aspect>=1)
            {
                aspectX = size;
                aspectY = size / aspect;
            }
            else
            {
                aspectX = aspect*size;
                aspectY = size;
            }
            Renderer.VertexDataStruct[] vertices =
            {
                new Renderer.VertexDataStruct
                {
                    position = new Vector4(-aspectX,-aspectY,1,1.0f),
                    texCoord= new Vector2(0,1),
                },
                new Renderer.VertexDataStruct
                {
                    position = new Vector4(+aspectX,-aspectY,1,1.0f),
                    texCoord= new Vector2(1,1),
                },
                new Renderer.VertexDataStruct
                {
                    position = new Vector4(-aspectX,+aspectY,1,1.0f),
                    texCoord= new Vector2(0,0),
                },
                new Renderer.VertexDataStruct
                {
                    position = new Vector4(+aspectX,+aspectY,1,1.0f),
                    texCoord= new Vector2(1,0),
                },
            };
            uint[] indices = new uint[]
            {
                0, 1, 2,    1,3,2,
            };



            return new MeshObject(_directX3DGraphics,new Vector4(position.X,position.Y,0,1),
                0, 0, 0, vertices, indices);
        }
        public MeshObject MakeTileSquare(Vector4 position)
        {
            Renderer.VertexDataStruct[] vertices = new Renderer.VertexDataStruct[4]
            {
                new Renderer.VertexDataStruct
                {
                    position = new Vector4(position.X,position.Y,position.Z,1.0f),
                    texCoord= new Vector2(0,0),
                },
                new Renderer.VertexDataStruct
                {
                    position = new Vector4(position.X+1,position.Y,position.Z,1.0f),
                    texCoord= new Vector2(1,0),
                },
                new Renderer.VertexDataStruct
                {
                    position = new Vector4(position.X,position.Y,position.Z+1,1.0f),
                    texCoord= new Vector2(0,1),
                },
                new Renderer.VertexDataStruct
                {
                    position = new Vector4(position.X+1,position.Y,position.Z+1,1.0f),
                    texCoord= new Vector2(1,1),
                },
            };
            uint[] indices = new uint[]
            {
                0, 1, 2,    1,3,2,
            };



            return new MeshObject(_directX3DGraphics, position,
                0, 0, 0, vertices, indices);
        }
        public MeshObject MakeCube(Vector4 position,Vector2 size, float yaw, float pitch, float roll)
        {
            float deltaWidth = size[0];
            float height = size[1];
            Renderer.VertexDataStruct[] vertices =
               new Renderer.VertexDataStruct[]
               {

                    new Renderer.VertexDataStruct // top 0
                    {
                        position = new Vector4(-deltaWidth/2, height, deltaWidth/2, 1.0f),
                        texCoord = new Vector2(3/4f,3/4f) //yellow
                    },
                    new Renderer.VertexDataStruct // top 1
                    {
                        position = new Vector4(-deltaWidth/2, height, -deltaWidth/2, 1.0f),
                        texCoord = new Vector2(3/4f, 1/4f) //red
                    },
                    new Renderer.VertexDataStruct // top 2
                    {
                        position = new Vector4(deltaWidth/2, height, -deltaWidth/2, 1.0f),
                        texCoord = new Vector2(1/4f, 1/4f)//cyan
                    },
                    new Renderer.VertexDataStruct // top 3
                    {
                        position = new Vector4(deltaWidth/2, height, deltaWidth/2, 1.0f),
                        texCoord = new Vector2(1/4f,3/4f)//magenta
                    },

                    new Renderer.VertexDataStruct // bottom 4
                    {
                        position = new Vector4(-deltaWidth/2, 0, -deltaWidth/2, 1.0f),
                        texCoord = new Vector2(1f, 1/4f)//green
                    },
                    new Renderer.VertexDataStruct // bottom 45 (5)
                    {
                        position = new Vector4(-deltaWidth/2, 0, -deltaWidth/2, 1.0f),
                        texCoord = new Vector2(3/4f, 0.0f)//green
                    },
                    new Renderer.VertexDataStruct // bottom 5 (6)
                    {
                        position = new Vector4(-deltaWidth/2, 0,deltaWidth/2, 1.0f),
                        texCoord = new Vector2(1.0f, 3/4f)//blue
                    },
                    new Renderer.VertexDataStruct // bottom 56 (7)
                    {
                        position = new Vector4(-deltaWidth/2, 0,deltaWidth/2, 1.0f),
                        texCoord = new Vector2(3 / 4f, 1f)//blue
                    },
                    new Renderer.VertexDataStruct // bottom 6 (8)
                    {
                        position = new Vector4(deltaWidth/2, 0, deltaWidth/2, 1.0f),
                        texCoord = new Vector2(1 / 4f, 1.0f)//black
                    },
                    new Renderer.VertexDataStruct // bottom 67 (9)
                    {
                        position = new Vector4(deltaWidth/2, 0, deltaWidth/2, 1.0f),
                        texCoord = new Vector2(0f, 3 / 4f)//black
                    },
                    new Renderer.VertexDataStruct // bottom 7 (10)
                    {
                        position = new Vector4(deltaWidth/2, 0, -deltaWidth/2, 1.0f),
                        texCoord = new Vector2(0f, 1/ 4f)//white
                    },
                    new Renderer.VertexDataStruct // bottom 78 (11)
                    {
                        position = new Vector4(deltaWidth/2, 0, -deltaWidth/2, 1.0f),
                        texCoord = new Vector2(1 / 4f, 0f)//white
                    }
               };
            uint[] indices = new uint[]
            {
                0, 1, 2,    0,2,3,
                //4, 6,8,      4,8,10,

                3,10,9,
                2,10,3,

                3,8,7,
                0,3,7,

                1,0,6,
                1,6,4,

                11,2,1,
                11,1,5
            };
            // Создание вершинного буфера с данными
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
            for (uint j = 0; j < (n - 1) * (n - 1) * 6; j += (n - 1) * 6)
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
                Filter = Filter.MinMagMipPoint, // Линейная фильтрация
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

        //public static Color[,] BitmapSourceTo2DArray(SharpDX.WIC.BitmapSource bitmapSource)
        //{
        //    // Получаем размеры изображения
        //    int width = bitmapSource.Size.Width;
        //    int height = bitmapSource.Size.Height;

        //    // Проверка формата изображения
        //    var pixelFormat = bitmapSource.PixelFormat;

        //    // Выделяем память для одномерного массива пикселей
        //    int stride = width * 4; // 4 байта на пиксель (RGBA)
        //    var pixelArray = new byte[height * stride];

        //    // Копируем пиксели из BitmapSource в массив
        //    bitmapSource.CopyPixels(pixelArray, stride);

        //    // Создаем двумерный массив
        //    Color[,] colorArray = new Color[height, width];

        //    // Заполняем двумерный массив цветами
        //    for (int y = 0; y < height; y++)
        //    {
        //        for (int x = 0; x < width; x++)
        //        {
        //            int index = (y * width + x) * 4;
        //            byte b = pixelArray[index];     // Blue
        //            byte g = pixelArray[index + 1]; // Green
        //            byte r = pixelArray[index + 2]; // Red
        //            byte a = pixelArray[index + 3]; // Alpha

        //            // Создаем объект Color (или используйте другую структуру)
        //            colorArray[y, x] = Color.FromArgb(a, r, g, b);
        //        }
        //    }

        //    return colorArray;
        //}
        public void Dispose()
        {

        }
    }
}
