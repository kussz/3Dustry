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
                    return new Copper(quantity);
                case Tile.Lead: return new Lead(quantity);
                default:
                    return null;
            }
        }
    }
}
