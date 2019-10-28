using System;
using System.Numerics;

namespace RayTracer.Core
{
    public sealed class PointLight : IEquatable<PointLight>
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

        #region Equality
        public bool Equals(PointLight other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Intensity.IsEquivalentTo(other.Intensity) && Position.IsEquivalentTo(other.Position);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is PointLight other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Intensity.Truncate().GetHashCode() * 397) ^ Position.Truncate().GetHashCode();
            }
        }

        public static bool operator ==(PointLight left, PointLight right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PointLight left, PointLight right)
        {
            return !Equals(left, right);
        }
        #endregion
    }
}