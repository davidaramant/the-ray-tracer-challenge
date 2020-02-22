using System;
using RayTracer.Core.Patterns;
using RayTracer.Core.Shapes;
using RayTracer.Core.Utilities;
using static System.Numerics.Matrix4x4;
using static RayTracer.Core.Tuples;

namespace RayTracer.Core
{
    public static class TestScene
    {
        public static World CreateTestWorld()
        {
            var floor = new XZPlane("floor")
            {
                Material =
                {
                    Pattern = new CheckerPattern(VColor.LinearRGB(1,0.5f,0.5f), VColor.Red)
                    {
                        Transform = CreateScale(2,2,2) * CreateShear(0,1,0,0,0,0)
                    },
                    Specular = 0,
                    Reflective = 0.1f,
                },
            };
            var water = new XZPlane("water")
            {
                Transform = CreateTranslation(0,0.25f,0),
                Material =
                {
                    Color = VColor.LinearRGB(0,0,0.1f),
                    Reflective = 0.4f,
                    RefractiveIndex = 1.7f,
                    Transparency = 0.9f,
                }
            };
            var rightWall = new XZPlane("right wall")
            {
                Transform =
                    CreateRotationZ(-MathF.PI / 2) *
                    CreateTranslation(5, 0, 0),
                Material =
                {
                    Pattern = new RingPattern(VColor.Green, VColor.White),
                    Specular = 0,
                },
            };
            var leftWall = new XZPlane("left wall")
            {
                Transform =
                        CreateRotationX(MathF.PI / 2) *
                        CreateTranslation(0, 0, 5),
                Material =
                {
                    Pattern = new StripePattern(VColor.Blue, VColor.LinearRGB(0.5f,0.5f,1)),
                    Specular = 0,
                },
            };
            var middle = new Sphere("middle")
            {
                Transform = CreateTranslation(-0.5f, 1, 0.5f),
                Material =
                {
                    Pattern = new GradientPattern(VColor.LinearRGB(0.1f,1,0.5f), VColor.SRGB(1,0.5f,0))
                    {
                        Transform =
                            CreateScale(2,1,1) *
                            CreateTranslation(1,0,0),
                    },
                    Diffuse = 0.7f,
                    Specular = 0.3f,
                    Reflective = 0.05f,
                    Transparency = 0.3f,
                    RefractiveIndex = 1.5f,
                }
            };
            var right = Sphere.CreateGlass("right");
            right.Transform = CreateScale(0.5f, 0.5f, 0.5f) *
                              CreateTranslation(1.5f, 0.5f, -0.5f);
            right.Material.Color = VColor.Black;

            var left = new Cone("left")
            {
                Closed = true,
                Minimum = -1,
                Maximum = 1,
                Transform = CreateScale(0.33f, 1, 0.33f) *
                            CreateTranslation(-1.5f, 1, -0.75f),
                Material =
                {
                    Color = VColor.White,
                    //Ambient = 0.1f,
                    //Diffuse = 0.4f,
                    //Specular = 0,
                }
            };

            return new World
            {
                Objects = { floor, water, rightWall, leftWall, middle, right, left, },
                Lights =
                {
                    new PointLight(CreatePoint(-10, 10, -10), VColor.LinearRGB(0.3f,0.3f,0.3f)),
                    new PointLight(CreatePoint(0, 10, -10), VColor.LinearRGB(0.3f,0.3f,0.3f)),
                    new PointLight(CreatePoint(3, 10, -10), VColor.LinearRGB(0.3f,0.3f,0.3f)),
                },
            };
        }

        public static Camera CreateCamera(IOutputBuffer output) => new Camera(output, MathF.PI / 3)
        {
            Transform = CreateViewTransform(
                    from: CreatePoint(-5, 3, -5),
                    to: CreatePoint(0, 1, 0),
                    up: CreateVector(0, 1, 0)),
        };
    }
}
