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
        public Core(Loader loader, Vector2 position) : base(position, new Vector2(4,1f),loader,1)
        {
            Type = EntityType.Core;
            
        }
        override protected void  Act()
        {
        }
    }
}
