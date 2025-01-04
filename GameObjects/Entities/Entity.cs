using GameObjects.Drawing;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Entities
{
    public abstract class Entity
    {
        public MeshObject Mesh { get; set; }
        public Vector2 Position { get { return Position; } set { Position = value; Mesh.Position = new Vector4( Position.X,0,Position.Y,1); } }
        public EntityType Type { get; set; }
        public Vector2 Size { get; set; }
    }
}
