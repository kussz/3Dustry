using GameObjects.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Resources
{
    public class Copper : GameResource
    {
        public Copper(int quantity) : base(ResourceType.Copper, quantity)
        { }
    }
}
