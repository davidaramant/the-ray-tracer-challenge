﻿using System.Numerics;
using RayTracer.Core.Shapes;
using static System.Numerics.Vector4;

namespace RayTracer.Core
{
    public sealed class Computations
    {
        public float T { get; }
        public IShape Object { get; }
        public Vector4 Point { get; }
        public Vector4 OverPoint { get; }
        public Vector4 FarOverPoint { get; }
        public Vector4 EyeV { get; }
        public Vector4 NormalV { get; }
        public bool Inside { get; }

        private Computations(float t, IShape o, Vector4 point, Vector4 eyeV, Vector4 normalV)
        {
            T = t;
            Object = o;
            Point = point;
            EyeV = eyeV;
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
            OverPoint = point + NormalV * Tuples.Tolerance;
            // HACK: Move the point pretty far for use in IsShadowed.  Some floating point error somewhere causes issues otherwise
            FarOverPoint = point + NormalV * 0.001f;
        }

        public static Computations Prepare(Intersection i, Ray ray)
        {
            var point = ray.ComputePosition(i.T);
            return new Computations(i.T, i.Shape, point, -ray.Direction, i.Shape.GetNormalAt(point));
        }
    }
}