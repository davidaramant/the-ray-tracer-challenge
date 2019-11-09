using System;
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
            Action reportRowRendered = null) =>
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        Parallel.For(
                            0,
                            camera.Dimensions.Height,
                            new ParallelOptions
                            {
                                CancellationToken = cancelToken,
                            },
                            (y, loopState) =>
                            {
                                for (int x = 0; x < camera.Dimensions.Width && !loopState.ShouldExitCurrentIteration; x++)
                                {
                                    var ray = camera.CreateRayForPixel(x, y);
                                    var color = world.ComputeColor(ray, maximumReflections);
                                    drawPixel(x, y, color);
                                }

                                reportRowRendered?.Invoke();
                            });
                    }
                    catch (OperationCanceledException)
                    {
                        // Why does this throw??? What am I supposed to do here other than ignore it???
                    }
                });
    }
}
