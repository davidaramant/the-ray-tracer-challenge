using System.Numerics;
using static System.Numerics.Vector4;

namespace RayTracer.Core
{
    public sealed class Computations
    {
        public float T { get; }
        public IShape Object { get; }
        public Vector4 Point { get; }
        public Vector4 EyeV { get; }
        public Vector4 NormalV { get; }
        public bool Inside { get; }

        private Computations(float t, IShape o, Vector4 point, Vector4 eyeV, Vector4 normalV)
        {
            T = t;
            Object = o;
            Point = point;
            EyeV = eyeV;
            NormalV = normalV;
            if (Dot(normalV, eyeV) < 0)
            {
                Inside = true;
                NormalV = -NormalV;
            }
            else
            {
                Inside = false;
            }
        }

        public static Computations Prepare(Intersection i, Ray ray)
        {
            var point = ray.ComputePosition(i.T);
            return new Computations(i.T, i.Shape, point, -ray.Direction, i.Shape.GetNormalAt(point));
        }
    }
}