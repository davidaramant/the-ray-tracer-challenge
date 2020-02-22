using System.Collections.Generic;
using System.Numerics;
using static RayTracer.Core.Tuples;
using static System.MathF;

namespace RayTracer.Core.Shapes
{
    public sealed class Cylinder : Shape
    {
        public string Name { get; }
        public Cylinder(string name = null) => Name = name ?? "cylinder";
        public override string ToString() => Name;

        public float Minimum { get; set; } = float.NegativeInfinity;
        public float Maximum { get; set; } = float.PositiveInfinity;
        public bool Closed { get; set; } = false;

        public override List<Intersection> LocalIntersect(Ray localRay)
        {
            var a = localRay.Direction.X * localRay.Direction.X + localRay.Direction.Z * localRay.Direction.Z;

            var xs = new List<Intersection>();

            if (!a.IsZero())
            {

                var b =
                    2 * localRay.Origin.X * localRay.Direction.X +
                    2 * localRay.Origin.Z * localRay.Direction.Z;

                var c = localRay.Origin.X * localRay.Origin.X + localRay.Origin.Z * localRay.Origin.Z - 1;

                var disc = b * b - 4 * a * c;
                if (disc < 0)
                {
                    return new List<Intersection>();
                }

                var t0 = (-b - Sqrt(disc)) / (2 * a);
                var t1 = (-b + Sqrt(disc)) / (2 * a);

                
                var y0 = localRay.Origin.Y + t0 * localRay.Direction.Y;
                if (Minimum < y0 && y0 < Maximum)
                {
                    xs.Add(new Intersection(t0, this));
                }

                var y1 = localRay.Origin.Y + t1 * localRay.Direction.Y;
                if (Minimum < y1 && y1 < Maximum)
                {
                    xs.Add(new Intersection(t1, this));
                }
            }

            IntersectCaps(localRay, xs);

            return xs;
        }

        private static bool CheckCap(Ray ray, float t)
        {
            var x = ray.Origin.X + t * ray.Direction.X;
            var z = ray.Origin.Z + t * ray.Direction.Z;

            // HACK: Add the tolerance here since two tests fail otherwise.  
            return (x * x + z * z) <= (1 + Tolerance);
        }

        private void IntersectCaps(Ray ray, List<Intersection> xs)
        {
            if (!Closed || ray.Direction.Y.IsZero())
            {
                return;
            }

            var lowerT = (Minimum - ray.Origin.Y) / ray.Direction.Y;
            if (CheckCap(ray, lowerT))
            {
                xs.Add(new Intersection(lowerT, this));
            }

            var upperT = (Maximum - ray.Origin.Y) / ray.Direction.Y;
            if (CheckCap(ray, upperT))
            {
                xs.Add(new Intersection(upperT, this));
            }
        }

        public override Vector4 GetLocalNormalAt(Vector4 localPoint)
        {
            var dist = localPoint.X * localPoint.X + localPoint.Z * localPoint.Z;
            if (dist < 1 && localPoint.Y >= (Maximum - Tolerance))
            {
                return CreateVector(0, 1, 0);
            }
            else if (dist < 1 && localPoint.Y <= (Minimum + Tolerance))
            {
                return CreateVector(0, -1, 0);
            }

            return CreateVector(localPoint.X, 0, localPoint.Z);
        }
    }
}
