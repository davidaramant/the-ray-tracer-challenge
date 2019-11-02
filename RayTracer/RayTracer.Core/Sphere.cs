using System.Collections.Generic;
using System.Numerics;
using static System.MathF;
using static System.Numerics.Vector4;

namespace RayTracer.Core
{
    public sealed class Sphere : Shape
    {
        public string Name { get; }

        public Sphere(string name = null)
        {
            Name = name ?? "Sphere";
        }

        public override string ToString() => Name;

        protected override List<Intersection> LocalIntersect(Ray localRay)
        {
            var sphereToRay = localRay.Origin;
            sphereToRay.W = 0;

            var a = Dot(localRay.Direction, localRay.Direction);
            var b = 2 * Dot(localRay.Direction, sphereToRay);
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

        protected override Vector4 GetLocalNormalAt(Vector4 localPoint)
        {
            var localNormal = localPoint;
            localNormal.W = 0;
            return localNormal;
        }
    }
}
