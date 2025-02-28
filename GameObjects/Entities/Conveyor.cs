using GameObjects.Drawing;
using GameObjects.GameLogic;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GameObjects.Entities
{
    public class Conveyor : Building, IRotatable, IPassable
    {
        private const int PADDING = 30;
        private int _angle = 0;
        private int _nextState = 0;
        int _maxProgress = 50;
        public List<ResourceTile> Resources { get; set; } = new List<ResourceTile>();
        public bool CanProgress(int i)
        {
            if(i==0)
            {
                if (NextEntities[0] is Conveyor con && con.Resources.Count > 0)
                {
                    // Вычисляем значение counting
                    var counting = con.Resources.Last().Progress + 50 - Resources[0].Progress;

                    // Проверяем условие
                    if (counting > PADDING)
                    {
                        return true;
                    }
                    else
                        return false;
                }
                return true;
            }
            else
            {
                return (Resources[i - 1].Progress - PADDING >= Resources[i].Progress);
            }
        }
        protected override void Act()
        {
            if (Resources.Count>0&& Resources[0].Progress >= 100)
            {
                Pass(Resources[0],_nextState);
            }
            Vector2 vec = GetDirection();
            for(int i=0;i<Resources.Count; i++)
            {
                var res = Resources[i];
                if (res.Progress < _maxProgress&&CanProgress(i))
                    res.AddProgress(1);

                res.Mesh.MoveTo(Position.X - vec.X + vec.X * res.Progress / 50f, 0.55f, Position.Y - vec.Y + vec.Y * res.Progress / 50f);
            }
        }
        public override void Build()
        {
            base.Build();
            BindNextEntities(_map);
        }
        public List<Building> NextEntities {  get; set; }
        public Conveyor(Vector2 position,TextureHolder textureHolder,int rot) :base(position,new Vector2(1,0.5f),500,textureHolder)
        {
            Cost = new Inventory(new CopperOre(2));
            SetAngle(rot);
            NextEntities = new List<Building>();
            Type=EntityType.Conveyor;
            //foreach (var res in Resources)
            //    res.Mesh.YawBy((float)Math.PI);
            Initialize();
        }
        public void Pass(ResourceTile resource,int progress)
        {
            if(NextEntities.Count>0 && NextEntities[0] is Conveyor conv)
            {
                conv.Resources.Add(resource);
                Resources.Remove(resource);
                resource.Progress = progress;
            }
        }
        public void SetAngle(int angle)
        {
            _angle = angle-1;
            Rotate();
        }
        public void SetMaxProgress(int progress)
        {
            _nextState=progress;
        }
        public int GetAngle()
        {
            return _angle;
        }
        public void Rotate()
        {
            _angle += 1;
            if (_angle >= 4 || _angle<0)
                _angle = 0;
            float angleaspect = _angle * (float)Math.PI / 2;
            Mesh.YawBy(angleaspect);

            foreach (var res in Resources)
                res.Mesh.YawBy(angleaspect);
        }
        public void BindNextEntities(Building[,] entities)
        {
            SetNext(entities);
            
            List<Entity> orthogonalConvs = new List<Entity>();
            if ((int)Position.Y > 0)
                orthogonalConvs.Add(entities[(int)Position.Y - 1, (int)Position.X]);

            // Проверка нижнего элемента
            if ((int)Position.Y < entities.GetLength(0) - 1)
                orthogonalConvs.Add(entities[(int)Position.Y + 1, (int)Position.X]);

            // Проверка левого элемента
            if ((int)Position.X > 0)
                orthogonalConvs.Add(entities[(int)Position.Y, (int)Position.X - 1]);

            // Проверка правого элемента
            if ((int)Position.X < entities.GetLength(1) - 1)
                orthogonalConvs.Add(entities[(int)Position.Y, (int)Position.X + 1]);

            foreach (var conv in orthogonalConvs.OfType<Building>())
                conv.SetNext(entities);

        }
        internal override void SetNext(Building[,]entities)
        {
            Vector2 vec = GetDirection();
            NextEntities.Clear();
            _maxProgress = 50;
            if((int)Position.Y + (int)vec.Y<entities.GetLength(0)&&
                (int)Position.X + (int)vec.X < entities.GetLength(1)&&
                (int)Position.Y + (int)vec.Y >=0 &&
                (int)Position.X + (int)vec.X >=0)
            if (entities[(int)Position.Y + (int)vec.Y, (int)Position.X + (int)vec.X] is Conveyor con)
            {
                NextEntities.Clear();
                NextEntities.Add(con);
                Vector2 vec2 = con.GetDirection();
                _nextState = 50;
                if (vec2 == vec)
                {
                    _maxProgress = 100;
                }
                else if (vec2.X != -vec.X && vec2.Y != -vec.Y)
                    _maxProgress = 100;

            }
        }
        public Vector2 GetDirection()
        {
            switch (_angle)
            {
                case 0:
                    return new Vector2(-1, 0);
                case 1:
                    return new Vector2(0, 1);
                case 2:
                    return new Vector2(1, 0);
                case 3:
                    return new Vector2(0, -1);
                default:
                    return Vector2.Zero;
            }
        }

    }
}
