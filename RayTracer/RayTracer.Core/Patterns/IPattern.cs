using System.Numerics;

namespace RayTracer.Core.Patterns
{
    public interface IPattern
    {
        Matrix4x4 Transform { get; set; }
        Matrix4x4 InverseTransform { get; }
        Vector4 GetColorAt(Vector4 point);
    }
}