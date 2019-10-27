using System.Numerics;

namespace RayTracer.Core
{
    public delegate void DrawPixel(int x, int y, Vector4 color);

    public static class Graphics
    {
        public static Vector4 CreateColor(float r, float g, float b) => new Vector4(r, g, b, 0);

        public static bool IsColor(this Vector4 tuple) => 
            tuple.W.EquivalentTo(0);

    }
}
