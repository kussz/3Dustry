using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.WIC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Drawing
{
    public class MapLoader
    {
        DirectX3DGraphics _graphics;
        Loader _loader;

        public MapLoader(DirectX3DGraphics graphics, Loader loader)
        {
            _graphics = graphics;
            _loader = loader;
        }
        private Tile[,] GetTiles(string name, string type)
        {
            BitmapSource bitmap = TextureLoader.LoadBitmap("Assets/Maps/" + name + "/" + type + ".png");
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
                    string color = ColorToHex(r, g, b);

                    tiles[y, x] = TextureStorage.GetTileFromColorString(color);


                }
            }
            return tiles;
        }
        public Tile[][,] LoadMap(string name)
        {
            Tile[,] ground = GetTiles(name, "map");
            Tile[,] ores = GetTiles(name, "ores");
            return [ground, ores];
        }
        private string ColorToHex(byte r, byte g, byte b)
        {
            // Преобразуем значения в HEX
            return $"#{r:X2}{g:X2}{b:X2}"; // Формат RGBA
        }
        public MeshObject[] GetCompoundMap(Tile[,] map, float y)
        {
            List<MeshObject> compoundHolder = new List<MeshObject>();
            foreach (var tileType in map.Cast<Tile>().Distinct().Except([Tile.None]))
            {
                int indexOffset = 0;
                List<Renderer.VertexDataStruct> allVertices = new List<Renderer.VertexDataStruct>();
                List<uint> allIndices = new List<uint>();
                ShaderResourceView shaderResourceView = TextureLoader.GetFloorTexture(_graphics, tileType);
                for (int i = 0; i < map.GetLength(0); i++)
                    for (int j = 0; j < map.GetLength(1); j++)
                    {
                        if (map[i, j] == tileType)
                        {
                            MeshObject tile = _loader.MakeTileSquare(new Vector4(j, y, i, 1f));
                            allVertices.AddRange(tile.Vertices); // Предполагается, что у вас есть доступ к вершинам
                            allIndices.AddRange(tile.Indices.Select(idx => (uint)(idx + indexOffset))); // Корректируем индексы
                            indexOffset += tile.Vertices.Length;
                        }
                    }
                compoundHolder.Add(new MeshObject(_graphics, new Vector4(0, 0, 0, 1), 0, 0, 0, allVertices.ToArray(), allIndices.ToArray(), shaderResourceView));
            }
            return compoundHolder.ToArray();
        }
        public bool[,] GetCollideMap(Tile[,] map)
        {
            bool[,] m = new bool[map.GetLength(0), map.GetLength(1)];
            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                    if (map[i, j] != Tile.None)
                        m[i, j] = true;
            return m;
        }
    }
}
