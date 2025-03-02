using GameObjects.Drawing;
using GameObjects.GameLogic;
using GameObjects.Interfaces;
using GameObjects.Resources;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameObjects.Entities
{
    public class Furnace : Building, IAcceptor, IPassable, IConvertor
    {
        public Inventory Output { get; set; }
        public KeyValuePair<Inventory, Inventory>? CurrentRecipe { get; set; }
        public float ConvertionProgress { get; set; }

        private int _nextEntity = 0;

        public Dictionary<Inventory,Inventory> Recipes {  get; set; }
        public List<Building> NextEntities {  get; set; }
        public bool Get(GameResource resource)
        {
            return(Inventory.Add(resource));
        }
        public bool Get(Inventory inv)
        {
            Inventory += inv;
            return true;
        }
        public bool IsWorking { get; set; } = false;
        public Furnace(Vector2 position, TextureHolder textureHolder) : base(position, new Vector2(2, 2f), 1, textureHolder)
        {
            NextEntities = new List<Building>();
            Type = EntityType.Furnace;
            State = 1;
            Cost = new Inventory(new CopperOre(10), new LeadOre(30));
            Output = new Inventory();
            Inventory.MaxItems = 10;
            Output.MaxItems = 10;
            Recipes = new Dictionary<Inventory, Inventory>()
            {
                { new Inventory(new CopperOre(2),new CoalOre(1)),new Inventory(new Copper(2)) },
                { new Inventory(new LeadOre(2),new CoalOre(1)),new Inventory(new Lead(2)) },

            };
            Initialize();

        }
        public void EndWork()
        {
            if ((this as IConvertor).CheckAvailable() == null)
                State = 1;
            IsWorking = false;
            ConvertionProgress = 0;
            Output += CurrentRecipe.Value.Value;
        }
        protected override void TickWork()
        {
            if (!IsWorking)
            {
                CurrentRecipe = (this as IConvertor).CheckAvailable();
                if (CurrentRecipe != null)
                {
                    IsWorking=true;
                    State = 0;
                    Inventory.Subtract(CurrentRecipe.Value.Key);
                }
            }
            else if (IsWorking)
            {
                ConvertionProgress += Speed;
                if (ConvertionProgress >= 100)
                {
                    EndWork();
                }
            }
            var f = Output.GetFirst();
            if (f != null && f.Quantity > 0)
            {
                var newer = ResourceFactory.CreateResource(f.Type, 1);
                ResourceTile tile = new ResourceTile(newer);
                if (NextEntities.Count > 0)
                    Pass(tile);
            }

        }
        protected override void IntervalWork()
        {
            
            
                
        }
        public bool CanProgress()
        {
            
            if (NextEntities.Count > 0 && NextEntities[_nextEntity] is Conveyor con && con.Resources.Count > 0)
            {
                // Вычисляем значение counting
                var counting = con.Resources.Last().Progress;

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
        public void Pass(ResourceTile tile, int progress = 25)
        {
            if (_nextEntity >= NextEntities.Count)
            {
                _nextEntity %= NextEntities.Count;
            }
            if (NextEntities[_nextEntity] is Conveyor con)
            {
                if(CanProgress())
                {
                    tile.Progress = progress;
                    con.Resources.Add(tile);
                    Output.Subtract(tile.LogicResource);
                }
            }
            _nextEntity++;

        }

    }
}
