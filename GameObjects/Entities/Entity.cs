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
        public Entity(Vector2 position, Vector2 size, Loader loader)
        {
            Position = position;
            Size = size;
            Mesh = loader.MakeCube(new SharpDX.Vector4(Position.X, 0, Position.Y, 1),size, 0, 0, 0);
        }
        public MeshObject Mesh { get; set; }
        public Vector2 Position { get; set; }
        public EntityType Type { get; set; }
        public Vector2 Size { get; set; }
        public bool IsBuilt { get; set; }
    }
}
