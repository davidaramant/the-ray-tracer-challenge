using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace RayTracer.Core
{
    public static class Renderer
    {
        public delegate void DrawPixel(int x, int y, Vector4 color);

        public static Task TraceScene(
            Camera camera,
            World world,
            DrawPixel drawPixel,
            int maximumReflections,
            CancellationToken cancelToken,
            Action reportPixelRendered = null) =>
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        Parallel.ForEach(
                            Shuffled(GetAllPoints(camera.Dimensions)),
                            new ParallelOptions
                            {
                                CancellationToken = cancelToken,
                            },
                            (position, loopState) =>
                            {
                                var ray = camera.CreateRayForPixel(position.X, position.Y);
                                var color = world.ComputeColor(ray, maximumReflections);
                                drawPixel(position.X, position.Y, color);

                                reportPixelRendered?.Invoke();
                            });
                    }
                    catch (OperationCanceledException)
                    {
                        // Why does this throw??? What am I supposed to do here other than ignore it???
                    }
                });

        private static IEnumerable<Point> GetAllPoints(Size dimensions)
        {
            for (int y = 0; y < dimensions.Height; y++)
            {
                for (int x = 0; x < dimensions.Width; x++)
                {
                    yield return new Point(x, y);
                }
            }
        }

        private static IEnumerable<T> Shuffled<T>(IEnumerable<T> sequence)
        {
            var list = sequence.ToList();
            var rand = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }
    }
}
