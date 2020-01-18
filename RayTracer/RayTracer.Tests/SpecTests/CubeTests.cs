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
using SharpDX.DirectWrite;
using static System.MathF;
using static System.Numerics.Matrix4x4;
using static System.Numerics.Vector4;
using static RayTracer.Core.Tuples;
using static RayTracer.Tests.SpecTests.Framework.Comparisons;

namespace RayTracer.Tests.SpecTests
{
    public class CubeTests
    {
        // Scenario Outline: A ray intersects a cube
        //  Given c ← cube()
        //    And r ← ray(<origin>, <direction>)
        //  When xs ← local_intersect(c, r)
        //  Then xs.count = 2
        //    And xs[0].t = <t1>
        //    And xs[1].t = <t2>
        //  Examples:
        //    |        | origin            | direction        | t1 | t2 |
        //    | +x     | point(5, 0.5, 0)  | vector(-1, 0, 0) |  4 |  6 |
        //    | -x     | point(-5, 0.5, 0) | vector(1, 0, 0)  |  4 |  6 |
        //    | +y     | point(0.5, 5, 0)  | vector(0, -1, 0) |  4 |  6 |
        //    | -y     | point(0.5, -5, 0) | vector(0, 1, 0)  |  4 |  6 |
        //    | +z     | point(0.5, 0, 5)  | vector(0, 0, -1) |  4 |  6 |
        //    | -z     | point(0.5, 0, -5) | vector(0, 0, 1)  |  4 |  6 |
        //    | inside | point(0, 0.5, 0)  | vector(0, 0, 1)  | -1 |  1 |
        [Theory]
        [InlineData(5, 0.5f, 0, -1, 0, 0, 4, 6)]
        [InlineData(-5, 0.5f, 0, 1, 0, 0, 4, 6)]
        [InlineData(0.5f, 5, 0, 0, -1, 0, 4, 6)]
        [InlineData(0.5f, -5, 0, 0, 1, 0, 4, 6)]
        [InlineData(0.5f, 0, 5, 0, 0, -1, 4, 6)]
        [InlineData(0.5f, 0, -5, 0, 0, 1, 4, 6)]
        [InlineData(0, 0.5f, 0, 0, 0, 1, -1, 1)]
        public void ShouldIntersectRayWithCube(float oX, float oY, float oZ, float dX, float dY, float dZ, float t1, float t2)
        {
            var origin = CreatePoint(oX, oY, oZ);
            var direction = CreateVector(dX, dY, dZ);

            var c = new Cube();
            var r = CreateRay(origin, direction);
            var xs = c.LocalIntersect(r);
            xs.Count.Should().Be(2);
            xs[0].T.Should().BeApproximately(t1, Tolerance);
            xs[1].T.Should().BeApproximately(t2, Tolerance);
        }

        //Scenario Outline: A ray misses a cube
        //  Given c ← cube()
        //    And r ← ray(<origin>, <direction>)
        //  When xs ← local_intersect(c, r)
        //  Then xs.count = 0

        //  Examples:
        //    | origin            | direction                        |
        //    | point(-2,  0,  0) | vector(0.2673,  0.5345,  0.8018) |
        //    | point( 0, -2,  0) | vector(0.8018,  0.2673,  0.5345) |
        //    | point( 0,  0, -2) | vector(0.5345,  0.8018,  0.2673) |
        //    | point( 2,  0,  2) | vector(0,       0,      -1     ) |
        //    | point( 0,  2,  2) | vector(0,      -1,       0     ) |
        //    | point( 2,  2,  0) | vector(-1,      0,       0     ) |
        [Theory]
        [InlineData(-2, 0, 0, 0.2673, 0.5345, 0.8018)]
        [InlineData(0, -2, 0, 0.8018, 0.2673, 0.5345)]
        [InlineData(0, 0, -2, 0.5345, 0.8018, 0.2673)]
        [InlineData(2, 0, 2, 0, 0, -1)]
        [InlineData(0, 2, 2, 0, -1, 0)]
        [InlineData(2, 2, 0, -1, 0, 0)]
        public void ShouldHandleRayMissingCube(float oX, float oY, float oZ, float dX, float dY, float dZ)
        {
            var origin = CreatePoint(oX, oY, oZ);
            var direction = CreateVector(dX, dY, dZ);

            var c = new Cube();
            var r = CreateRay(origin, direction);
            var xs = c.LocalIntersect(r);
            xs.Count.Should().Be(0);
        }

        //Scenario Outline: The normal on the surface of a cube
        //  Given c ← cube()
        //    And p ← <point>
        //  When normal ← local_normal_at(c, p)
        //  Then normal = <normal>

        //  Examples:
        //    | point                | normal           |
        //    | point(1, 0.5, -0.8)  | vector(1, 0, 0)  |
        //    | point(-1, -0.2, 0.9) | vector(-1, 0, 0) |
        //    | point(-0.4, 1, -0.1) | vector(0, 1, 0)  |
        //    | point(0.3, -1, -0.7) | vector(0, -1, 0) |
        //    | point(-0.6, 0.3, 1)  | vector(0, 0, 1)  |
        //    | point(0.4, 0.4, -1)  | vector(0, 0, -1) |
        //    | point(1, 1, 1)       | vector(1, 0, 0)  |
        //    | point(-1, -1, -1)    | vector(-1, 0, 0) |
        [Theory]
        [InlineData(1, 0.5, -0.8, 1, 0, 0)]
        [InlineData(-1, -0.2, 0.9, -1, 0, 0)]
        [InlineData(-0.4, 1, -0.1, 0, 1, 0)]
        [InlineData(0.3, -1, -0.7, 0, -1, 0)]
        [InlineData(-0.6, 0.3, 1, 0, 0, 1)]
        [InlineData(0.4, 0.4, -1, 0, 0, -1)]
        [InlineData(1, 1, 1, 1, 0, 0)]
        [InlineData(-1, -1, -1, -1, 0, 0)]
        public void ShouldComputeSurfaceNormalOfCube(float pX, float pY, float pZ, float nX, float nY, float nZ)
        {
            var point = CreatePoint(pX, pY, pZ);
            var expectedNormal = CreateVector(nX, nY, nZ);

            var c = new Cube();
            var normal = c.GetLocalNormalAt(point);
            AssertActualEqualToExpected(normal, expectedNormal);
        }

    }
}