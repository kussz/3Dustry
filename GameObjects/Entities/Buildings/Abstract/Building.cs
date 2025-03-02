using GameObjects.Drawing;
using GameObjects.Entities.Buildings.Concrete;
using GameObjects.GameLogic;
using GameObjects.Interfaces;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects.Entities.Buildings.Abstract
{
    public abstract class Building : Entity
    {
        protected Building[,] _map;
        public Building(Vector2 position, Vector2 size, float speed, TextureHolder textureHolder) : base(position)
        {
            Metadata = new TextureMetaData();
            Speed = speed;
            IsBuilt = false;
            Size = size;
            Mesh = _loader.MakeCube(new Vector4(Position.X, 0, Position.Y, 1), size, 0, 0, 0);
            TextureHolder = textureHolder;
        }
<<<<<<< HEAD:GameObjects/Entities/Buildings/Abstract/Building.cs

        public int State { get; set; }
=======
        
        public TextureMetaData Metadata { get; set; }
>>>>>>> 478e56b3c91a0692ab06e996ec5a31f79e259c01:GameObjects/Entities/Building.cs
        private float _maxProgress;
        protected void Initialize()
        {
            _maxProgress = 30 * (float)Math.Sqrt(Cost.GetItemsCount());
        }
<<<<<<< HEAD:GameObjects/Entities/Buildings/Abstract/Building.cs
        public float BuildProgress { get { return _buildProgress / _maxProgress * Size.Y; } }
        public float _buildProgress = 0;
=======
        public float BuildProgress { get { return (float)_buildProgress / _maxProgress * Size.Y; } }
        public float _buildProgress=0;
>>>>>>> 478e56b3c91a0692ab06e996ec5a31f79e259c01:GameObjects/Entities/Building.cs
        private bool _activated = false;
        public Vector2 Size { get; set; }
        public bool IsBuilt { get; private set; }
        public Inventory? Cost { get; protected set; }
        public EntityType Type { get; set; }
        public void Activate(Building[,] map)
        {
            _map = map;
            _activated = true;
            _buildProgress = 0;
        }
        public virtual void Build()
        {
<<<<<<< HEAD:GameObjects/Entities/Buildings/Abstract/Building.cs
=======
            foreach (var b in GetAligned(_map))
                b.SetNext(_map);
            Position = new Vector2(Mesh.Position.X, Mesh.Position.Z);
>>>>>>> 478e56b3c91a0692ab06e996ec5a31f79e259c01:GameObjects/Entities/Building.cs
            IsBuilt = true;
            foreach (var b in GetAligned(_map))
                b.SetNext(_map);
            Position = new Vector2(Mesh.Position.X, Mesh.Position.Z);
            //_buildProgress = Int32.MaxValue;
        }
        private List<Building> GetAligned(Building[,] entities)
        {
            List<Building> list = new List<Building>();
            for (int i = (int)Position.Y - (int)Size.X / 2; i < (int)Position.Y + Size.X / 2; i++)
            {
                //Size.X / 2 % 1
                Building e1 = entities[i, (int)(Position.X - Size.X / 2 - 1)];
                Building e2 = entities[i, (int)(Position.X + Size.X / 2)];
                if (!list.Contains(e1))
                    list.Add(e1);
                if (!list.Contains(e2))
                    list.Add(e2);
            }
            for (int i = (int)Position.X - (int)Size.X / 2; i < (int)Position.X + Size.X / 2; i++)
            {
                Building e1 = entities[(int)(Position.Y - Size.X / 2 - 1), i];
                Building e2 = entities[(int)(Position.Y + Size.X / 2), i];
                if (!list.Contains(e1))
                    list.Add(e1);
                if (!list.Contains(e2))
                    list.Add(e2);
            }
<<<<<<< HEAD:GameObjects/Entities/Buildings/Abstract/Building.cs
            return list.Where(b => b != null).ToList();
=======
            return list.Where(b=>b!=null).ToList();
>>>>>>> 478e56b3c91a0692ab06e996ec5a31f79e259c01:GameObjects/Entities/Building.cs
        }
        internal virtual void SetNext(Building[,] entities)
        {
            var list = GetAligned(entities);
            var newlist = new List<Building>();
<<<<<<< HEAD:GameObjects/Entities/Buildings/Abstract/Building.cs
            foreach (var b in list)
            {
                if (b is Conveyor con)
                {
                    if (con.GetDirection().X == Math.Sign(Position.X - con.Position.X) && Math.Abs(Position.X - con.Position.X) > Size.X / 2 ||
                        con.GetDirection().Y == Math.Sign(Position.Y - con.Position.Y) && Math.Abs(Position.Y - con.Position.Y) > Size.X / 2)
                        continue;

=======
            foreach(var b in list)
            {
                if(b is Conveyor con)
                {
                    if ((con.GetDirection().X == Math.Sign(Position.X - con.Position.X)&& Math.Abs(Position.X - con.Position.X)>Size.X/2) ||
                        (con.GetDirection().Y == Math.Sign(Position.Y - con.Position.Y) && Math.Abs(Position.Y - con.Position.Y) > Size.X / 2))
                        continue;
                   
>>>>>>> 478e56b3c91a0692ab06e996ec5a31f79e259c01:GameObjects/Entities/Building.cs
                }
                newlist.Add(b);
            }
            if (this is IPassable passable)
                passable.NextEntities = newlist.Where(e => e != null).ToList();
        }
        public async void Produce(float deltaT)
        {
            if (_activated)
            {
                if (IsBuilt)
                {
                    Cooldown -= deltaT * Speed * 30f;
                    TickWork();
                    if (Cooldown <= 0)
                    {
                        Cooldown = 100;
                        IntervalWork();
                    }
                }
                else
                {
<<<<<<< HEAD:GameObjects/Entities/Buildings/Abstract/Building.cs
                    _buildProgress += 50f * deltaT;
                    if (_buildProgress >= _maxProgress)
=======
                    _buildProgress += 50f *deltaT;
                    if(_buildProgress>=_maxProgress)
>>>>>>> 478e56b3c91a0692ab06e996ec5a31f79e259c01:GameObjects/Entities/Building.cs
                    {
                        Build();
                    }
                }
<<<<<<< HEAD:GameObjects/Entities/Buildings/Abstract/Building.cs

=======
                if(this is IConvertor convertor && convertor.IsWorking)
                {
                    convertor.ConvertionProgress++;
                    if(convertor.ConvertionProgress>=100)
                    {
                        convertor.EndWork();
                    }
                }
>>>>>>> 478e56b3c91a0692ab06e996ec5a31f79e259c01:GameObjects/Entities/Building.cs
            }
        }
        protected abstract void IntervalWork();
        protected abstract void TickWork();
        public Vector3 IntersectWithLook(Camera camera, float distance)
        {
            Vector3 lookDirection = new Vector3(
            (float)(Math.Cos(-camera.Pitch) * Math.Sin(camera.Yaw)),
            (float)Math.Sin(-camera.Pitch),
            (float)(Math.Cos(-camera.Pitch) * Math.Cos(camera.Yaw))
            );
            lookDirection.Normalize();

            Vector3 intersectionPoint = IntersectRay((Vector3)camera.Position, lookDirection);
            if (Vector3.Distance((Vector3)camera.Position, intersectionPoint) > distance)
                return Vector3.Zero;
            return intersectionPoint;
        }

        private Vector3 IntersectRay(Vector3 origin, Vector3 direction)
        {

            // Установите значения для tMin и tMax
            float tMin = (Position.X - Size.X / 2 - origin.X) / direction.X;
            float tMax = (Position.X + Size.X / 2 - origin.X) / direction.X;

            // Обмен значениями, если необходимо
            if (tMin > tMax)
            {
                float temp = tMin;
                tMin = tMax;
                tMax = temp;
            }

            // Проверка по оси Y
            float tyMin = -origin.Y / direction.Y;
            float tyMax = (Size.Y - origin.Y) / direction.Y;

            if (tyMin > tyMax)
            {
                float temp = tyMin;
                tyMin = tyMax;
                tyMax = temp;
            }

            // Обновление tMin и tMax
            if (tMin > tyMax || tyMin > tMax)
            {
                return Vector3.Zero; // Нет пересечения
            }

            if (tyMin > tMin)
            {
                tMin = tyMin;
            }

            if (tyMax < tMax)
            {
                tMax = tyMax;
            }

            // Проверка по оси Z
            float tzMin = (Position.Y - Size.X / 2 - origin.Z) / direction.Z;
            float tzMax = (Position.Y + Size.X / 2 - origin.Z) / direction.Z;

            if (tzMin > tzMax)
            {
                float temp = tzMin;
                tzMin = tzMax;
                tzMax = temp;
            }

            // Обновление tMin и tMax
            if (tMin > tzMax || tzMin > tMax)
            {
                return Vector3.Zero; // Нет пересечения
            }

            if (tzMin > tMin)
            {
                tMin = tzMin;
            }

            if (tzMax < tMax)
            {
                tMax = tzMax;
            }

            // Инициализация переменной для хранения точки пересечения
            Vector3 intersectionPoint;
            // Если tMin больше нуля, то это точка пересечения
            if (tMin >= 0)
            {
                intersectionPoint = origin + tMin * direction;
                return intersectionPoint;
            }

            // Если tMax больше нуля, то это точка пересечения
            if (tMax >= 0)
            {
                intersectionPoint = origin + tMax * direction;
                return intersectionPoint;
            }

            return Vector3.Zero; // Нет положительных пересечений
        }

        public override void Dispose()
        {
            foreach (var b in GetAligned(_map))
                b.SetNext(_map);
<<<<<<< HEAD:GameObjects/Entities/Buildings/Abstract/Building.cs
            Mesh.Dispose();
=======
>>>>>>> 478e56b3c91a0692ab06e996ec5a31f79e259c01:GameObjects/Entities/Building.cs
            base.Dispose();
        }

    }
}
