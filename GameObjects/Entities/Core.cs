using GameObjects.Drawing;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Entities
{
    public class Core : Entity
    {
        public Core(Vector2 position,TextureHolder textureHolder) : base(position, new Vector2(4,1f),1, textureHolder)
        {
            Type = EntityType.Core;
            
        }
        override protected void  Act()
        {
        }
    }
}
