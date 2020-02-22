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
using System.Xml;
using RayTracer.Core.Shapes;
using static System.MathF;
using static System.Numerics.Matrix4x4;
using static System.Numerics.Vector4;
using static RayTracer.Core.Tuples;
using static RayTracer.Tests.SpecTests.Framework.Comparisons;

namespace RayTracer.Tests.SpecTests
{
    /// <summary>
    /// cones.feature
    /// </summary>
    public class ConeTests
    {
        //Scenario Outline: Intersecting a cone with a ray
        //  Given shape ← cone()
        //    And direction ← normalize(<direction>)
        //    And r ← ray(<origin>, direction)
        //  When xs ← local_intersect(shape, r)
        //  Then xs.count = 2
        //    And xs[0].t = <t0>
        //    And xs[1].t = <t1>

        //  Examples:
        //    | origin          | direction           | t0      | t1       |
        //    | point(0, 0, -5) | vector(0, 0, 1)     | 5       |  5       |
        //    | point(0, 0, -5) | vector(1, 1, 1)     | 8.66025 |  8.66025 |
        //    | point(1, 1, -5) | vector(-0.5, -1, 1) | 4.55006 | 49.44994 |
        [Theory]
        [InlineData(0, 0, -5, 0, 0, 1, 5, 5)]
        [InlineData(0, 0, -5, 1, 1, 1, 8.66025f, 8.66025f)]
        [InlineData(1, 1, -5, -0.5f, -1, 1, 4.55006f, 49.44994f)]
        public void ShouldIntersectConeWithRay(
            float oX, float oY, float oZ,
            float dX, float dY, float dZ,
            float t0, float t1)
        {
            var shape = new Cone();
            var direction = Normalize(CreateVector(dX, dY, dZ));
            var origin = CreatePoint(oX, oY, oZ);
            var r = new Ray(origin, direction);
            var xs = shape.LocalIntersect(r);
            xs.Count.Should().Be(2);
            xs[0].T.Should().BeApproximately(t0, Tolerance);
            xs[1].T.Should().BeApproximately(t1, Tolerance);
        }

        //Scenario: Intersecting a cone with a ray parallel to one of its halves
        //  Given shape ← cone()
        //    And direction ← normalize(vector(0, 1, 1))
        //    And r ← ray(point(0, 0, -1), direction)
        //  When xs ← local_intersect(shape, r)
        //  Then xs.count = 1
        //    And xs[0].t = 0.35355
        [Fact]
        public void ShouldIntersectConeWithRayParallelToOneHalf()
        {
            var shape = new Cone();
            var direction = Normalize(CreateVector(0, 1, 1));
            var r = new Ray(CreatePoint(0, 0, -1), direction);
            var xs = shape.LocalIntersect(r);
            xs.Count.Should().Be(1);
            xs[0].T.Should().BeApproximately(0.35355f, Tolerance);
        }

        //Scenario Outline: Intersecting a cone's end caps
        //  Given shape ← cone()
        //    And shape.minimum ← -0.5
        //    And shape.maximum ← 0.5
        //    And shape.closed ← true
        //    And direction ← normalize(<direction>)
        //    And r ← ray(<origin>, direction)
        //  When xs ← local_intersect(shape, r)
        //  Then xs.count = <count>

        //  Examples:
        //    | origin             | direction       | count |
        //    | point(0, 0, -5)    | vector(0, 1, 0) | 0     |
        //    | point(0, 0, -0.25) | vector(0, 1, 1) | 2     |
        //    | point(0, 0, -0.25) | vector(0, 1, 0) | 4     |
        [Theory]
        [InlineData(0, 0, -5, 0, 1, 0, 0)]
        [InlineData(0, 0, -0.25f, 0, 1, 1, 2)]
        [InlineData(0, 0, -0.25f, 0, 1, 0, 4)]
        public void ShouldIntersectConeEndCaps(
            float oX, float oY, float oZ,
            float dX, float dY, float dZ,
            int count)
        {
            var shape = new Cone
            {
                Minimum = -0.5f,
                Maximum = 0.5f,
                Closed = true,
            };

            var direction = Normalize(CreateVector(dX, dY, dZ));
            var origin = CreatePoint(oX, oY, oZ);
            var r = new Ray(origin, direction);
            var xs = shape.LocalIntersect(r);
            xs.Count.Should().Be(count);
        }

        //Scenario Outline: Computing the normal vector on a cone
        //  Given shape ← cone()
        //  When n ← local_normal_at(shape, <point>)
        //  Then n = <normal>

        //  Examples:
        //    | point             | normal                 |
        //    | point(0, 0, 0)    | vector(0, 0, 0)        |
        //    | point(1, 1, 1)    | vector(1, -√2, 1)      |
        //    | point(-1, -1, 0)  | vector(-1, 1, 0)       |
        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0)]
        [InlineData(1, 1, 1, 1, -1.4142135623730951f, 1)]
        [InlineData(-1, -1, 0, -1, 1, 0)]
        public void ShouldComputeNormalOnCone(float pX, float pY, float pZ, float nX, float nY, float nZ)
        {
            var shape = new Cone();
            var n = shape.GetLocalNormalAt(CreatePoint(pX, pY, pZ));
            AssertActualEqualToExpected(n, CreateVector(nX, nY, nZ));
        }
    }
}
