using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameObjects.Drawing;
using SharpDX;

namespace GameObjects.Entities
{
    public class Camera : Game3DObject
    {
        private float _fovY;
        public float FOVY { get => _fovY; set => _fovY = value; }

        private float _aspect;
        public float Aspect { get => _aspect; set => _aspect = value; }

        public Camera(Vector4 position,
            float yaw = 0.0f, float pitch = 0.0f, float roll = 0.0f,
            float fovY = MathUtil.PiOverTwo, float aspect = 1.0f)
            : base(position, yaw, pitch, roll)
        {
            _fovY = fovY;
            _aspect = aspect;
        }

        public Matrix GetProjectionMatrix()
        {
            return Matrix.PerspectiveFovLH(_fovY, _aspect, 0.1f, 1000.0f);
        }

        public Matrix GetViewMatrix()
        {
            Matrix rotation = Matrix.RotationYawPitchRoll(_yaw, _pitch, _roll);
            Vector3 viewTo = (Vector3)Vector4.Transform(Vector4.UnitZ, rotation);
            Vector3 viewUp = (Vector3)Vector4.Transform(Vector4.UnitY, rotation);
            return Matrix.LookAtLH((Vector3)_position,
                (Vector3)_position + viewTo, viewUp);
        }

        public Vector3 IntersectRayWithPlane(float distance,float planeY)
        {
            Vector3 lookDirection = new Vector3(
            (float)(Math.Cos(-Pitch) * Math.Sin(Yaw)),
            (float)Math.Sin(-Pitch),
            (float)(Math.Cos(-Pitch) * Math.Cos(Yaw))
            );
            lookDirection.Normalize();

            Vector3 intersectionPoint = IntersectWithPlaneY((Vector3)Position, lookDirection, planeY);
            if (Vector3.Distance((Vector3)Position, intersectionPoint) > distance)
                return Vector3.Zero;
            return intersectionPoint;
        }

        static Vector3 IntersectWithPlaneY(Vector3 origin, Vector3 direction, float planeY)
        {
            // Проверяем, чтобы направление не было параллельно плоскости
            if (Math.Abs(direction.Y) < 1e-6)
            {
                return Vector3.Zero; // Нет пересечения
            }

            // Вычисляем параметр t
            float t = (planeY - origin.Y) / direction.Y;

            // Если пересечение позади камеры, то игнорируем
            if (t < 0)
            {
                return Vector3.Zero;
            }

            // Вычисляем точку пересечения
            return origin + t * direction;
        }
    }
}
