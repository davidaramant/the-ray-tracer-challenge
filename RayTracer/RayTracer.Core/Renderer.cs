﻿using System;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using static System.Numerics.Vector4;
using static RayTracer.Core.Tuples;
using static RayTracer.Core.Graphics;

namespace RayTracer.Core
{
    public static class Renderer
    {
        public delegate void DrawPixel(int x, int y, Vector4 color);

        public static Task TraceScene(Size canvasSize, DrawPixel drawPixel, Action reportRowRendered = null)
        {
            var rayOrigin = CreatePoint(0, 0, -5);
            var wallZ = 10f;
            var wallSize = 7f;
            var halfWallSize = wallSize / 2;

            var pixelSize = new SizeF(wallSize / canvasSize.Width, wallSize / canvasSize.Height);
            var sphereColor = CreateColor(1, 0, 0);
            var shape = new Sphere
            {
                Material =
                {
                    Color = CreateColor(1,0.2f,1)
                }
            };

            var light = new PointLight(CreatePoint(-10, 10, -10), CreateColor(1, 1, 1));

            return Task.Factory.StartNew(() => Parallel.For(0, canvasSize.Height, y =>
            {
                var worldY = halfWallSize - pixelSize.Height * y;
                for (int x = 0; x < canvasSize.Width; x++)
                {
                    var worldX = pixelSize.Width * x - halfWallSize;

                    var targetPosition = CreatePoint(worldX, worldY, wallZ);

                    var ray = CreateRay(rayOrigin, Normalize(targetPosition - rayOrigin));
                    var xs = shape.Intersect(ray);

                    if (xs.Any())
                    {
                        var hit = xs.First();

                        var hitPoint = ray.ComputePosition(hit.T);
                        var normal = hit.Shape.GetNormalAt(hitPoint);
                        var eyeVector = -ray.Direction;

                        var color = hit.Shape.Material.GetLight(light, hitPoint, eyeVector, normal);

                        drawPixel(x, y, color);
                    }
                }
                reportRowRendered?.Invoke();
            }));
        }
    }
}