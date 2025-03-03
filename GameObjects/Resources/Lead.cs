using GameObjects.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Resources
{
    public class Lead : GameResource
    {
        public Lead(int quantity) : base(ResourceType.Lead, quantity)
        { }
    }
}
