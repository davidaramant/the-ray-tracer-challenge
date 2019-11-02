using System.Collections.Generic;
using System.Numerics;
using static RayTracer.Core.Tuples;

namespace RayTracer.Core
{
    public sealed class XZPlane : Shape
    {
        public string Name { get; }

        public XZPlane(string name = null)
        {
            Name = name ?? "Plane";
        }

        public override string ToString() => Name;

        public override List<Intersection> LocalIntersect(Ray localRay)
        {
            if (localRay.Direction.Y.IsZero())
            {
                return new List<Intersection>();
            }

            return new List<Intersection>
            {
                new Intersection(-localRay.Origin.Y/ localRay.Direction.Y, this)
            };
        }

        public override Vector4 GetLocalNormalAt(Vector4 localPoint) => CreateVector(0, 1, 0);
    }
}
