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
    public class Furnace : Building, IAcceptor
    {
        public Tile[] Acceptings {  get; set; }
        public void Get(GameResource resource)
        {
            Inventory.Add(resource);
        }
        public void Get(Inventory inv)
        {
            Inventory += inv;
        }
        public bool IsWorking {  get; set; }
        public Furnace(Vector2 position, TextureHolder textureHolder) : base(position, new Vector2(2, 2f), 1, textureHolder)
        {
            Type = EntityType.Furnace;
            Cost = new Inventory(new CopperOre(10), new LeadOre(30));
            SetWorking(false);
            Initialize();

        }
        private void SetWorking(bool isWorking)
        {
            IsWorking=isWorking;
            State = Convert.ToInt32(!isWorking);
        }
        protected override void Act()
        {
            SetWorking(!IsWorking);
        }
    }
}
