using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Drawing
{
    public enum Tile
    {
        None,
        Sand,
        BlackSand,
        Copper,
        Lead,
        Coal
    }
    public enum EntityType
    {
        None,
        Core,
        Miner,
        Conveyor,
        Furnace
    }
    public enum ResourceType
    {
        CopperOre,
        LeadOre,
        CoalOre,
        Copper,
        Lead,

    }
}
