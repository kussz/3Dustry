using GameObjects.Drawing;
using GameObjects.Interfaces;
using GameObjects.Resources;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Entities
{
    public class Miner : Building, IPassable
    {
        public List<Building> NextEntities {  get; set; }
        private int _nextEntity = 0;
        private GameResource _resource;
        public Miner(Vector2 position, GameResource resource,TextureHolder textureHolder):base(position,new Vector2(2,3),resource==null?0:resource.Quantity,textureHolder)
        {
            Cost = new GameLogic.Inventory(new Copper(20));
            Type=EntityType.Miner;
            _resource = resource;
            NextEntities = new List<Building>();
            Initialize();
        }
        protected override void Act()
        {
            Inventory.Add(ResourceFactory.CreateResource(_resource.Type,1));
            Player.GetInstance().Inventory.Add(ResourceFactory.CreateResource(_resource.Type, 1));
            if(NextEntities.Count > 0)
            {
                if (_nextEntity >= NextEntities.Count)
                {
                    _nextEntity %= NextEntities.Count;
                }
                var pos = NextEntities[_nextEntity].Position;
                Pass(new ResourceTile(Inventory.GetResource(_resource.Type)));
            }
        }
        public void BindNextEntities(Building[,] entities)
        {
            SetNext(entities);
            foreach(var entity in NextEntities) 
            {
                entity.SetNext(entities);
            }
        }
        
        public void Pass(ResourceTile tile,int progress = 50)
        {
            if (NextEntities[_nextEntity] is Conveyor con)
            {
                con.Resources.Add(tile);
                Inventory.Subtract(ResourceFactory.CreateResource(_resource.Type, 1));
            }
            _nextEntity++;

        }
    }
}
