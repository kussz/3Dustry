using GameObjects.Drawing;
using GameObjects.Interfaces;
using GameObjects.Resources;
using SharpDX;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Entities
{
    public class Conveyor : Entity, IRotatable
    {
        private float _angle = 0;
        public List<ResourceTile> Resources = new List<ResourceTile>();
        protected override void Act()
        {
            if (Resources.Count>0&& Resources[0].Progress >= 100)
            {
                Resources.RemoveAt(0);
            }
            foreach(var res in Resources)
            {
                res.AddProgress(1);
                
                res.Mesh.MoveTo(Position.X + 1 - res.Progress/100f, 0.55f, Position.Y+0.5f);
            }
        }
        public Conveyor(Vector2 position,TextureHolder textureHolder) :base(position,new Vector2(1,0.5f),500,textureHolder)
        {
            Type=EntityType.Conveyor;
            Resources.Add(new ResourceTile(new Copper(1), new Vector4(position.X,0,position.Y,1)));
            //foreach (var res in Resources)
            //    res.Mesh.YawBy((float)Math.PI);
        }
        public void Rotate(float angle)
        {
            _angle += angle;
            if (_angle >= Math.PI * 2)
                _angle = 0;

            Mesh.YawBy(_angle);

            foreach (var res in Resources)
                res.Mesh.YawBy(_angle);
        }
    }
}
