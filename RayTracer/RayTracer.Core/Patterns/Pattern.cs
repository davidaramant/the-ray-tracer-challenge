using System;
using System.Numerics;
using static System.Numerics.Matrix4x4;

namespace RayTracer.Core.Patterns
{
    public abstract class Pattern : IPattern
    {
        private Matrix4x4 _transform = Identity;
        private Matrix4x4 _inverseTransform = Identity;

        public Matrix4x4 Transform
        {
            get => _transform;
            set
            {
                _transform = value;
                var result = Invert(value, out _inverseTransform);
#if DEBUG
                if (!result) throw new InvalidOperationException("Could not invert pattern transform");
#endif
            }
        }

        public Matrix4x4 InverseTransform => _inverseTransform;

        public abstract Vector4 GetColorAt(Vector4 point);

    }
}
