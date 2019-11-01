using System.Numerics;
using Colourful;
using Colourful.Conversion;

namespace RayTracer.Core
{
    public static class VColor
    {
        public static Vector4 LinearRGB(float r, float g, float b) => new Vector4(r, g, b, 0);

        public static Vector4 SRGB(float r, float g, float b)
        {
            var converter = new ColourfulConverter();
            var srgb = new RGBColor(r, g, b);
            var linear = converter.ToLinearRGB(srgb);
            return new Vector4((float)linear.R, (float)linear.G, (float)linear.B, 0);
        }

        public static bool IsColor(this Vector4 tuple) =>
            tuple.W.IsEquivalentTo(0);

        public static readonly Vector4 Black = Vector4.Zero;
        public static readonly Vector4 White = LinearRGB(1, 1, 1);
        public static readonly Vector4 Red = LinearRGB(1, 0, 0);
        public static readonly Vector4 Green = LinearRGB(0, 1, 0);
        public static readonly Vector4 Blue = LinearRGB(0, 0, 1);
    }
}
