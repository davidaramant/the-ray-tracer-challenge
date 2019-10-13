namespace RayTracer
{
    public sealed class Matrix2x2
    {
        public float M11 { get; }
        public float M12 { get; }
        public float M21 { get; }
        public float M22 { get; }

        public Matrix2x2(float m11, float m12, float m21, float m22)
        {
            M11 = m11;
            M12 = m12;
            M21 = m21;
            M22 = m22;
        }
    }
}
