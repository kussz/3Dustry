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
    public class Miner : Entity
    {
        private GameResource _resource;
        public Miner(Loader loader, Vector2 position, GameResource resource,TextureHolder textureHolder):base(position,new Vector2(2,3),loader,1,textureHolder)
        {
            Type=EntityType.Miner;
            _resource = resource;

        }
        protected override void Act()
        {
            Inventory.Add(_resource);
            Mesh.Position = new Vector4(Mesh.Position.X, Inventory.Get("Copper"), Mesh.Position.Z, 1);
        }
    }
}
