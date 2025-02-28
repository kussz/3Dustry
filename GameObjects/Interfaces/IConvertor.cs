﻿using GameObjects.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Interfaces
{
    public interface IConvertor
    {
        Dictionary<Tile[], Tile[]> Convert(Tile[] tiles);
    }
}
