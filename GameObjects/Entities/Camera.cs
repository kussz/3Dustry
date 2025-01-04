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

        public Vector3 IntersectRayWithPlane(Viewport viewport)
        {
            (Vector3 rayOrigin, Vector3 rayDirection) = UnprojectMouseToRay(Yaw, -Pitch, viewport, GetProjectionMatrix(), GetViewMatrix());
            Vector3 planeNormal = new Vector3(0, 1, 0);
            float planeD = 0;
            float denom = Vector3.Dot(planeNormal, rayDirection);
            if (Math.Abs(denom) > 1e-6f)
            {
                float t = -(Vector3.Dot(planeNormal, rayOrigin) + planeD) / denom;
                return rayOrigin + t * rayDirection;
            }
            return Vector3.Zero; // Нет пересечения
        }

        public  (Vector3 rayOrigin, Vector3 rayDirection) UnprojectMouseToRay(
        float mouseX, float mouseY,
        Viewport viewport,
        Matrix projectionMatrix,
        Matrix viewMatrix)
        {
            // Преобразование координат мыши в диапазон [-1, 1]
            float pointX = (mouseX / viewport.Width) * 2.0f - 1.0f;
            float pointY = 1.0f - (mouseY / viewport.Height) * 2.0f;

            // Координаты в пространстве устройства (Near и Far)
            Vector3 nearPoint = new Vector3(pointX, pointY, 0.0f);
            Vector3 farPoint = new Vector3(pointX, pointY, 1.0f);

            // Обратное преобразование (Unproject)
            Matrix viewProjectionInverse = Matrix.Invert(viewMatrix * projectionMatrix);
            Vector3 nearWorld = Vector3.Unproject(nearPoint, viewport.X, viewport.Y, viewport.Width, viewport.Height, 0.0f, 1.0f, viewProjectionInverse);
            Vector3 farWorld = Vector3.Unproject(farPoint, viewport.X, viewport.Y, viewport.Width, viewport.Height, 0.0f, 1.0f, viewProjectionInverse);

            // Направление луча
            Vector3 rayDirection = Vector3.Normalize(farWorld - nearWorld);
            return (nearWorld, rayDirection);
        }
    }
}
