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
        Tile[] Acceptings { get; }
        void Get(GameResource resource);
        void Get(Inventory inventory);
    }
}
