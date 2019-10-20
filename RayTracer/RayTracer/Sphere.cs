﻿using System;
using System.Collections.Generic;
using System.Numerics;
using static System.MathF;
using static System.Numerics.Matrix4x4;
using static System.Numerics.Vector4;
using static RayTracer.Tuples;

namespace RayTracer
{
    public sealed class Sphere : IShape
    {
        public Matrix4x4 Transform { get; set; } = Matrix4x4.Identity;
        public Vector4 Position { get; }
        public float Radius { get; }

        public Sphere() : this(CreatePoint(0, 0, 0), 1)
        {
        }

        public Sphere(Vector4 position, float radius)
        {
#if DEBUG
            if (!position.IsPoint()) throw new ArgumentException("Position is not point", nameof(position));
            if (radius < 0) throw new ArgumentOutOfRangeException(nameof(radius), "Radius was negative");
#endif

            Position = position;
            Radius = radius;
        }

        public override string ToString() => "Sphere";

        public List<Intersection> Intersect(Ray ray)
        {
            Invert(Transform, out var inverse);
            var ray2 = ray.Transform(ref inverse);
            var sphereToRay = ray2.Origin - Position;

            var a = Dot(ray2.Direction, ray2.Direction);
            var b = 2 * Dot(ray2.Direction, sphereToRay);
            var c = Dot(sphereToRay, sphereToRay) - 1;

            var discriminant = b * b - 4 * a * c;

            if (discriminant < 0)
            {
                return new List<Intersection>();
            }
            else
            {
                return new List<Intersection>
                {
                    new Intersection((-b - Sqrt(discriminant)) / (2*a), this),
                    new Intersection((-b + Sqrt(discriminant)) / (2*a), this),
                };
            }
        }
    }
}
