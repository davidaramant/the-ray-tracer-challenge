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
            Lights = { new PointLight(CreatePoint(-10, 10, -10), VColor.White) },
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

        public static World CreateTestWorld()
        {
            var floor = new Sphere
            {
                Transform = CreateScale(10, 0.01f, 10),
                Material =
                {
                    Color = VColor.Create(1, 0.9f, 0.9f),
                    Specular = 0,
                },
            };
            var leftWall = new Sphere
            {
                Transform = CreateScale(10, 0.01f, 10) *
                            CreateRotationZ(PI / 2) *
                            CreateRotationY(-PI / 4) *
                            CreateTranslation(0, 0, 5),
                Material = floor.Material,
            };
            var rightWall = new Sphere
            {
                Transform = CreateScale(10, 0.01f, 10) *
                            CreateRotationX(PI / 2) *
                            CreateRotationY(-PI / 4) *
                            CreateTranslation(0, 0, 5),
                Material = floor.Material,
            };
            var middle = new Sphere
            {
                Transform = CreateTranslation(-0.5f, 1, 0.5f),
                Material =
                {
                    Color = VColor.Create(0.1f,1,0.5f),
                    Diffuse = 0.7f,
                    Specular = 0.3f,
                }
            };
            var right = new Sphere
            {
                Transform = CreateScale(0.5f, 0.5f, 0.5f) *
                            CreateTranslation(1.5f, 0.5f, -0.5f),
                Material =
                {
                    Color = VColor.Create(0.5f,1,0.1f),
                    Diffuse = 0.7f,
                    Specular = 0.3f,
                }
            };
            var left = new Sphere
            {
                Transform = CreateScale(0.33f, 0.33f, 0.33f) *
                            CreateTranslation(-1.5f, 0.33f, -0.75f),
                Material =
                {
                    Color = VColor.Create(1,0.8f,0.1f),
                    Diffuse = 0.7f,
                    Specular = 0.3f,
                }
            };

            return new World
            {
                Objects = { floor, leftWall, rightWall, middle, right, left, },
                Lights =
                {
                    new PointLight(CreatePoint(-10, 10, -10), VColor.Blue),
                    new PointLight(CreatePoint(0, 10, -10), VColor.Red),
                    new PointLight(CreatePoint(10, 10, -10), VColor.Green),
                },
            };
        }

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