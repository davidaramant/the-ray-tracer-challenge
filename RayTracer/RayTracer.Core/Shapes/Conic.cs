using System.Collections.Generic;
using static RayTracer.Core.Tuples;

namespace RayTracer.Core.Shapes
{
    public abstract class Conic : Shape
    {
        public string Name { get; }
        protected Conic(string name) => Name = name;
        public override string ToString() => Name;

        public float Minimum { get; set; } = float.NegativeInfinity;
        public float Maximum { get; set; } = float.PositiveInfinity;
        public bool Closed { get; set; } = false;

        protected static bool CheckCap(Ray ray, float t, float radius = 1)
        {
            var x = ray.Origin.X + t * ray.Direction.X;
            var z = ray.Origin.Z + t * ray.Direction.Z;

            // HACK: Add the tolerance here since two tests fail otherwise.  
            return (x * x + z * z) <= (radius * radius + Tolerance);
        }
    }
}
