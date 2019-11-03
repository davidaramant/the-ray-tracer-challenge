using System;
using System.Numerics;
using static System.MathF;

namespace RayTracer.Core.Patterns
{
    public sealed class StripePattern : Pattern
    {
        public StripePattern(Vector4 a, Vector4 b)
        {
#if DEBUG
            if (!a.IsColor()) throw new ArgumentException("Expected color", nameof(a));
            if (!b.IsColor()) throw new ArgumentException("Expected color", nameof(b));
#endif

            A = a;
            B = b;
        }

        public Vector4 A { get; }
        public Vector4 B { get; }

        public override Vector4 GetColorAt(Vector4 point)
        {
#if DEBUG
            if (!point.IsPoint()) throw new ArgumentException("Expected point", nameof(point));
#endif

            return (int)Floor(point.X) % 2 == 0 ? A : B;
        }
    }
}
