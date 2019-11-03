using System;
using System.Numerics;

namespace RayTracer.Core.Patterns
{
    public sealed class EmptyPattern : IPattern
    {
        public static readonly IPattern Instance = new EmptyPattern();

        private EmptyPattern(){}

        Matrix4x4 IPattern.Transform { get; set; } = Matrix4x4.Identity;
        Matrix4x4 IPattern.InverseTransform { get; } = Matrix4x4.Identity;

        Vector4 IPattern.GetColorAt(Vector4 point)
        {
            throw new NotImplementedException();
        }
    }
}
