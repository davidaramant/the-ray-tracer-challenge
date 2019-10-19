using System.Numerics;

namespace RayTracer
{
    public static class Tuples
    {
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
    }
}
