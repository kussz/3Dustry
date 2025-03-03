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
            Speed = speed;
            IsBuilt = false;
            Size = size;
            Mesh = _loader.MakeCube(new Vector4(Position.X, 0, Position.Y, 1), size, 0, 0, 0);
            TextureHolder = textureHolder;
        }

        public int State { get; set; }
        private float _maxProgress;
        protected void Initialize()
        {
            _maxProgress = 30 * (float)Math.Sqrt(Cost.GetItemsCount());
        }
        public float BuildProgress { get { return _buildProgress / _maxProgress * Size.Y; } }
        public float _buildProgress = 0;
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
            return list.Where(b => b != null).ToList();
        }
        internal virtual void SetNext(Building[,] entities)
        {
            var list = GetAligned(entities);
            var newlist = new List<Building>();
            foreach (var b in list)
            {
                if (b is Conveyor con)
                {
                    if (con.GetDirection().X == Math.Sign(Position.X - con.Position.X) && Math.Abs(Position.X - con.Position.X) > Size.X / 2 ||
                        con.GetDirection().Y == Math.Sign(Position.Y - con.Position.Y) && Math.Abs(Position.Y - con.Position.Y) > Size.X / 2)
                        continue;
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
                    _buildProgress += 50f * deltaT;
                    if (_buildProgress >= _maxProgress)
                    {
                        Build();
                    }
                }
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
            Mesh.Dispose();
            base.Dispose();
        }

    }
}
