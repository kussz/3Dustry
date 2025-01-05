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
        public Core(Loader loader, Vector2 position) : base(position, new Vector2(2,1),loader)
        {
            Type = EntityType.Core;
            
        }
    }
}
