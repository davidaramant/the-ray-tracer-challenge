using System;
using System.Numerics;
using static RayTracer.Tuples;

namespace RayTracer
{
    public sealed class Ray
    {
        public Vector4 Origin { get; }
        public Vector4 Direction { get; }

        public Ray(Vector4 origin, Vector4 direction)
        {
#if DEBUG
            if (!origin.IsPoint())
                throw new ArgumentException("Origin is not a point", nameof(origin));
            if (!direction.IsVector())
                throw new ArgumentException("Direction is not a vector", nameof(direction));
#endif

            Origin = origin;
            Direction = direction;
        }

        public Vector4 ComputePosition(float scale) => Origin + scale * Direction;
        public Ray Transform(ref Matrix4x4 m) => CreateRay(Vector4.Transform(Origin, m), Vector4.Transform(Direction, m));
    }
}
