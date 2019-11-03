using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using static System.Numerics.Matrix4x4;
using static System.Numerics.Vector4;

namespace RayTracer.Core.Shapes
{
    public abstract class Shape : IShape
    {
        private Matrix4x4 _transform = Identity;
        protected Matrix4x4 InverseTransform = Identity;

        public Matrix4x4 Transform
        {
            get => _transform;
            set
            {
                _transform = value;
                var result = Invert(value, out InverseTransform);
#if DEBUG
                if (!result) throw new InvalidOperationException("Could not invert shape transform");
#endif

            }
        }

        public Material Material { get; set; } = new Material();

        public Vector4 GetPatternColorAt(Vector4 worldPoint)
        {
            var objectPoint = Transform(worldPoint, InverseTransform);
            var patternPoint = Transform(objectPoint, Material.Pattern.InverseTransform);
            return Material.Pattern.GetColorAt(patternPoint);
        }

        public List<Intersection> Intersect(Ray ray)
        {
            var localRay = ray.Transform(ref InverseTransform);
            return LocalIntersect(localRay);
        }

        public abstract List<Intersection> LocalIntersect(Ray localRay);

        public Vector4 GetNormalAt(Vector4 worldPoint)
        {
            var localPoint = Transform(worldPoint, InverseTransform);
            var localNormal = GetLocalNormalAt(localPoint);
            var worldNormal = Transform(localNormal, Transpose(InverseTransform));
            worldNormal.W = 0;
            return Normalize(worldNormal);
        }

        public abstract Vector4 GetLocalNormalAt(Vector4 localPoint);
    }
}
