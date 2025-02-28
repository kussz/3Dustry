using GameObjects.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Resources
{
    public class CopperOre : GameResource
    {
        public CopperOre(int quantity) : base(ResourceType.CopperOre,quantity)
        { }
    }
}
