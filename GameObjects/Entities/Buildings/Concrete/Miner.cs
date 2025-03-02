using GameObjects.Drawing;
using GameObjects.Entities.Buildings.Abstract;
using GameObjects.Interfaces;
using GameObjects.Resources;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Entities.Buildings.Concrete
{
    public class Miner : Building, IPassable
    {
        public List<Building> NextEntities { get; set; }
        private int _nextEntity = 0;
        private GameResource _resource;
        public Miner(Vector2 position, GameResource resource, TextureHolder textureHolder) : base(position, new Vector2(2, 3), resource == null ? 0 : resource.Quantity / 2f, textureHolder)
        {
            Cost = new GameLogic.Inventory(new CopperOre(20));
            Type = EntityType.Miner;
            _resource = resource;
            Inventory.MaxItems = 10;
            NextEntities = new List<Building>();
            Initialize();
        }
        protected override void IntervalWork()
        {
            Inventory.Add(ResourceFactory.CreateResource(_resource.Type, 1));

        }
        protected override void TickWork()
        {
            if (_resource != null)
            {
                if (NextEntities.Count > 0 && Inventory.GetCount(_resource.Type) > 0)
                {
                    if (_nextEntity >= NextEntities.Count)
                    {
                        _nextEntity %= NextEntities.Count;
                    }
                    Pass(new ResourceTile(Inventory.GetResource(_resource.Type)));
                }
            }
        }


        public void Pass(ResourceTile tile, int progress = 25)
        {
            if (NextEntities[_nextEntity] is Conveyor con)
            {
                tile.Progress = progress;
                con.Resources.Add(tile);
                Inventory.Subtract(tile.LogicResource);
            }
            _nextEntity++;

        }
    }
}
