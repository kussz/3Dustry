using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Interfaces
{
    public interface IRotatable
    {
        public void Rotate();
        public void SetAngle(int angle);
        public int GetAngle();
        public Vector2 GetDirection();
    }
}
