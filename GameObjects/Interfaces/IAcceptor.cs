using GameObjects.Drawing;
using GameObjects.GameLogic;
using GameObjects.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Interfaces
{
    public interface IAcceptor
    {
        ResourceType[] Acceptings { get; }
        bool Get(GameResource resource);
        bool Get(Inventory inventory);
    }
}
