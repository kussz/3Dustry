using GameObjects.Drawing;
using GameObjects.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Entities
{
    public class EntityFactory
    {
        private static Loader _loader;
        public static void Configure(Loader loader)
        {
            _loader = loader;
        }
        public static Entity? CreateEntity(int number,GameResource resource)
        {
            switch(number)
            {
                case 1:
                    return new Core(_loader, new SharpDX.Vector2(0, 0), TextureStorage.GetTextureHolder(EntityType.Core));
                case 2:
                    return new Miner(_loader, new SharpDX.Vector2(0, 0), resource, TextureStorage.GetTextureHolder(EntityType.Miner));
                default:
                    return null;
            }
        }
    }
}
