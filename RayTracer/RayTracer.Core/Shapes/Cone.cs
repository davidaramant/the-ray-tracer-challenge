using System.Collections.Generic;
using System.Numerics;
using static RayTracer.Core.Tuples;
using static System.MathF;

namespace RayTracer.Core.Shapes
{
    public sealed class Cone : Shape
    {
        public string Name { get; }
        public Cone(string name = null) => Name = name ?? "cone";
        public override string ToString() => Name;

        public float Minimum { get; set; } = float.NegativeInfinity;
        public float Maximum { get; set; } = float.PositiveInfinity;
        public bool Closed { get; set; } = false;

        public override List<Intersection> LocalIntersect(Ray localRay)
        {
            throw new System.NotImplementedException();
        }

        public override Vector4 GetLocalNormalAt(Vector4 localPoint)
        {
            throw new System.NotImplementedException();
        }
    }
}