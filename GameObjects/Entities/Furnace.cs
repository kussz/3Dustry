using GameObjects.Drawing;
using GameObjects.GameLogic;
using GameObjects.Resources;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Entities
{
    public class Furnace : Building
    {
        public Furnace(Vector2 position, TextureHolder textureHolder) : base(position, new Vector2(2, 2f), 1, textureHolder)
        {
            Type = EntityType.Furnace;
            Cost = new Inventory(new Copper(10), new Lead(30));
            Initialize();

        }
        protected override void Act()
        {
            
        }
    }
}
