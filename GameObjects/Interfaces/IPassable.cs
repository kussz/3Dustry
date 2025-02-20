using GameObjects.Entities;
using GameObjects.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Interfaces
{
    public interface IPassable
    {
        public List<Building> NextEntities { get; set; }
        public void Pass(ResourceTile resource, int progress);
        public void BindNextEntities(Building[,] entities);
    }
}
