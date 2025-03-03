using GameObjects.Drawing;
using GameObjects.Entities;
using GameObjects.Entities.Buildings.Abstract;
using GameObjects.Entities.Buildings.Concrete;
using System;
using System.Collections.Generic;

namespace GameObjects.Resources
{
    public class ResourceFactory
    {
        private static readonly Dictionary<Tile, Func<int, GameResource>> _tileMakers = new()
        {
            { Tile.Copper, quantity => new CopperOre(quantity) },
            { Tile.Lead, quantity => new LeadOre(quantity) },
            { Tile.Coal, quantity => new CoalOre(quantity) },
        };

        private static readonly Dictionary<ResourceType, Func<int, GameResource>> _resourceMakers = new()
        {
            { ResourceType.CopperOre, quantity => new CopperOre(quantity) },
            { ResourceType.LeadOre, quantity => new LeadOre(quantity) },
            { ResourceType.CoalOre, quantity => new CoalOre(quantity) },
            { ResourceType.Copper, quantity => new Copper(quantity) },
            { ResourceType.Lead, quantity => new Lead(quantity) },
        };

        public static GameResource CreateResource(Tile tileType, int quantity)
        {
            if (_tileMakers.TryGetValue(tileType, out var createFunc))
            {
                return createFunc(quantity);
            }

            return null;
        }

        public static GameResource CreateResource(ResourceType resourceType, int quantity)
        {
            if (_resourceMakers.TryGetValue(resourceType, out var createFunc))
            {
                return createFunc(quantity);
            }

            return null;
        }
    }
}