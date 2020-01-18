using System;
using System.Collections.Generic;
using System.Numerics;
using static System.MathF;
using static RayTracer.Core.FloatExtensions;
using static RayTracer.Core.Tuples;

namespace RayTracer.Core.Shapes
{
    public sealed class Cube : Shape
    {
        public string Name { get; }
        public Cube(string name = null) => Name = name ?? "cube";
        public override string ToString() => Name;

        public override List<Intersection> LocalIntersect(Ray localRay)
        {
            var (xTMin, xTMax) = CheckAxis(localRay.Origin.X, localRay.Direction.X);
            var (yTMin, yTMax) = CheckAxis(localRay.Origin.Y, localRay.Direction.Y);
            var (zTMin, zTMax) = CheckAxis(localRay.Origin.Z, localRay.Direction.Z);

            var tMin = Max(xTMin, yTMin, zTMin);
            var tMax = Min(xTMax, yTMax, zTMax);

            if (tMin > tMax)
            {
                return new List<Intersection>();
            }

            return new List<Intersection> { new Intersection(tMin, this), new Intersection(tMax, this) };
        }

        private (float tMin, float tMax) CheckAxis(float origin, float direction)
        {
            var tMinNumerator = -1 - origin;
            var tMaxNumerator = 1 - origin;

            float tMin = tMinNumerator / direction;
            float tMax = tMaxNumerator / direction;

            return (tMin > tMax) ? (tMax, tMin) : (tMin, tMax);
        }

        public override Vector4 GetLocalNormalAt(Vector4 localPoint)
        {
            var maxC = Max(Abs(localPoint.X), Abs(localPoint.Y), Abs(localPoint.Z));

            if (maxC.IsEquivalentTo(Abs(localPoint.X)))
            {
                return CreateVector(localPoint.X, 0, 0);
            }
            else if (maxC.IsEquivalentTo(Abs(localPoint.Y)))
            {
                return CreateVector(0, localPoint.Y, 0);
            }
            else
            {
                return CreateVector(0, 0, localPoint.Z);
            }
        }
    }
}
