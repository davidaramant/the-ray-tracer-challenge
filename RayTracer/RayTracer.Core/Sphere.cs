using System;
using System.Collections.Generic;
using System.Numerics;
using static System.MathF;
using static System.Numerics.Matrix4x4;
using static System.Numerics.Vector4;
using static RayTracer.Core.Tuples;

namespace RayTracer.Core
{
    public sealed class Sphere : IShape
    {
        private Matrix4x4 _transform = Identity;
        private Matrix4x4 _inverseTransform = Identity;

        public Matrix4x4 Transform
        {
            get => _transform;
            set
            {
                _transform = value;
                Invert(value, out _inverseTransform);
            }
        }

        public Vector4 Position { get; }
        public float Radius { get; }
        public Material Material { get; set; } = new Material();

        public Sphere() : this(CreatePoint(0, 0, 0), 1)
        {
        }

        public Sphere(Vector4 position, float radius)
        {
#if DEBUG
            if (!position.IsPoint()) throw new ArgumentException("Position is not point", nameof(position));
            if (radius < 0) throw new ArgumentOutOfRangeException(nameof(radius), "Radius was negative");
#endif

            Position = position;
            Radius = radius;
        }

        public override string ToString() => "Sphere";

        public List<Intersection> Intersect(Ray ray)
        {
            var ray2 = ray.Transform(ref _inverseTransform);
            var sphereToRay = ray2.Origin - Position;

            var a = Dot(ray2.Direction, ray2.Direction);
            var b = 2 * Dot(ray2.Direction, sphereToRay);
            var c = Dot(sphereToRay, sphereToRay) - 1;

            var discriminant = b * b - 4 * a * c;

            if (discriminant < 0)
            {
                return new List<Intersection>();
            }
            else
            {
                return new List<Intersection>
                {
                    new Intersection((-b - Sqrt(discriminant)) / (2*a), this),
                    new Intersection((-b + Sqrt(discriminant)) / (2*a), this),
                };
            }
        }

        public Vector4 GetNormalAt(Vector4 worldPoint)
        {
            var objectPoint = Transform(worldPoint, _inverseTransform);
            var objectNormal = objectPoint - Position;
            var worldNormal = Transform(objectNormal, Transpose(_inverseTransform));
            worldNormal.W = 0;
            return Normalize(worldNormal);
        }

        #region Equality
        public bool Equals(IShape other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return 
                other is Sphere o &&
                _transform.Equals(o._transform) && 
                Material.Equals(o.Material) && 
                Position.IsEquivalentTo(o.Position) && 
                Radius.IsEquivalentTo(o.Radius);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is Sphere other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = _transform.GetHashCode();
                hashCode = (hashCode * 397) ^ Material.GetHashCode();
                hashCode = (hashCode * 397) ^ Position.Truncate().GetHashCode();
                hashCode = (hashCode * 397) ^ Radius.Truncate().GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Sphere left, Sphere right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Sphere left, Sphere right)
        {
            return !Equals(left, right);
        }
        #endregion
    }
}
