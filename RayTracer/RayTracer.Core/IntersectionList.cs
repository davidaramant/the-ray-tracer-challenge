using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RayTracer.Core
{
    public sealed class IntersectionList : IEnumerable<Intersection>
    {
        private readonly List<Intersection> _intersections = new List<Intersection>();

        public int Count => _intersections.Count;
        public void Add(Intersection intersection) => _intersections.Add(intersection);

        public IEnumerator<Intersection> GetEnumerator() => _intersections.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public Intersection Hit() => _intersections.Where(i => i.T >= 0).OrderBy(i => i.T).FirstOrDefault();
    }
}
