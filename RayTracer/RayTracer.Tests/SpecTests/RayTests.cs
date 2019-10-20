using System;
using NUnit.Framework;
using System.Numerics;
using static System.MathF;
using static System.Numerics.Matrix4x4;
using static System.Numerics.Vector4;
using static RayTracer.Tuples;
using static RayTracer.Tests.SpecTests.Framework.Comparisons;

namespace RayTracer.Tests.SpecTests
{
    /// <summary>
    /// rays.feature
    /// </summary>
    [TestFixture]
    public class RayTests
    {
        //Scenario: Creating and querying a ray
        //  Given origin ← point(1, 2, 3)
        //    And direction ← vector(4, 5, 6)
        //  When r ← ray(origin, direction)
        //  Then r.origin = origin
        //    And r.direction = direction
        [Test]
        public void ShouldCreateRay()
        {
            var origin = CreatePoint(1, 2, 3);
            var direction = CreateVector(4, 5, 6);
            var r = new Ray(origin, direction);
            Assert.That(r.Origin, Is.EqualTo(origin));
            Assert.That(r.Direction, Is.EqualTo(direction));
        }

#if DEBUG
        [Test]
        public void ShouldVerifyRayOriginIsPoint() =>
            Assert.Throws<ArgumentException>(() => CreateRay(CreateVector(1, 2, 3), CreateVector(1, 2, 3)));

        [Test]
        public void ShouldVerifyRayDirectionIsVector() =>
            Assert.Throws<ArgumentException>(() => CreateRay(CreatePoint(1, 2, 3), CreatePoint(1, 2, 3)));
#endif

        //Scenario: Computing a point from a distance
        //  Given r ← ray(point(2, 3, 4), vector(1, 0, 0))
        //  Then position(r, 0) = point(2, 3, 4)
        //    And position(r, 1) = point(3, 3, 4)
        //    And position(r, -1) = point(1, 3, 4)
        //    And position(r, 2.5) = point(4.5, 3, 4)
        [TestCase(0, 2)]
        [TestCase(1, 3)]
        [TestCase(-1, 1)]
        [TestCase(2.5f, 4.5f)]
        public void ShouldComputePointFromDistance(float distance, float expectedX)
        {
            var r = CreateRay(CreatePoint(2, 3, 4), CreateVector(1, 0, 0));
            AssertActualEqualToExpected(r.ComputePosition(distance), CreatePoint(expectedX, 3, 4));
        }

        //Scenario: Translating a ray
        //  Given r ← ray(point(1, 2, 3), vector(0, 1, 0))
        //    And m ← translation(3, 4, 5)
        //  When r2 ← transform(r, m)
        //  Then r2.origin = point(4, 6, 8)
        //    And r2.direction = vector(0, 1, 0)
        [Test]
        public void ShouldTranslateRay()
        {
            var r = CreateRay(CreatePoint(1, 2, 3), CreateVector(0, 1, 0));
            var m = CreateTranslation(3, 4, 5);
            var r2 = r.Transform(ref m);
            AssertActualEqualToExpected(r2.Origin, CreatePoint(4, 6, 8));
            AssertActualEqualToExpected(r2.Direction, r.Direction);
        }

        //Scenario: Scaling a ray
        //  Given r ← ray(point(1, 2, 3), vector(0, 1, 0))
        //    And m ← scaling(2, 3, 4)
        //  When r2 ← transform(r, m)
        //  Then r2.origin = point(2, 6, 12)
        //    And r2.direction = vector(0, 3, 0)
        [Test]
        public void ShouldScakeRay()
        {
            var r = CreateRay(CreatePoint(1, 2, 3), CreateVector(0, 1, 0));
            var m = CreateScale(2, 3, 4);
            var r2 = r.Transform(ref m);
            AssertActualEqualToExpected(r2.Origin, CreatePoint(2, 6, 12));
            AssertActualEqualToExpected(r2.Direction, CreateVector(0, 3, 0));
        }
    }
}
