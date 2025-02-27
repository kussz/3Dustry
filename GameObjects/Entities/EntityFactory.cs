using GameObjects.Drawing;
using GameObjects.Resources;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Entities
{
    public class EntityFactory
    {
        public static Building? CreateBuilding(Building entity,GameResource resource,int rot)
        {
            Vector2 pos = new Vector2(entity.Mesh.Position.X, entity.Mesh.Position.Z);
            switch(entity.Type)
            {
                case EntityType.Core:
                    return new Core(pos, TextureStorage.GetTextureHolder(EntityType.Core));
                case EntityType.Miner:
                    return new Miner(pos, resource, TextureStorage.GetTextureHolder(EntityType.Miner));
                case EntityType.Conveyor:
                    return new Conveyor(pos,TextureStorage.GetTextureHolder(EntityType.Conveyor),rot);
                case EntityType.Furnace:
                    return new Furnace(pos, TextureStorage.GetTextureHolder(EntityType.Furnace));
                default:
                    return null;
            }
        }
        public static Building? CreateBuilding(int number, GameResource resource)
        {
            switch (number)
            {
                case 1:
                    return new Core(Vector2.Zero, TextureStorage.GetTextureHolder(EntityType.Core));
                case 2:
                    return new Miner(Vector2.Zero, resource, TextureStorage.GetTextureHolder(EntityType.Miner));
                case 3:
                    return new Conveyor(Vector2.Zero, TextureStorage.GetTextureHolder(EntityType.Conveyor),0);
                case 4:
                    return new Furnace(Vector2.Zero, TextureStorage.GetTextureHolder(EntityType.Furnace));
                default:
                    return null;
            }
        }
    }
}
