using System.Numerics;

namespace RayTracer
{
    public static class Tuples
    {
        public static Vector4 MakePoint(float x, float y, float z) => new Vector4(x, y, z, 1);
        public static Vector4 MakeVector(float x, float y, float z) => new Vector4(x, y, z, 0);

        public static Vector4 Cross(Vector4 a, Vector4 b) =>
            MakeVector(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X);
    }
}
