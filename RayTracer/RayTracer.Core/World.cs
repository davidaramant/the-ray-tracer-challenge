using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using RayTracer.Core.Shapes;
using SharpDX.WIC;
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
                        Color = VColor.LinearRGB(0.8f,1,0.6f),
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

        public Vector4 ShadeHit(Computations comp, int remainingReflections = 0)
        {
            var surfaceColor = Lights.Select(light =>
                    comp.Object.Material.ComputeColor(
                        light,
                        comp.Object,
                        comp.OverPoint,
                        comp.EyeV,
                        comp.NormalV,
                        inShadow: IsShadowed(light, comp.FarOverPoint)))
                .Aggregate(VColor.Black, (finalColor, color) => finalColor + color);

            var reflectedColor = ComputeReflectedColor(comp, remainingReflections);
            var refractedColor = ComputeRefractedColor(comp, remainingReflections);

            var material = comp.Object.Material;
            if (material.Reflective > 0 && material.Transparency > 0)
            {
                var reflectance = comp.GetSchlickReflectance();
                return surfaceColor + reflectedColor * reflectance + refractedColor * (1 - reflectance);
            }
            else
            {
                return surfaceColor + reflectedColor + refractedColor;
            }
        }

        public Vector4 ComputeColor(Ray ray, int remainingReflections = 0)
        {
            var xs = Intersect(ray);

            var hit = xs.TryGetHit();
            if (hit == null)
            {
                return VColor.Black;
            }

            var comp = Computations.Prepare(hit, ray, xs);

            return ShadeHit(comp, remainingReflections);
        }

        public bool IsShadowed(PointLight light, Vector4 point)
        {
            var v = light.Position - point;
            var distance = v.Length();
            var direction = Normalize(v);

            var r = CreateRay(point, direction);
            var xs = Intersect(r);
            var hit = xs.TryGetHit();
            return hit?.T < distance;
        }

        public Vector4 ComputeReflectedColor(Computations comps, int remainingReflections = 0)
        {
            if (comps.Object.Material.Reflective.IsZero() || remainingReflections == 0)
            {
                return VColor.Black;
            }

            var reflectRay = CreateRay(comps.OverPoint, comps.ReflectV);
            var color = ComputeColor(reflectRay, remainingReflections - 1);

            return color * comps.Object.Material.Reflective;
        }

        public Vector4 ComputeRefractedColor(Computations comps, int remainingReflections)
        {
            if (comps.Object.Material.Transparency.IsZero() || remainingReflections == 0)
            {
                return VColor.Black;
            }

            var nRatio = comps.N1 / comps.N2;
            var cosI = Dot(comps.EyeV, comps.NormalV);
            var sinT2 = nRatio * nRatio * (1 - cosI * cosI);
            if (sinT2 > 1)
            {
                return VColor.Black;
            }

            var cosT = Sqrt(1 - sinT2);
            var direction = comps.NormalV * (nRatio * cosI - cosT) - comps.EyeV * nRatio;
            var refractRay = new Ray(comps.UnderPoint, direction);

            return ComputeColor(refractRay, remainingReflections - 1) * comps.Object.Material.Transparency;
        }
    }
}