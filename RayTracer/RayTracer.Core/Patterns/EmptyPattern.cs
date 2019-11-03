using System;
using System.Numerics;

namespace RayTracer.Core.Patterns
{
    public sealed class EmptyPattern : IPattern
    {
        public static readonly IPattern Instance = new EmptyPattern();

        private EmptyPattern(){}

        Vector4 IPattern.GetColorAt(Vector4 point)
        {
            throw new NotImplementedException();
        }
    }
}
