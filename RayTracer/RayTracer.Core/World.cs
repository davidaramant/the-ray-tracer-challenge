﻿using System.Collections.Generic;
using static RayTracer.Core.Graphics;
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

        public static World CreateDefault() => new World
        {
            Lights = {new PointLight(CreatePoint(-10, 10, -10), CreateColor(1, 1, 1))},
            Objects =
            {
                new Sphere
                {
                    Material =
                    {
                        Color = CreateColor(0.8f,1,0.6f),
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
    }
}