using GameObjects.GameLogic;
using GameObjects.Resources;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Entities
{
    public class Player
    {
        public Camera Camera { get; set; }
        public Inventory Inventory { get; set; }
        public Vector4 Position { get=>Camera.Position; set=>Camera.Position=value; }
        private static Player _instance;
        private Player()
        {
            Camera = new Camera(new Vector4(0, 5.0f, 0, 1.0f));
            Inventory = new Inventory(new CopperOre(140),new LeadOre(130));
        }
        public static Player GetInstance()
        {
            if(_instance == null)
                _instance = new Player();
            return _instance;
        }
    }
}
