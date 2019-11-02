using System;
using RayTracer.Core.Shapes;

namespace RayTracer.Core
{
    public sealed class Intersection : IEquatable<Intersection>, IComparable<Intersection>
    {
        public float T { get; }
        public IShape Shape { get; }

        public Intersection(float t, IShape shape)
        {
            T = t;
            Shape = shape;
        }

        public override string ToString() => $"T:{T} {Shape}";

        public int CompareTo(Intersection other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return T.CompareTo(other.T);
        }

        #region Equality

        public bool Equals(Intersection other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Shape, other.Shape) && T.IsEquivalentTo(other.T);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is Intersection other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Shape != null ? Shape.GetHashCode() : 0) * 397) ^ T.Truncate().GetHashCode();
            }
        }

        public static bool operator ==(Intersection left, Intersection right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Intersection left, Intersection right)
        {
            return !Equals(left, right);
        }

        #endregion

    }
}
