namespace RayTracer.Core
{
    public static class FloatExtensions
    {
        public static bool IsInRange(this float num, float min, float max) => num >= min && num <= max;
        public static bool IsInUnitRange(this float num) => num.IsInRange(0, 1);
        public static bool IsPositive(this float num) => num >= 0;
    }
}
