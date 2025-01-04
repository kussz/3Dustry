using SharpDX;
//using SharpDX.Direct2D1.Effects;
using SharpDX.Direct3D11;
using SharpDX.WIC;

namespace Drawing
{
    public class Loader : IDisposable
    {
        private DirectX3DGraphics _directX3DGraphics;

        public Loader(DirectX3DGraphics directX3DGraphics)
        {
            _directX3DGraphics = directX3DGraphics;
        }
        public MeshObject MakeTileSquare(Vector4 position,Tile type)
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
            Texture2D texture = TextureLoader.LoadTexture(_directX3DGraphics.Device, Dictionaries.TexturePath(type));
            ShaderResourceView textureView = new ShaderResourceView(_directX3DGraphics.Device, texture);
            
            
            return new MeshObject(_directX3DGraphics, position,
                0, 0, 0, vertices, indices,textureView);
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
        public Tile[,] LoadMap(string path)
        {
            BitmapSource bitmap = TextureLoader.LoadBitmap(new SharpDX.WIC.ImagingFactory2(), path);
            int width = bitmap.Size.Width;
            int height = bitmap.Size.Height;
            int stride = width * 4; // 4 байта на пиксель (RGBA)
            // Выделяем память для одномерного массива пикселей
            int pixelCount = width * height;
            var pixelArray = new byte[height * stride]; // 4 байта на пиксель (RGBA)

            // Копируем пиксели из BitmapSource в массив
            bitmap.CopyPixels(pixelArray, stride);

            // Создаем двумерный массив
            Tile[,] tiles = new Tile[height, width];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = (y * width + x) * 4;
                    byte r = pixelArray[index];     // Blue
                    byte g = pixelArray[index + 1]; // Green
                    byte b = pixelArray[index + 2]; // Red
                    byte a = pixelArray[index + 3]; // Alpha
                    string color = ColorToHex(r, g, b, a);

                    tiles[y, x] = Dictionaries.Color(color);


                }
            }
            return tiles;
        }
        private static string ColorToHex(byte r, byte g, byte b, byte a)
        {
            // Преобразуем значения в HEX
            return $"#{r:X2}{g:X2}{b:X2}{a:X2}"; // Формат RGBA
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
