using GameObjects.Drawing;
using GameObjects.Entities.Buildings.Concrete;
using GameObjects.Entities.Buildings.Abstract;
using GameObjects.Resources;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Entities
{
    public static class BuildingFactory
    {
        private static readonly Dictionary<EntityType, Func<Vector2, GameResource, int, Building>> _buildingCreators = new()
    {
        { EntityType.Core, (pos, resource, rot) => new Core(pos, TextureStorage.GetTextureHolder(EntityType.Core)) },
        { EntityType.Miner, (pos, resource, rot) => new Miner(pos, resource, TextureStorage.GetTextureHolder(EntityType.Miner)) },
        { EntityType.Conveyor, (pos, resource, rot) => new Conveyor(pos, TextureStorage.GetTextureHolder(EntityType.Conveyor), rot) },
        { EntityType.Furnace, (pos, resource, rot) => new Furnace(pos, TextureStorage.GetTextureHolder(EntityType.Furnace)) }
    };
        public static Building? CreateBuilding(Building entity,GameResource resource,int rot)
        {
            Vector2 pos = new Vector2(entity.Mesh.Position.X, entity.Mesh.Position.Z);

            if (_buildingCreators.TryGetValue(entity.Type, out var createFunc))
            {
                return createFunc(pos, resource, rot);
            }

            return null;
        }
        public static Building? CreateBuilding(int number, GameResource resource)
        {
            if (number > _buildingCreators.Count)
                return null;
            if (_buildingCreators.TryGetValue(_buildingCreators.Keys.ToList()[number-1], out var createFunc))
            {
                return createFunc(Vector2.Zero, resource, 0);
            }
            return null;
        }
    }
}
