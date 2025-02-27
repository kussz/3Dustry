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
    public class Core : Building
    {
        public Core(Vector2 position,TextureHolder textureHolder) : base(position, new Vector2(4,1f),1, textureHolder)
        {
            Type = EntityType.Core;
            Cost = new Inventory(new Copper(100),new Lead(100));
            Initialize();

        }
        override protected void  Act()
        {
        }
    }
}
