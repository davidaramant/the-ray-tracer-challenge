using System;
using System.Drawing;
using System.Numerics;

namespace RayTracer.Core.Utilities
{
    public interface IOutputBuffer
    {
        Size Dimensions { get; }
        void SetPixel(int x, int y, Vector4 color);
    }
}
