using GameObjects.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Resources
{
    public class LeadOre : GameResource
    {
        public LeadOre(int quantity) : base(ResourceType.LeadOre, quantity) { }
    }
}
