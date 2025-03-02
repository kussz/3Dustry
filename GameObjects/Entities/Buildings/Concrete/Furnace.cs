using GameObjects.Drawing;
using GameObjects.Entities.Buildings.Abstract;
using GameObjects.GameLogic;
using GameObjects.Resources;
using SharpDX;

namespace GameObjects.Entities.Buildings.Concrete
{
    public class Furnace : Crafter
    {
        public Furnace(Vector2 position, TextureHolder textureHolder) : base(position, new Vector2(2, 2f), 1, textureHolder)
        {

            Type = EntityType.Furnace;
            Cost = new Inventory(new CopperOre(10), new LeadOre(30));
            Inventory.MaxItems = 10;
            Output.MaxItems = 10;
            Recipes = new Dictionary<Inventory, Inventory>()
            {
                { new Inventory(new CopperOre(2),new CoalOre(1)),new Inventory(new Copper(2)) },
                { new Inventory(new LeadOre(2),new CoalOre(1)),new Inventory(new Lead(2)) },

            };
            Initialize();
        }
    }
}
