using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using RayTracer.Core;
using static System.MathF;
using static System.Numerics.Matrix4x4;
using static System.Numerics.Vector4;
using static RayTracer.Core.Tuples;
using static RayTracer.Core.Graphics;

namespace RayTracer.Core
{
    public static class Renderer
    {
        public delegate void DrawPixel(int x, int y, Vector4 color);

        public static Task TraceScene(Size canvasSize, DrawPixel drawPixel)
        {
            var camera = CreateRay(CreatePoint(0, 0, -5), CreateVector(0, 0, 1));
            var wallZ = 10f;
            var wallSize = 7f;
            var halfWallSize = wallSize / 2;

            var pixelSize = new SizeF(wallSize/ canvasSize.Width, wallSize / canvasSize.Height);
            var sphereColor = CreateColor(1, 0, 0);
            var shape = new Sphere();

            return Task.Factory.StartNew(() => Parallel.For(0, canvasSize.Height, y =>
            {
                var worldY = halfWallSize - pixelSize.Height * y;
                for (int x = 0; x < canvasSize.Width; x++)
                {
                    var worldX = pixelSize.Width * x - halfWallSize;

                    var targetPosition = CreatePoint(worldX, worldY, wallZ);

                    var ray = CreateRay(camera.Origin, Normalize(targetPosition - camera.Origin));
                    var xs = shape.Intersect(ray);

                    if (xs != null)
                    {
                        drawPixel(x, y, sphereColor);
                    }
                }
            }));
        }
    }
}
