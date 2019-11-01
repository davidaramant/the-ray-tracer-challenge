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
            var image = new FastImage(100, 100);

            using (var progress = new ProgressBar(image.Height, "Tracing image..."))
            {
                await Renderer.TraceScene(
                    TestScene.CreateCamera(image.Dimensions), 
                    TestScene.CreateTestWorld(), 
                    image.SetPixel, 
                    () => progress.Tick());
            }

            image.Save("output.png");
        }
    }
}
