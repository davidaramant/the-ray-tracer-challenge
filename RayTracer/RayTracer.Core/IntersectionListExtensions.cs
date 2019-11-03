using System.Collections.Generic;
using System.Linq;

namespace RayTracer.Core
{
    public static class IntersectionListExtensions
    {
        public static Intersection TryGetHit(this List<Intersection> intersections) =>
            intersections.Where(i => i.T >= 0).OrderBy(i => i.T).FirstOrDefault();
    }
}
