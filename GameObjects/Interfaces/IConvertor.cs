using GameObjects.Drawing;
using GameObjects.Entities.Buildings.Abstract;
using GameObjects.GameLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Interfaces
{
    public interface IConvertor
    {
        public bool IsWorking { get; set; }
        public KeyValuePair<Inventory,Inventory>? CurrentRecipe { get; set; }
        public float ConvertionProgress { get; set; }
        Dictionary<Inventory, Inventory> Recipes { get; set; }
        Inventory Output { get; set; }
        public void EndWork();
        public void Craft(KeyValuePair<Inventory,Inventory>? crafting)
        {
            if(crafting!=null)
            {
                (this as Building).Inventory -= crafting.Value.Key;
                Output += crafting.Value.Value;
            }
        }
        public KeyValuePair<Inventory,Inventory>? CheckAvailable()
        {
            foreach (var recipe in Recipes)
            {
                if ((this as Building).Inventory >= recipe.Key)
                {
                    return recipe;
                }
            }
            return null;
        }
    }
}
