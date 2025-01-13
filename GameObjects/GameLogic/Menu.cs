using GameObjects.Drawing;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.GameLogic
{
    public class Menu
    {
        public MenuTile Hotbar;
        public MenuTile SelectedCell;
        private float _hotbarSize = 0.6f;
        private float _stepsize = 10 / 73f;
        private float _wallWidth = 0.1f;
        public Menu()
        {
            Hotbar = new MenuTile("Assets\\Menu\\Inventory.png", _hotbarSize, new Vector2(0, -0.9f));
            SelectedCell = new MenuTile("Assets\\Menu\\Selected.png", _hotbarSize*_stepsize, new Vector2(_stepsize*_hotbarSize*(1-_wallWidth), -0.9f));
            SetSelectedCell(1);
        }
        public void SetSelectedCell(int index)
        {
            float x = -_hotbarSize+ _stepsize * _hotbarSize;
            x += 2*(index-1) * _hotbarSize * _stepsize*(1-_wallWidth);
            SelectedCell.Mesh.Position = new Vector4(x, SelectedCell.Mesh.Position.Y, SelectedCell.Mesh.Position.Z, 1);
        }
    }
}
