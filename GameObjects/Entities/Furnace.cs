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

namespace GameObjects.Entities
{
    public class Furnace : Building, IAcceptor, IPassable, IConvertor
    {
        public Inventory Output { get; set; }
        public KeyValuePair<Inventory, Inventory>? CurrentRecipe { get; set; }
        public float ConvertionProgress { get; set; }

        public ResourceType[] Acceptings { get; }
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
            Type = EntityType.Furnace;
            Metadata.State = 1;
            Cost = new Inventory(new CopperOre(10), new LeadOre(30));
            Output = new Inventory();
            Inventory.MaxItems = 10;
            Output.MaxItems = 10;
            Recipes = new Dictionary<Inventory, Inventory>()
            {
                { new Inventory(new CopperOre(1)),new Inventory(new LeadOre(1)) }
            };
            Initialize();

        }
        public void EndWork()
        {
            SetWorking(false);
            ConvertionProgress = 0;
            Output += CurrentRecipe.Value.Value;
        }
        public void SetWorking(bool isWorking)
        {
            IsWorking=isWorking;
            Metadata.State = Convert.ToInt32(!isWorking);
        }
        protected override void Act()
        {
            if (!IsWorking)
            {
                CurrentRecipe = (this as IConvertor).CheckAvailable();
                if(CurrentRecipe!=null)
                {
                    SetWorking(true);
                    Inventory.Subtract(CurrentRecipe.Value.Key);
                }
            }
            var f = Output.GetFirst();
            if(f!=null)
            {
                var newer = ResourceFactory.CreateResource(f.Type, 1);
                Output.Subtract(newer);
                ResourceTile tile = new ResourceTile(newer);
                if(NextEntities.Count>0)
                    Pass(tile);
            }
                
        }
        public void Pass(ResourceTile tile, int progress = 25)
        {
            if (_nextEntity >= NextEntities.Count)
            {
                _nextEntity %= NextEntities.Count;
            }
            if (NextEntities[_nextEntity] is Conveyor con)
            {
                tile.Progress = progress;
                con.Resources.Add(tile);
                Inventory.Subtract(tile.LogicResource);
            }
            _nextEntity++;

        }

    }
}
