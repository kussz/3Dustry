using SharpDX.Direct3D11;
using SharpDX.DirectInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Drawing
{
    public static class TextureStorage
    {
        private static DirectX3DGraphics _graphics;
        public static void Configure(DirectX3DGraphics graphics)
        {
            _graphics=graphics;
        }
        private static Dictionary<string, Tile> _keyValuePairs = new Dictionary<string, Tile>
        {
            {"#D2AE8D",Tile.Sand },
            {"#3C3838",Tile.BlackSand },
            {"#AE7C5B",Tile.Copper },
            {"#8E85A2",Tile.Lead }
        };
        private static Dictionary<EntityType, TextureHolder> _textures = new Dictionary<EntityType, TextureHolder>();
        public static Tile GetTileFromColorString(string key)
        {
            if (_keyValuePairs.TryGetValue(key, out Tile tile))
            {
                return tile;
            }
            return Tile.None; // Или используйте значение по умолчанию
        }
        //public static void SetTextureHolder(EntityType type, TextureHolder texture)
        //{
        //    _textures.Add(type, texture);
        //}
        public static TextureHolder? GetTextureHolder(EntityType type)
        {
            if (_textures.TryGetValue(type, out TextureHolder texture))
            {
                return texture;
            }
            _textures.Add(type,new TextureHolder(_graphics.Device,GetTexturePath(type)));
            return _textures[type];
        }
        private static Dictionary<Tile, string> _tileTexturePaths = new Dictionary<Tile, string>
        {
            { Tile.Sand, "Assets/Tiles/sand.png"},
            { Tile.BlackSand, "Assets/Tiles/sand2.png"},
            { Tile.Copper, "Assets/Tiles/Copper.png"},
            { Tile.Lead, "Assets/Tiles/Lead.png"},
        };
        private static Dictionary<EntityType, string> _entityTexturePaths = new Dictionary<EntityType, string>
        {
            {EntityType.Core,"Assets/Entities/Core"},
            {EntityType.Miner,"Assets/Entities/Miner"}
        };
        public static string GetTexturePath(Tile key)
        { return _tileTexturePaths[key]; }
        public static string GetTexturePath(EntityType key)
        { return _entityTexturePaths[key]; }
        public static int Types { get { return _tileTexturePaths.Count; } }
    }
}
