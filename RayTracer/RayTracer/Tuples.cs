using System;
using System.Numerics;

namespace RayTracer
{
    public static class Tuples
    {
        public static bool EquivalentTo(this float a, float b) => Math.Abs(a - b) < 0.0001f;

        public static bool EquivalentTo(this Vector4 a, Vector4 b) =>
            a.X.EquivalentTo(b.X) &&
            a.Y.EquivalentTo(b.Y) &&
            a.Z.EquivalentTo(b.Z) &&
            a.W.EquivalentTo(b.W);

        public static bool IsPoint(this Vector4 tuple) => tuple.W.EquivalentTo(1);
        public static bool IsVector(this Vector4 tuple) => tuple.W.EquivalentTo(0);

        public static Vector4 MakePoint(float x, float y, float z) => new Vector4(x, y, z, 1);
        public static Vector4 MakeVector(float x, float y, float z) => new Vector4(x, y, z, 0);


    }
}
