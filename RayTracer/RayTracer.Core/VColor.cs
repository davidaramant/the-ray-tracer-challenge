using System.Numerics;

namespace RayTracer.Core
{
    public static class VColor
    {
        public static Vector4 Create(float r, float g, float b) => new Vector4(r, g, b, 0);

        public static bool IsColor(this Vector4 tuple) => 
            tuple.W.IsEquivalentTo(0);

        public static readonly Vector4 Black = Vector4.Zero;
        public static readonly Vector4 White = Create(1, 1, 1);
        public static readonly Vector4 Red = Create(1, 0, 0);
        public static readonly Vector4 Green = Create(0, 1, 0);
        public static readonly Vector4 Blue = Create(0, 0, 1);
    }
}
