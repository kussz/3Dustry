using GameObjects.Drawing;
using GameObjects.GameLogic;
using GameObjects.Interfaces;
using GameObjects.Resources;
using SharpDX;

namespace GameObjects.Entities
{
    public class Core : Building, IAcceptor
    {
        public Core(Vector2 position, TextureHolder textureHolder) : base(position, new Vector2(4, 1f), 1, textureHolder)
        {
            Type = EntityType.Core;
            Inventory.MaxItems = 2000;
            Cost = new Inventory(new CopperOre(100), new LeadOre(100));
            Initialize();

        }
        public bool Get(GameResource resource)
        {
            if (Inventory.Add(resource))
            {
                Player.GetInstance().Inventory.Add(resource);
                return true;
            }
            return false;
        }
        public bool Get(Inventory inv)
        {
            Inventory += inv;
            Player.GetInstance().Inventory += inv;
            return true;
        }
        protected override void TickWork()
        {

        }
        override protected void IntervalWork()
        {
        }
    }
}
