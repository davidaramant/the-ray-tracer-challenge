using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Moq;
using RayTracer.Core;
using RayTracer.Core.Utilities;
using FluentAssertions;
using Xunit;
using System.Numerics;
using RayTracer.Core.Shapes;
using static System.MathF;
using static System.Numerics.Matrix4x4;
using static System.Numerics.Vector4;
using static RayTracer.Core.Tuples;
using static RayTracer.Tests.SpecTests.Framework.Comparisons;

namespace RayTracer.Tests.SpecTests
{
    /// <summary>
    /// cylinders.feature
    /// </summary>
    public class CylinderTests
    {
        //Scenario Outline: A ray misses a cylinder
        //  Given cyl ← cylinder()
        //    And direction ← normalize(<direction>)
        //    And r ← ray(<origin>, direction)
        //  When xs ← local_intersect(cyl, r)
        //  Then xs.count = 0

        //  Examples:
        //    | origin          | direction       |
        //    | point(1, 0, 0)  | vector(0, 1, 0) |
        //    | point(0, 0, 0)  | vector(0, 1, 0) |
        //    | point(0, 0, -5) | vector(1, 1, 1) |
        [Theory]
        [InlineData(1, 0, 0, 0, 1, 0)]
        [InlineData(0, 0, 0, 0, 1, 0)]
        [InlineData(0, 0, -5, 1, 1, 1)]
        public void ShouldNotIntersectWhenRayMissesCylinder(float oX, float oY, float oZ, float dX, float dY, float dZ)
        {
            var cyl = new Cylinder();
            var direction = Normalize(CreateVector(dX, dY, dZ));
            var r = CreateRay(CreatePoint(oX, oY, oZ), direction);
            var xs = cyl.LocalIntersect(r);
            xs.Count.Should().Be(0);
        }

        //Scenario Outline: A ray strikes a cylinder
        //  Given cyl ← cylinder()
        //    And direction ← normalize(<direction>)
        //    And r ← ray(<origin>, direction)
        //  When xs ← local_intersect(cyl, r)
        //  Then xs.count = 2
        //    And xs[0].t = <t0>
        //    And xs[1].t = <t1>

        //  Examples:
        //    | origin            | direction         | t0      | t1      |
        //    | point(1, 0, -5)   | vector(0, 0, 1)   | 5       | 5       |
        //    | point(0, 0, -5)   | vector(0, 0, 1)   | 4       | 6       |
        //    | point(0.5, 0, -5) | vector(0.1, 1, 1) | 6.80798 | 7.08872 |
        [Theory]
        [InlineData(1, 0, -5, 0, 0, 1, 5, 5)]
        [InlineData(0, 0, -5, 0, 0, 1, 4, 6)]
        [InlineData(0.5, 0, -5, 0.1, 1, 1, 6.80798, 7.08872)]
        public void ShouldIntersectWithRayStrikesCylinder(
            float oX, float oY, float oZ,
            float dX, float dY, float dZ,
            float t0, float t1)
        {
            var cyl = new Cylinder();
            var direction = Normalize(CreateVector(dX, dY, dZ));
            var r = CreateRay(CreatePoint(oX, oY, oZ), direction);
            var xs = cyl.LocalIntersect(r);
            xs.Count.Should().Be(2);
            xs[0].T.Should().BeApproximately(t0, Tolerance);
            xs[1].T.Should().BeApproximately(t1, Tolerance);
        }

        //Scenario Outline: Normal vector on a cylinder
        //  Given cyl ← cylinder()
        //  When n ← local_normal_at(cyl, <point>)
        //  Then n = <normal>

        //  Examples:
        //    | point           | normal           |
        //    | point(1, 0, 0)  | vector(1, 0, 0)  |
        //    | point(0, 5, -1) | vector(0, 0, -1) |
        //    | point(0, -2, 1) | vector(0, 0, 1)  |
        //    | point(-1, 1, 0) | vector(-1, 0, 0) |
        [Theory]
        [InlineData(1, 0, 0, 1, 0, 0)]
        [InlineData(0, 5, -1, 0, 0, -1)]
        [InlineData(0, -2, 1, 0, 0, 1)]
        [InlineData(-1, 1, 0, -1, 0, 0)]
        public void ShouldComputeNormalOnCylinder(float pX, float pY, float pZ, float nX, float nY, float nZ)
        {
            var cyl = new Cylinder();
            var n = cyl.GetLocalNormalAt(CreatePoint(pX, pY, pZ));
            AssertActualEqualToExpected(n, CreateVector(nX, nY, nZ));
        }

        //Scenario: The default minimum and maximum for a cylinder
        //  Given cyl ← cylinder()
        //  Then cyl.minimum = -infinity
        //    And cyl.maximum = infinity
        [Fact]
        public void ShouldHaveCorrectDefaultMinimumAndMaximum()
        {
            var cyl = new Cylinder();
            cyl.Minimum.Should().Be(float.NegativeInfinity);
            cyl.Maximum.Should().Be(float.PositiveInfinity);
        }

        //Scenario Outline: Intersecting a constrained cylinder
        //  Given cyl ← cylinder()
        //    And cyl.minimum ← 1
        //    And cyl.maximum ← 2
        //    And direction ← normalize(<direction>)
        //    And r ← ray(<point>, direction)
        //  When xs ← local_intersect(cyl, r)
        //  Then xs.count = <count>

        //  Examples:
        //    |   | point             | direction         | count |
        //    | 1 | point(0, 1.5, 0)  | vector(0.1, 1, 0) | 0     |
        //    | 2 | point(0, 3, -5)   | vector(0, 0, 1)   | 0     |
        //    | 3 | point(0, 0, -5)   | vector(0, 0, 1)   | 0     |
        //    | 4 | point(0, 2, -5)   | vector(0, 0, 1)   | 0     |
        //    | 5 | point(0, 1, -5)   | vector(0, 0, 1)   | 0     |
        //    | 6 | point(0, 1.5, -2) | vector(0, 0, 1)   | 2     |
        [Theory]
        [InlineData(0, 1.5, 0, 0.1, 1, 0, 0)]
        [InlineData(0, 3, -5, 0, 0, 1, 0)]
        [InlineData(0, 0, -5, 0, 0, 1, 0)]
        [InlineData(0, 2, -5, 0, 0, 1, 0)]
        [InlineData(0, 1, -5, 0, 0, 1, 0)]
        [InlineData(0, 1.5, -2, 0, 0, 1, 2)]
        public void ShouldIntersectConstrainedCylinder(
            float pX, float pY, float pZ,
            float dX, float dY, float dZ,
            int count)
        {
            var cyl = new Cylinder
            {
                Minimum = 1,
                Maximum = 2
            };
            var direction = Normalize(CreateVector(dX, dY, dZ));
            var r = CreateRay(CreatePoint(pX, pY, pZ), direction);
            var xs = cyl.LocalIntersect(r);
            xs.Count.Should().Be(count);
        }

        //Scenario: The default closed value for a cylinder
        //  Given cyl ← cylinder()
        //  Then cyl.closed = false
        [Fact]
        public void ShouldNotBeClosedByDefault()
        {
            var cyl = new Cylinder();
            cyl.Closed.Should().Be(false);
        }

        //Scenario Outline: Intersecting the caps of a closed cylinder
        //  Given cyl ← cylinder()
        //    And cyl.minimum ← 1
        //    And cyl.maximum ← 2
        //    And cyl.closed ← true
        //    And direction ← normalize(<direction>)
        //    And r ← ray(<point>, direction)
        //  When xs ← local_intersect(cyl, r)
        //  Then xs.count = <count>

        //  Examples:
        //    |   | point            | direction        | count |
        //    | 1 | point(0, 3, 0)   | vector(0, -1, 0) | 2     |
        //    | 2 | point(0, 3, -2)  | vector(0, -1, 2) | 2     |
        //    | 3 | point(0, 4, -2)  | vector(0, -1, 1) | 2     | # corner case
        //    | 4 | point(0, 0, -2)  | vector(0, 1, 2)  | 2     |
        //    | 5 | point(0, -1, -2) | vector(0, 1, 1)  | 2     | # corner case
        [Theory]
        [InlineData(0, 3, 0, 0, -1, 0, 2)]
        [InlineData(0, 3, -2, 0, -1, 2, 2)]
        [InlineData(0, 4, -2, 0, -1, 1, 2)]
        [InlineData(0, 0, -2, 0, 1, 2, 2)]
        [InlineData(0, -1, -2, 0, 1, 1, 2)]
        public void ShouldIntersectTheCapsOfClosedCylinder(
            float pX, float pY, float pZ,
            float dX, float dY, float dZ,
            int count)
        {
            var cyl = new Cylinder
            {
                Minimum = 1,
                Maximum = 2,
                Closed = true,
            };
            var direction = Normalize(CreateVector(dX, dY, dZ));
            var r = CreateRay(CreatePoint(pX, pY, pZ), direction);
            var xs = cyl.LocalIntersect(r);
            xs.Count.Should().Be(count);
        }

        //Scenario Outline: The normal vector on a cylinder's end caps
        //  Given cyl ← cylinder()
        //    And cyl.minimum ← 1
        //    And cyl.maximum ← 2
        //    And cyl.closed ← true
        //  When n ← local_normal_at(cyl, <point>)
        //  Then n = <normal>

        //  Examples:
        //    | point            | normal           |
        //    | point(0, 1, 0)   | vector(0, -1, 0) |
        //    | point(0.5, 1, 0) | vector(0, -1, 0) |
        //    | point(0, 1, 0.5) | vector(0, -1, 0) |
        //    | point(0, 2, 0)   | vector(0, 1, 0)  |
        //    | point(0.5, 2, 0) | vector(0, 1, 0)  |
        //    | point(0, 2, 0.5) | vector(0, 1, 0)  |
        [Theory]
        [InlineData(0, 1, 0, 0, -1, 0)]
        [InlineData(0.5, 1, 0, 0, -1, 0)]
        [InlineData(0, 1, 0.5, 0, -1, 0)]
        [InlineData(0, 2, 0, 0, 1, 0)]
        [InlineData(0.5, 2, 0, 0, 1, 0)]
        [InlineData(0, 2, 0.5, 0, 1, 0)]
        public void ShouldComputeNormalVectorOfCylinderEndCaps(
            float pX, float pY, float pZ,
            float nX, float nY, float nZ)
        {
            var cyl = new Cylinder
            {
                Minimum = 1,
                Maximum = 2,
                Closed = true,
            };
            var n = cyl.GetLocalNormalAt(CreatePoint(pX, pY, pZ));
            AssertActualEqualToExpected(n, CreateVector(nX, nY, nZ));
        }


    }
}
