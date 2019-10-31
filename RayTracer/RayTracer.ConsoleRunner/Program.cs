using System;
using System.Threading.Tasks;
using RayTracer.Core;
using RayTracer.Core.Utilities;
using ShellProgressBar;
using static RayTracer.Core.Tuples;

namespace RayTracer.ConsoleRunner
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var image = new FastImage(100, 100);

            using (var progress = new ProgressBar(image.Height, "Tracing image..."))
            {
                var camera = new Camera(image.Dimensions, MathF.PI / 3)
                {
                    Transform = CreateViewTransform(
                        from: CreatePoint(0, 1.5f, -5),
                        to: CreatePoint(0, 1, 0),
                        up: CreateVector(0, 1, 0)),
                };

                await Renderer.TraceScene(camera, World.CreateTestWorld(), image.SetPixel, () => progress.Tick());
            }

            image.Save("output.png");
        }
    }
}
