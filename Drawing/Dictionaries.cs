using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawing
{
    public static class Dictionaries
    {
        private static Dictionary<string,Tile> _keyValuePairs = new Dictionary<string, Tile>
        {
            { "#FFD08CFF",Tile.Sand },
            {"#656565FF",Tile.BlackSand }
        };
        private static Dictionary<Tile, string> _textures = new Dictionary<Tile, string>
        {
            { Tile.Sand, "Assets/Tiles/sand.png"},
            { Tile.BlackSand, "Assets/Tiles/sand2.png"}
        };
        public static Tile Color(string key)
        {
            try
            {
                return _keyValuePairs[key];
            }
            catch (Exception e) { return 0; };
        }
        public static string TexturePath(Tile key)
        { return _textures[key]; }
        public static int Types {  get { return _textures.Count; } }
    }
}
