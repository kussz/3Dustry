using GameObjects.Drawing;
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
        private bool _isDrilling = false;
        private Vector2 _drillPosition;
        private float _drillProgress = 0;
        private GameResource _drillingRes;
        public Camera Camera { get; set; }
        public Inventory Inventory { get; set; }
        public Vector4 Position { get=>Camera.Position; set=>Camera.Position=value; }
        private static Player _instance;
        private Player()
        {
            Camera = new Camera(new Vector4(0, 5.0f, 0, 1.0f));
            Inventory = new Inventory(new CopperOre(140),new LeadOre(130));
        }
        public void ChangeDrilling(Tile tile,Vector2 posit)
        {
            if (tile != Tile.None)
            {
                _drillPosition = posit;
                _drillProgress = 0;
                _drillingRes = ResourceFactory.CreateResource(tile, 1);
                _isDrilling = true;
            }
            else
            {
                _isDrilling = false;
            }
        }
        public void Drill(float DeltaT)
        {
            if(Vector2.Distance(new Vector2(Position.X, Position.Z), _drillPosition) > 20f)
            {
                ChangeDrilling(Tile.None,Vector2.Zero);
            }
            else if(_isDrilling)
            {
                _drillProgress += DeltaT;
                if(_drillProgress>1)
                {
                    Inventory.Add(_drillingRes);
                    _drillProgress %= 1;
                }
            }
        }
        public static Player GetInstance()
        {
            if(_instance == null)
                _instance = new Player();
            return _instance;
        }
    }
}
