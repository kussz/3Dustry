using GameObjects.Entities;
using GameObjects.Entities.Buildings.Abstract;
using GameObjects.Entities.Buildings.Concrete;
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
        public bool CanProgress()
        {
            if (NextEntities[0] is Conveyor con && con.Resources.Count > 0)
            {
                // Вычисляем значение counting
                var counting = con.Resources.Last().Progress - 50;

                // Проверяем условие
                if (counting > Conveyor.PADDING)
                {
                    return true;
                }
                else
                    return false;
            }
            return true;

        }
        public List<Building> NextEntities { get; set; }
        public void Pass(ResourceTile resource, int progress);
        public virtual void BindNextEntities(Building[,] entities)
        {
            (this as Building).SetNext(entities);
            foreach (var entity in NextEntities)
            {
                entity.SetNext(entities);
            }
        }
    }
}
