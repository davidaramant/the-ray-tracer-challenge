using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using RayTracer.Core.Shapes;
using static System.Numerics.Vector4;
using static RayTracer.Core.Tuples;

namespace RayTracer.Core
{
    public sealed class Computations
    {
        public float T { get; }
        public IShape Object { get; }
        public Vector4 Point { get; }
        public Vector4 OverPoint { get; }
        public Vector4 FarOverPoint { get; }
        public Vector4 UnderPoint { get; }
        public Vector4 EyeV { get; }
        public Vector4 NormalV { get; }
        public Vector4 ReflectV { get; }
        public bool Inside { get; }
        public float N1 { get; }
        public float N2 { get; }

        private Computations(float t, IShape o, Vector4 point, Vector4 eyeV, Vector4 normalV, float n1, float n2)
        {
            T = t;
            Object = o;
            Point = point;
            EyeV = eyeV;
            N1 = n1;
            N2 = n2;
            if (Dot(normalV, eyeV) < 0)
            {
                Inside = true;
                NormalV = -normalV;
            }
            else
            {
                Inside = false;
                NormalV = normalV;
            }

            ReflectV = Reflect(-eyeV, NormalV);
            OverPoint = point + NormalV * Tolerance;
            UnderPoint = point - NormalV * Tolerance;
            // HACK: Move the point pretty far for use in IsShadowed.  Some floating point error somewhere causes issues otherwise
            FarOverPoint = point + NormalV * 0.001f;
        }

        public static Computations Prepare(Intersection hit, Ray ray) => Prepare(hit, ray, new List<Intersection> { hit });

        public static Computations Prepare(Intersection hit, Ray ray, List<Intersection> xs)
        {
            float n1 = 0;
            float n2 = 0;

            var containers = new List<IShape>();

            foreach (var i in xs)
            {
                if (i == hit)
                {
                    n1 = !containers.Any() ? 1 : containers.Last().Material.RefractiveIndex;
                }

                if (containers.Contains(i.Shape))
                {
                    containers.Remove(i.Shape);
                }
                else
                {
                    containers.Add(i.Shape);
                }

                if (i == hit)
                {
                    n2 = !containers.Any() ? 1 : containers.Last().Material.RefractiveIndex;
                    break;
                }
            }

            var point = ray.ComputePosition(hit.T);
            return new Computations(hit.T, hit.Shape, point, -ray.Direction, hit.Shape.GetNormalAt(point), n1, n2);
        }
    }
}