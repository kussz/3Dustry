using GameObjects.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Entities
{
    public class Core : Entity
    {
        public Core(Loader loader) : base()
        {
            Type = EntityType.Core;
            Size=new SharpDX.Vector2(1,1);
            Mesh = loader.MakeCube(new SharpDX.Vector4(0, 0, 0, 1), 0, 0, 0);
        }
    }
}
