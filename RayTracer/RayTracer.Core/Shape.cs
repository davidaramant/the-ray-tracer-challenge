using System.Collections.Generic;
using System.Numerics;
using static System.Numerics.Matrix4x4;
using static System.Numerics.Vector4;

namespace RayTracer.Core
{
    public abstract class Shape : IShape
    {
        private Matrix4x4 _transform = Identity;
        protected Matrix4x4 _inverseTransform = Identity;

        public Matrix4x4 Transform
        {
            get => _transform;
            set
            {
                _transform = value;
                Invert(value, out _inverseTransform);
            }
        }

        public Material Material { get; set; } = new Material();

        public List<Intersection> Intersect(Ray ray)
        {
            var localRay = ray.Transform(ref _inverseTransform);
            return LocalIntersect(localRay);
        }

        protected abstract List<Intersection> LocalIntersect(Ray localRay);

        public Vector4 GetNormalAt(Vector4 worldPoint)
        {
            var localPoint = Transform(worldPoint, _inverseTransform);
            var localNormal = GetLocalNormalAt(localPoint);
            var worldNormal = Transform(localNormal, Transpose(_inverseTransform));
            worldNormal.W = 0;
            return Normalize(worldNormal);
        }

        protected abstract Vector4 GetLocalNormalAt(Vector4 localPoint);
    }
}
