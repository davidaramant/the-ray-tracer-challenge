using System;
using System.Numerics;
using System.Threading.Tasks;

namespace RayTracer.Core
{
    public static class Renderer
    {
        public delegate void DrawPixel(int x, int y, Vector4 color);

        public static Task TraceScene(Camera camera, World world, DrawPixel drawPixel, Action reportRowRendered = null) =>
            Task.Factory.StartNew(() => Parallel.For(0, camera.Dimensions.Height, y =>
            {
                for (int x = 0; x < camera.Dimensions.Width; x++)
                {
                    var ray = camera.CreateRayForPixel(x, y);
                    var color = world.ComputeColor(ray);
                    drawPixel(x, y, color);
                }
                reportRowRendered?.Invoke();
            }));
    }
}
