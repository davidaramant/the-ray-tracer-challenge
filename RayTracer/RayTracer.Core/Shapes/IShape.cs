using System.Collections.Generic;
using System.Numerics;

namespace RayTracer.Core.Shapes
{
    public interface IShape
    {
        Matrix4x4 Transform { get; set; }
        Material Material { get; set; }

        Vector4 GetNormalAt(Vector4 worldPoint);
        List<Intersection> Intersect(Ray ray);
        Vector4 GetPatternColorAt(Vector4 worldPoint);
    }
}