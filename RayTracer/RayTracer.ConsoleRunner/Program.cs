using System.Threading;
using System.Threading.Tasks;
using RayTracer.Core;
using RayTracer.Core.Utilities;
using ShellProgressBar;

namespace RayTracer.ConsoleRunner
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var image = new ImageBuffer(800, 800);
            var cancelTokenSource = new CancellationTokenSource();

            using (var progress = new ProgressBar(image.Height, "Tracing image..."))
            {
                await Renderer.TraceScene(
                    TestScene.CreateCamera(image),
                    TestScene.CreateTestWorld(),
                    image.SetPixel,
                    maximumReflections: 5,
                    cancelTokenSource.Token,
                    () => progress.Tick());
            }

            image.Save("output.png");
        }
    }
}
