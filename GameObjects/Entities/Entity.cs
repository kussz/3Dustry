using GameObjects.Drawing;
using GameObjects.GameLogic;
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
        public float Cooldown { get; set; } = 100f;
        public float Speed { get; set; }
        public Inventory Inventory { get; }
        public Entity(Vector2 position, Vector2 size, Loader loader,float speed)
        {
            Inventory = new Inventory();
            Speed = speed;
            IsBuilt = false;
            Position = position;
            Size = size;
            Mesh = loader.MakeCube(new SharpDX.Vector4(Position.X, 0, Position.Y, 1), size, 0, 0, 0);
        }
        public MeshObject Mesh { get; set; }
        public Vector2 Position { get; set; }
        public EntityType Type { get; set; }
        public Vector2 Size { get; set; }
        public bool IsBuilt { get; set; }
        public void Dispose() { Mesh.Dispose(); }
        public void Produce(float deltaT)
        {
            Cooldown -= deltaT * Speed*30f;
            if(Cooldown<=0)
            {
                Cooldown=100;
                Act();
            }
        }
        protected abstract void Act();
        public Vector3 IntersectWithLook(Camera camera,float distance)
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
            float tMin = (Position.X - origin.X) / direction.X;
            float tMax = (Position.X + Size.X - origin.X) / direction.X;

            // Обмен значениями, если необходимо
            if (tMin > tMax)
            {
                float temp = tMin;
                tMin = tMax;
                tMax = temp;
            }

            // Проверка по оси Y
            float tyMin = (- origin.Y) / direction.Y;
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
            float tzMin = (Position.Y - origin.Z) / direction.Z;
            float tzMax = (Position.Y + Size.X - origin.Z) / direction.Z;

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
    }
}
