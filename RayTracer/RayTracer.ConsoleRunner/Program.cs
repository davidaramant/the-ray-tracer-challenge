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
            var image = new ImageBuffer(1980, 1200);
            var cancelTokenSource = new CancellationTokenSource();

            using (var progress = new ProgressBar(image.Dimensions.Height, "Tracing image..."))
            {
                await Renderer.TraceSceneByRows(
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
