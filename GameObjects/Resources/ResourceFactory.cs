using GameObjects.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Resources
{
    public class ResourceFactory
    {
        public static GameResource CreateResource(Tile resType,int quantity)
        {
            switch (resType)
            {
                case Tile.Copper:
                    return new CopperOre(quantity);
                case Tile.Lead: return new LeadOre(quantity);
                default:
                    return null;
            }
        }
        public static GameResource CreateResource(ResourceType resType, int quantity)
        {
            switch (resType)
            {
                case ResourceType.CopperOre:
                    return new CopperOre(quantity);
                case ResourceType.LeadOre: return new LeadOre(quantity);
                default:
                    return null;
            }
        }
    }
}
