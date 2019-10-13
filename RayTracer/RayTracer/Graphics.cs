using System.Numerics;

namespace RayTracer
{
    public delegate void DrawPixel(int x, int y, Vector4 color);

    public static class Graphics
    {
        public static Vector4 MakeColor(float r, float g, float b) => new Vector4(r, g, b, 0);
    }
}
