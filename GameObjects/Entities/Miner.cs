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
    public class Miner : Entity, IPassable
    {
        public List<Entity> NextEntities {  get; private set; }
        private int _nextEntity = 0;
        private GameResource _resource;
        public Miner(Vector2 position, GameResource resource,TextureHolder textureHolder):base(position,new Vector2(2,3),resource.Quantity,textureHolder)
        {
            Type=EntityType.Miner;
            _resource = resource;
            NextEntities = new List<Entity>();
        }
        protected override void Act()
        {
            Inventory.Add(ResourceFactory.CreateResource(_resource.Type,1));
            if(NextEntities.Count > 0)
            {
                if (_nextEntity >= NextEntities.Count)
                {
                    _nextEntity %= NextEntities.Count;
                }
                var pos = NextEntities[_nextEntity].Position;
                Pass(new ResourceTile(Inventory.GetResource(_resource.Type)));
                Mesh.Position = new Vector4(Mesh.Position.X, Inventory.Get(Tile.Copper), Mesh.Position.Z, 1);
            }
        }
        public void BindNextEntities(Entity[,] entities)
        {
            SetNext(entities);
            foreach(var entity in NextEntities.OfType<IPassable>()) 
            {
                entity.SetNext(entities);
            }
        }
        public void SetNext(Entity[,] entities)
        {
            List<Entity> list = new List<Entity>();
            for (int i = (int)Position.Y - (int)Size.X / 2; i < (int)Position.Y + Size.X / 2; i++)
            {
                Entity e1 = entities[i, (int)Position.X - (int)Size.X / 2 - 1];
                Entity e2 = entities[i, (int)Position.X + (int)Size.X / 2 + 1];
                if (!list.Contains(e1))
                    list.Add(e1);
                if (!list.Contains(e2))
                    list.Add(e2);
            }
            for (int i = (int)Position.X - (int)Size.X / 2; i < (int)Position.X + Size.X / 2; i++)
            {
                Entity e1 = entities[(int)Position.Y - (int)Size.X / 2 - 1, i];
                Entity e2 = entities[(int)Position.Y + (int)Size.X / 2 + 1, i];
                if (!list.Contains(e1))
                    list.Add(e1);
                if (!list.Contains(e2))
                    list.Add(e2);
            }
            NextEntities = list.Where(e => e != null).ToList();
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
