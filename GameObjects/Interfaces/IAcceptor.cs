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
<<<<<<< HEAD
=======
        ResourceType[] Acceptings { get; }
>>>>>>> 478e56b3c91a0692ab06e996ec5a31f79e259c01
        bool Get(GameResource resource);
        bool Get(Inventory inventory);
    }
}
