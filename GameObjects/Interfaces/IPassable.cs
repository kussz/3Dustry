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
        public List<Entity> NextEntities {  get; }
        public void Pass(ResourceTile resource, int progress);
        public void SetNext(Entity[,] entities);
        public void BindNextEntities(Entity[,] entities);
    }
}
