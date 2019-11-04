namespace RayTracer.Core.Utilities
{
    public enum RenderScale
    {
        Normal = 1,
        Half = 2,
        Quarter = 4,
        Eighth = 8,
    }

    public static class RenderScaleExtensions
    {
        public static bool IsMaximumQuality(this RenderScale scale) => scale == RenderScale.Normal;
        public static bool IsMinimumQuality(this RenderScale scale) => scale == RenderScale.Eighth;

        public static RenderScale DecreaseQuality(this RenderScale scale) =>
            scale.IsMinimumQuality() ? scale : (RenderScale) ((int) scale * 2);
        public static RenderScale IncreaseQuality(this RenderScale scale) =>
            scale.IsMaximumQuality() ? scale : (RenderScale)((int)scale / 2);
    }
}
