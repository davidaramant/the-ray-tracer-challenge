using System;
using RayTracer.Core.Utilities;
using static System.Numerics.Matrix4x4;
using static RayTracer.Core.Tuples;

namespace RayTracer.Core
{
    public static class TestScene
    {
        public static World CreateTestWorld()
        {
            var floor = new Sphere("floor")
            {
                Transform = CreateScale(10, 0.01f, 10),
                Material =
                {
                    Color = VColor.Red,
                    Specular = 0,
                },
            };
            var leftWall = new Sphere("right wall")
            {
                Transform = CreateScale(10, 0.01f, 10) *
                            CreateRotationZ(MathF.PI / 2) *
                            CreateRotationY(-MathF.PI / 4) *
                            CreateTranslation(0, 0, 5),
                Material =
                {
                    Color = VColor.Green,
                    Specular = 0,
                },
            };
            var rightWall = new Sphere("left wall")
            {
                Transform = CreateScale(10, 0.01f, 10) *
                            CreateRotationX(MathF.PI / 2) *
                            CreateRotationY(-MathF.PI / 4) *
                            CreateTranslation(0, 0, 5),
                Material =
                {
                    Color = VColor.Blue,
                    Specular = 0,
                },
            };
            var middle = new Sphere("middle")
            {
                Transform = CreateTranslation(-0.5f, 1, 0.5f),
                Material =
                {
                    Color = VColor.LinearRGB(0.1f,1,0.5f),
                    Diffuse = 0.7f,
                    Specular = 0.3f,
                }
            };
            var right = new Sphere("right")
            {
                Transform = CreateScale(0.5f, 0.5f, 0.5f) *
                            CreateTranslation(1.5f, 0.5f, -0.5f),
                Material =
                {
                    Color = VColor.Black,
                    Diffuse = 0.7f,
                    Specular = 0.3f,
                }
            };
            var left = new Sphere("left")
            {
                Transform = CreateScale(0.33f, 0.33f, 0.33f) *
                            CreateTranslation(-1.5f, 0.33f, -0.75f),
                Material =
                {
                    Color = VColor.White,
                    Diffuse = 0.7f,
                    Specular = 0.3f,
                }
            };

            return new World
            {
                Objects = { floor, leftWall, rightWall, middle, right, left, },
                Lights =
                {
                    new PointLight(CreatePoint(-10, 10, -10), VColor.LinearRGB(0.3f,0.3f,0.3f)),
                    new PointLight(CreatePoint(0, 10, -10), VColor.LinearRGB(0.3f,0.3f,0.3f)),
                    new PointLight(CreatePoint(10, 10, -10), VColor.LinearRGB(0.3f,0.3f,0.3f)),
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
