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
    public abstract class Entity : IDisposable
    {
        protected static Loader _loader;
        public static void Configure(Loader loader)
        {
            _loader = loader;
        }
        public float Cooldown { get; set; } = 100f;
        public float Speed { get; set; }
        public TextureHolder TextureHolder { get; set; }
        public Inventory Inventory { get; set; }
        public Entity(Vector2 position)
        {
            Inventory = new Inventory();
            Position = position;
            
        }
        
        public MeshObject Mesh { get; set; }
        public Vector2 Position { get; set; }
        public virtual void Dispose() { Mesh.Dispose(); }
        
    }
}
