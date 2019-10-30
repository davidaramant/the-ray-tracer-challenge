using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using static RayTracer.Core.Tuples;
using static System.Numerics.Matrix4x4;
using static System.Numerics.Vector4;
using static System.MathF;

namespace RayTracer.Core
{
    public sealed class World
    {
        public List<IShape> Objects { get; } = new List<IShape>();
        public List<PointLight> Lights { get; } = new List<PointLight>();

        public List<Intersection> Intersect(Ray ray)
        {
            var xs = new List<Intersection>();

            foreach (var o in Objects)
            {
                xs.AddRange(o.Intersect(ray));
            }

            xs.Sort();

            return xs;
        }

        public static World CreateDefault() => new World
        {
            Lights = { new PointLight(CreatePoint(-10, 10, -10), VColor.Create(1, 1, 1)) },
            Objects =
            {
                new Sphere
                {
                    Material =
                    {
                        Color = VColor.Create(0.8f,1,0.6f),
                        Diffuse = 0.7f,
                        Specular = 0.2f,
                    }
                },
                new Sphere
                {
                    Transform = CreateScale(0.5f, 0.5f, 0.5f),
                },
            },
        };

        public Vector4 ShadeHit(Computations comp) =>
            Lights.Select(light =>
                comp.Object.Material.ComputeColor(light, comp.Point, comp.EyeV, comp.NormalV))
            .Aggregate(VColor.Black, (finalColor, color) => finalColor + color);

        public Vector4 ComputeColor(Ray ray)
        {
            var xs = Intersect(ray);

            var hit = xs.TryGetHit();
            if (hit == null)
            {
                return VColor.Black;
            }

            var comp = Computations.Prepare(hit, ray);

            return ShadeHit(comp);
        }
    }
}