using GameObjects.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Resources
{
    public class CoalOre : GameResource
    {
        public CoalOre(int quantity) : base(ResourceType.CoalOre, quantity)
        { }
    }
}
