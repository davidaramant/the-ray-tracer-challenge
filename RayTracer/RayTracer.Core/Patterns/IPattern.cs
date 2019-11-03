using System.Numerics;

namespace RayTracer.Core.Patterns
{
    public interface IPattern
    {
        Vector4 GetColorAt(Vector4 point);
    }
}