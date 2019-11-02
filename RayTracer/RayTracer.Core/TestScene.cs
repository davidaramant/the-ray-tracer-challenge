using System;
using System.Drawing;
using RayTracer.Core.Utilities;
using static System.Numerics.Matrix4x4;
using static RayTracer.Core.Tuples;

namespace RayTracer.Core
{
    public static class TestScene
    {
        public static World CreateTestWorld()
        {
            var floor = new Sphere
            {
                Transform = CreateScale(10, 0.01f, 10),
                Material =
                {
                    Color = VColor.SRGB(1, 0.9f, 0.9f),
                    Specular = 0,
                },
            };
            var leftWall = new Sphere
            {
                Transform = CreateScale(10, 0.01f, 10) *
                            CreateRotationZ(MathF.PI / 2) *
                            CreateRotationY(-MathF.PI / 4) *
                            CreateTranslation(0, 0, 5),
                Material = floor.Material,
            };
            var rightWall = new Sphere
            {
                Transform = CreateScale(10, 0.01f, 10) *
                            CreateRotationX(MathF.PI / 2) *
                            CreateRotationY(-MathF.PI / 4) *
                            CreateTranslation(0, 0, 5),
                Material = floor.Material,
            };
            var middle = new Sphere
            {
                Transform = CreateTranslation(-0.5f, 1, 0.5f),
                Material =
                {
                    Color = VColor.LinearRGB(0.1f,1,0.5f),
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
                    Color = VColor.SRGB(0.5f,1,0.1f),
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
                    Color = VColor.SRGB(1,0.8f,0.1f),
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

        public static Camera CreateCamera(IOutputBuffer output) => new Camera(output, MathF.PI / 3)
            {
                Transform = CreateViewTransform(
                    from: CreatePoint(0, 1.5f, -5),
                    to: CreatePoint(0, 1, 0),
                    up: CreateVector(0, 1, 0)),
            };
    }
}
