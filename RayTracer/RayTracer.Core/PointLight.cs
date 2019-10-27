using System;
using System.Numerics;

namespace RayTracer.Core
{
    public sealed class PointLight
    {
        public Vector4 Position { get; }
        public Vector4 Intensity { get; }

        public PointLight(Vector4 position, Vector4 intensity)
        {
#if DEBUG
            if (!position.IsPoint())
                throw new ArgumentException("Position is not a point", nameof(position));
            if (!intensity.IsColor())
                throw new ArgumentException("Intensity is not a color", nameof(intensity));
#endif

            Position = position;
            Intensity = intensity;
        }
    }
}