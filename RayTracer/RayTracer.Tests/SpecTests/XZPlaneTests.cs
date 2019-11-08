using System.Linq;
using System.Numerics;
using RayTracer.Core;
using RayTracer.Core.Shapes;
using Shouldly;
using Xunit;
using static System.MathF;
using static System.Numerics.Matrix4x4;
using static System.Numerics.Vector4;
using static RayTracer.Core.Tuples;
using static RayTracer.Tests.SpecTests.Framework.Comparisons;

namespace RayTracer.Tests.SpecTests
{
    /// <summary>
    /// planes.feature
    /// </summary>
        public class XZPlaneTests
    {
        //Scenario: The normal of a plane is constant everywhere
        //  Given p ← plane()
        //  When n1 ← local_normal_at(p, point(0, 0, 0))
        //    And n2 ← local_normal_at(p, point(10, 0, -10))
        //    And n3 ← local_normal_at(p, point(-5, 0, 150))
        //  Then n1 = vector(0, 1, 0)
        //    And n2 = vector(0, 1, 0)
        //    And n3 = vector(0, 1, 0)
        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(10, 0, -10)]
        [InlineData(-5, 0, 150)]
        public void ShouldHaveConstantNormal(float x, float y, float z)
        {
            var p = new XZPlane();
            var n = p.GetLocalNormalAt(CreatePoint(x, y, z));
            AssertActualEqualToExpected(n, CreateVector(0, 1, 0));
        }

        //Scenario: Intersect with a ray parallel to the plane
        //  Given p ← plane()
        //    And r ← ray(point(0, 10, 0), vector(0, 0, 1))
        //  When xs ← local_intersect(p, r)
        //  Then xs is empty
        //
        //Scenario: Intersect with a coplanar ray
        //  Given p ← plane()
        //    And r ← ray(point(0, 0, 0), vector(0, 0, 1))
        //  When xs ← local_intersect(p, r)
        //  Then xs is empty
        [Theory]
        [InlineData(0, 10, 0)]
        [InlineData(0, 0, 0)]
        public void ShouldNotIntersectWithParallelRay(float x, float y, float z)
        {
            var p = new XZPlane();
            var r = CreateRay(CreatePoint(x, y, z), CreateVector(0, 0, 1));
            var xs = p.LocalIntersect(r);
            xs.ShouldBeEmpty();
        }

        //Scenario: A ray intersecting a plane from above
        //  Given p ← plane()
        //    And r ← ray(point(0, 1, 0), vector(0, -1, 0))
        //  When xs ← local_intersect(p, r)
        //  Then xs.count = 1
        //    And xs[0].t = 1
        //    And xs[0].object = p
        //
        //Scenario: A ray intersecting a plane from below
        //  Given p ← plane()
        //    And r ← ray(point(0, -1, 0), vector(0, 1, 0))
        //  When xs ← local_intersect(p, r)
        //  Then xs.count = 1
        //    And xs[0].t = 1
        //    And xs[0].object = p
        [Theory]
        [InlineData(0, 1, 0, 0, -1, 0)]
        [InlineData(0, -1, 0, 0, 1, 0)]
        public void ShouldIntersectWithPerpendicularRay(float px, float py, float pz, float vx, float vy, float vz)
        {
            var p = new XZPlane();
            var r = CreateRay(CreatePoint(px, py, pz), CreateVector(vx, vy, vz));
            var xs = p.LocalIntersect(r);
            xs.ShouldHaveSingleItem().T.ShouldBe(1);
            xs.First().Shape.ShouldBe(p);
        }
    }
}
