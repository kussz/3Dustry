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
        private static DirectX3DGraphics? _graphics = DirectX3DGraphics.Instance;
        private static Dictionary<string, Tile> _keyValuePairs = new Dictionary<string, Tile>
        {
            {"#D2AE8D",Tile.Sand },
            {"#3C3838",Tile.BlackSand },
            {"#AE7C5B",Tile.Copper },
            {"#8E85A2",Tile.Lead },
            {"#242424", Tile.Coal },
        };
        //private static Dictionary<EntityType, TextureMetaData> _syncMetaDatas
        private static Dictionary<EntityType, TextureHolder> _entityTextures = new Dictionary<EntityType, TextureHolder>();
        private static Dictionary<Tile, ShaderResourceView> _tileTextures = new Dictionary<Tile, ShaderResourceView>();
        private static Dictionary<ResourceType, ShaderResourceView> _resTextures = new Dictionary<ResourceType, ShaderResourceView>();
        private static ShaderResourceView _textTile;
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
            if (_entityTextures.TryGetValue(type, out TextureHolder? texture))
            {
                return texture;
            }
            _entityTextures.Add(type,new TextureHolder(_graphics!.Device,GetTexturePath(type)));
            return _entityTextures[type];
        }
        public static ShaderResourceView GetTextTile()
        {
            if (_textTile == null)
                _textTile = TextureLoader.GetTexture(_graphics,"Assets/Font/GameFont.png");
            return _textTile;
        }
        public static ShaderResourceView? GetTexture(Tile type)
        {
            if (_tileTextures.TryGetValue(type, out ShaderResourceView? texture))
            {
                return texture;
            }
            _tileTextures.Add(type, TextureLoader.GetFloorTexture(_graphics!,type));
            return _tileTextures[type];
        }
        public static ShaderResourceView? GetTexture(ResourceType type)
<<<<<<< HEAD
        {
            if (_resTextures.TryGetValue(type, out ShaderResourceView? texture))
            {
                return texture;
            }
            _resTextures.Add(type, TextureLoader.GetResourceTexture(_graphics!, type));
            return _resTextures[type];
        }
        public static string GetTexturePath(Tile key)
        { return $"Assets/Tiles/{Enum.GetName(key)}.png"; }
        public static string GetTexturePath(ResourceType key)
        { return $"Assets/Resources/{Enum.GetName(key)}.png"; }

=======
        {
            if (_resTextures.TryGetValue(type, out ShaderResourceView? texture))
            {
                return texture;
            }
            _resTextures.Add(type, TextureLoader.GetResourceTexture(_graphics!, type));
            return _resTextures[type];
        }
        private static Dictionary<Tile, string> _tileTexturePaths = new Dictionary<Tile, string>
        {
            { Tile.Sand, "Assets/Tiles/sand.png"},
            { Tile.BlackSand, "Assets/Tiles/sand2.png"},
            { Tile.Copper, "Assets/Tiles/Copper.png"},
            { Tile.Lead, "Assets/Tiles/Lead.png"},
        };
        private static Dictionary<ResourceType, string> _resTexturePaths = new Dictionary<ResourceType, string>
        {
            { ResourceType.CopperOre, "Assets/Resources/CopperOre.png"},
            { ResourceType.LeadOre, "Assets/Resources/LeadOre.png"},
        };
        private static Dictionary<EntityType, string> _entityTexturePaths = new Dictionary<EntityType, string>
        {
            {EntityType.Core,"Assets/Entities/Core"},
            {EntityType.Miner,"Assets/Entities/Miner"},
            {EntityType.Conveyor,"Assets/Entities/Conveyor" },
            {EntityType.Furnace,"Assets/Entities/Furnace" },
        };
        public static string GetTexturePath(Tile key)
        { return _tileTexturePaths[key]; }
        public static string GetTexturePath(ResourceType key)
        { return _resTexturePaths[key]; }
>>>>>>> 478e56b3c91a0692ab06e996ec5a31f79e259c01
        public static string GetTexturePath(EntityType key)
        { return $"Assets/Entities/{Enum.GetName(key)}"; }

    }
}
