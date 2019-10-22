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

            using (var progress = new ProgressBar(image.PixelCount, "Tracing image..."))
            {
                await Renderer.TraceScene(image.Dimensions, (x, y, color) =>
                    {
                        image.SetPixel(x, y, color);
                        progress.Tick();
                    });
            }

            image.Save("output.png");
        }
    }
}
