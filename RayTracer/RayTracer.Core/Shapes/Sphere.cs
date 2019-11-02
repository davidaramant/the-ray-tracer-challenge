using System.Collections.Generic;
using System.Numerics;
using static System.MathF;
using static System.Numerics.Vector4;

namespace RayTracer.Core.Shapes
{
    public sealed class Sphere : Shape
    {
        public string Name { get; }

        public Sphere(string name = null)
        {
            Name = name ?? "Sphere";
        }

        public override string ToString() => Name;

        private bool SolveQuadratic(float a, float b, float c, ref float t0, ref float t1)
        {
            void Swap(ref float one, ref float two)
            {
                var temp = one;
                one = two;
                two = temp;
            }

            float discriminant = b * b - 4 * a * c;
            if (discriminant < 0)
            {
                return false;
            }
            else if (discriminant.IsZero())
            {
                t0 = t1 = -0.5f * b / a;
            }
            else
            {
                float q = b > 0 ?
                    -0.5f * (b + Sqrt(discriminant)) :
                    -0.5f * (b - Sqrt(discriminant));
                t0 = q / a;
                t1 = c / q;
            }

            if (t0 > t1)
            {
                Swap(ref t0, ref t1);
            }

            return true;
        }

        public override List<Intersection> LocalIntersect(Ray localRay)
        {
            var sphereToRay = localRay.Origin;
            sphereToRay.W = 0;

            var a = Dot(localRay.Direction, localRay.Direction);
            var b = 2 * Dot(localRay.Direction, sphereToRay);
            var c = Dot(sphereToRay, sphereToRay) - 1;
            float t0 = 0;
            float t1 = 0;
            if (!SolveQuadratic(a, b, c, ref t0, ref t1))
            {
                return new List<Intersection>();
            }

            return new List<Intersection>
            {
                new Intersection(t0, this),
                new Intersection(t1, this),
            };
        }

        public override Vector4 GetLocalNormalAt(Vector4 localPoint)
        {
            var localNormal = localPoint;
            localNormal.W = 0;
            return localNormal;
        }
    }
}
