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
        public static Entity? CreateEntity(int number,GameResource resource)
        {
            switch(number)
            {
                case 1:
                    return new Core(new SharpDX.Vector2(0, 0), TextureStorage.GetTextureHolder(EntityType.Core));
                case 2:
                    return new Miner(new SharpDX.Vector2(0, 0), resource, TextureStorage.GetTextureHolder(EntityType.Miner));
                case 3:
                    return new Conveyor(new SharpDX.Vector2(0,0),TextureStorage.GetTextureHolder(EntityType.Conveyor));
                default:
                    return null;
            }
        }
    }
}
