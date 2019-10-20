using System.Collections.Generic;
using System.Numerics;

namespace RayTracer.Core
{
    public interface IShape
    {
        Matrix4x4 Transform { get; set; }
        List<Intersection> Intersect(Ray ray);
    }
}