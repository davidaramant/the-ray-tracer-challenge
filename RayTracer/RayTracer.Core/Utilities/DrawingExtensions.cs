using System.Drawing;

namespace RayTracer.Core.Utilities
{
    public static class DrawingExtensions
    {
        public static int Area(this Size size) => size.Width * size.Height;
        public static Size DivideBy(this Size size, int denominator) => new Size(size.Width / denominator, size.Height / denominator);
        public static Size DivideBy(this Size size, RenderScale scale) => size.DivideBy((int)scale);
    }
}
