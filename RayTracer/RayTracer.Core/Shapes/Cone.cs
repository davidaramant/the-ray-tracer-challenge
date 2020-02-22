using System.Collections.Generic;
using System.Numerics;
using static RayTracer.Core.Tuples;
using static System.MathF;

namespace RayTracer.Core.Shapes
{
    public sealed class Cone : Conic
    {
        public Cone(string name = null) : base(name ?? "cone")
        {
        }

        public override List<Intersection> LocalIntersect(Ray localRay)
        {
            var xs = new List<Intersection>();

            var a =
                localRay.Direction.X * localRay.Direction.X -
                localRay.Direction.Y * localRay.Direction.Y +
                localRay.Direction.Z * localRay.Direction.Z;

            var b =
                2 * localRay.Origin.X * localRay.Direction.X -
                2 * localRay.Origin.Y * localRay.Direction.Y +
                2 * localRay.Origin.Z * localRay.Direction.Z;

            var c =
                localRay.Origin.X * localRay.Origin.X -
                localRay.Origin.Y * localRay.Origin.Y +
                localRay.Origin.Z * localRay.Origin.Z;

            if (a.IsZero())
            {
                if (!b.IsZero())
                {
                    xs.Add(new Intersection(-c / (2 * b), this));
                }
            }
            else
            {
                var disc = b * b - 4 * a * c;
                if (disc >= 0)
                {
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
            }

            IntersectCaps(localRay, xs);

            return xs;
        }

        private void IntersectCaps(Ray ray, List<Intersection> xs)
        {
            if (!Closed || ray.Direction.Y.IsZero())
            {
                return;
            }

            var lowerT = (Minimum - ray.Origin.Y) / ray.Direction.Y;
            if (CheckCap(ray, lowerT, Minimum))
            {
                xs.Add(new Intersection(lowerT, this));
            }

            var upperT = (Maximum - ray.Origin.Y) / ray.Direction.Y;
            if (CheckCap(ray, upperT, Maximum))
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

            var y = Sqrt(localPoint.X * localPoint.X + localPoint.Z * localPoint.Z);
            if (localPoint.Y > 0)
            {
                y = -y;
            }

            return CreateVector(localPoint.X, y, localPoint.Z);
        }
    }
}