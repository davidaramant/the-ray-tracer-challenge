using System;
using System.Numerics;
using static System.Numerics.Vector4;

namespace RayTracer.Core
{
    public static class Tuples
    {
        public const float Tolerance = 0.00001f;

        public static bool IsZero(this float f) => MathF.Abs(f) < Tolerance;

        public static float Truncate(this float f) => MathF.Round(f, 5);

        public static bool IsEquivalentTo(this float a, float b) => System.MathF.Abs(a - b) < Tolerance;

        public static bool IsEquivalentTo(this Vector4 a, Vector4 b) =>
            a.X.IsEquivalentTo(b.X) &&
            a.Y.IsEquivalentTo(b.Y) &&
            a.Z.IsEquivalentTo(b.Z) &&
            a.W.IsEquivalentTo(b.W);

        public static Vector4 Truncate(this Vector4 a) => new Vector4(a.X.Truncate(), a.Y.Truncate(), a.Z.Truncate(), a.W.Truncate());

        public static bool IsPoint(this Vector4 tuple) => tuple.W.IsEquivalentTo(1);
        public static bool IsVector(this Vector4 tuple) => tuple.W.IsEquivalentTo(0);

        public static Vector4 CreatePoint(float x, float y, float z) => new Vector4(x, y, z, 1);
        public static Vector4 CreateVector(float x, float y, float z) => new Vector4(x, y, z, 0);

        public static Vector4 Cross(ref Vector4 a, ref Vector4 b) =>
            CreateVector(
                a.Y * b.Z - a.Z * b.Y,
                a.Z * b.X - a.X * b.Z,
                a.X * b.Y - a.Y * b.X);

        public static Vector4 GetRow1(ref Matrix4x4 m) => new Vector4(m.M11, m.M12, m.M13, m.M14);
        public static Vector4 GetRow2(ref Matrix4x4 m) => new Vector4(m.M21, m.M22, m.M23, m.M24);
        public static Vector4 GetRow3(ref Matrix4x4 m) => new Vector4(m.M31, m.M32, m.M33, m.M34);
        public static Vector4 GetRow4(ref Matrix4x4 m) => new Vector4(m.M41, m.M42, m.M43, m.M44);

        public static Vector4 Multiply(ref Matrix4x4 m, ref Vector4 a) => new Vector4(
                Vector4.Dot(GetRow1(ref m), a),
                Vector4.Dot(GetRow2(ref m), a),
                Vector4.Dot(GetRow3(ref m), a),
                Vector4.Dot(GetRow4(ref m), a));

        public static Vector4 Multiply(Matrix4x4 m, ref Vector4 a) => new Vector4(
            Vector4.Dot(GetRow1(ref m), a),
            Vector4.Dot(GetRow2(ref m), a),
            Vector4.Dot(GetRow3(ref m), a),
            Vector4.Dot(GetRow4(ref m), a));

        public static Matrix4x4 CreateShear(float xy, float xz, float yx, float yz, float zx, float zy) =>
            new Matrix4x4(
                1, yx, zx, 0,
                xy, 1, zy, 0,
                xz, yz, 1, 0,
                0, 0, 0, 1);

        public static Ray CreateRay(Vector4 origin, Vector4 direction) => new Ray(origin, direction);

        public static Vector4 Reflect(Vector4 direction, Vector4 normal) =>
            direction - normal * 2 * Vector4.Dot(direction, normal);

        public static Matrix4x4 CreateViewTransform(Vector4 from, Vector4 to, Vector4 up)
        {
            var forward = Normalize(to - from);
            var upNormal = Normalize(up);
            var left = Cross(ref forward, ref upNormal);
            var trueUp = Cross(ref left, ref forward);

            var orientation = new Matrix4x4(
                left.X, trueUp.X, -forward.X, 0,
                left.Y, trueUp.Y, -forward.Y, 0,
                left.Z, trueUp.Z, -forward.Z, 0,
                0, 0, 0, 1);

            return Matrix4x4.CreateTranslation(-from.X, -from.Y, -from.Z) * orientation;
        }
    }
}
