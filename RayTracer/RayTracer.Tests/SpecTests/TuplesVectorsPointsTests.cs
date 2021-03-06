﻿using System.Numerics;
using FluentAssertions;
using Xunit;
using static System.MathF;
using static System.Numerics.Vector4;
using static RayTracer.Core.Tuples;
using static RayTracer.Tests.SpecTests.Framework.Comparisons;

namespace RayTracer.Tests.SpecTests
{
    /// <summary>
    /// tuples.feature
    /// </summary>
        public class TuplesVectorsPointsTests
    {
        //Scenario: A tuple with w=1.0 is a point
        //  Given a ← tuple(4.3, -4.2, 3.1, 1.0)
        //  Then a.x = 4.3
        //    And a.y = -4.2
        //    And a.z = 3.1
        //    And a.w = 1.0
        //    And a is a point
        //    And a is not a vector
        [Fact]
        public void ShouldDetermineIfPoint()
        {
            var a = new Vector4(4.3f, -4.2f, 3.1f, 1.0f);
            a.IsPoint().Should().Be(true);
            a.IsVector().Should().Be(false);
        }

        //Scenario: A tuple with w=0 is a vector
        //  Given a ← tuple(4.3, -4.2, 3.1, 0.0)
        //  Then a.x = 4.3
        //    And a.y = -4.2
        //    And a.z = 3.1
        //    And a.w = 0.0
        //    And a is not a point
        //    And a is a vector
        [Fact]
        public void ShouldDetermineIfVector()
        {
            var a = new Vector4(4.3f, -4.2f, 3.1f, 0.0f);
            a.IsPoint().Should().Be(false);
            a.IsVector().Should().Be(true);
        }

        //Scenario: point() creates tuples with w=1
        //  Given p ← point(4, -4, 3)
        //  Then p = tuple(4, -4, 3, 1)
        [Fact]
        public void ShouldMakePoint() =>
            AssertActualEqualToExpected(CreatePoint(4, -4, 3), new Vector4(4, -4, 3, 1));

        //Scenario: vector() creates tuples with w=0
        //  Given v ← vector(4, -4, 3)
        //  Then v = tuple(4, -4, 3, 0)
        [Fact]
        public void ShouldMakeVector() =>
            AssertActualEqualToExpected(CreateVector(4, -4, 3), new Vector4(4, -4, 3, 0));

        //Scenario: Adding two tuples
        //  Given a1 ← tuple(3, -2, 5, 1)
        //    And a2 ← tuple(-2, 3, 1, 0)
        //   Then a1 + a2 = tuple(1, 1, 6, 1)
        [Fact]
        public void ShouldAddTuples()
        {
            var a1 = new Vector4(3, -2, 5, 1);
            var a2 = new Vector4(-2, 3, 1, 0);

            AssertActualEqualToExpected(a1 + a2, new Vector4(1, 1, 6, 1));
        }

        //Scenario: Subtracting two points
        //  Given p1 ← point(3, 2, 1)
        //    And p2 ← point(5, 6, 7)
        //  Then p1 - p2 = vector(-2, -4, -6)
        [Fact]
        public void ShouldSubtractPoints()
        {
            var p1 = CreatePoint(3, 2, 1);
            var p2 = CreatePoint(5, 6, 7);

            AssertActualEqualToExpected(p1 - p2, CreateVector(-2, -4, -6));
        }

        //Scenario: Subtracting a vector from a point
        //  Given p ← point(3, 2, 1)
        //    And v ← vector(5, 6, 7)
        //  Then p - v = point(-2, -4, -6)
        [Fact]
        public void ShouldSubtractVectorFromPoint()
        {
            var p = CreatePoint(3, 2, 1);
            var v = CreateVector(5, 6, 7);

            AssertActualEqualToExpected(p - v, CreatePoint(-2, -4, -6));
        }

        //Scenario: Subtracting two vectors
        //  Given v1 ← vector(3, 2, 1)
        //    And v2 ← vector(5, 6, 7)
        //  Then v1 - v2 = vector(-2, -4, -6)
        [Fact]
        public void ShouldSubtractTwoVectors()
        {
            var v1 = CreateVector(3, 2, 1);
            var v2 = CreateVector(5, 6, 7);

            AssertActualEqualToExpected(v1 - v2, CreateVector(-2, -4, -6));
        }

        //Scenario: Subtracting a vector from the zero vector
        //  Given zero ← vector(0, 0, 0)
        //    And v ← vector(1, -2, 3)
        //  Then zero - v = vector(-1, 2, -3)
        [Fact]
        public void ShouldSubtractVectorFromZero() =>
            AssertActualEqualToExpected(Vector4.Zero - CreateVector(1, -2, 3), CreateVector(-1, 2, -3));

        //Scenario: Negating a tuple
        //  Given a ← tuple(1, -2, 3, -4)
        //  Then -a = tuple(-1, 2, -3, 4)
        [Fact]
        public void ShouldNegateTuple()
        {
            var a = new Vector4(1, -2, 3, -4);
            AssertActualEqualToExpected(-a, new Vector4(-1, 2, -3, 4));
        }

        //Scenario: Multiplying a tuple by a scalar
        //  Given a ← tuple(1, -2, 3, -4)
        //  Then a * 3.5 = tuple(3.5, -7, 10.5, -14)
        [Fact]
        public void ShouldMultiplyTupleByScalar()
        {
            var a = new Vector4(1, -2, 3, -4);
            AssertActualEqualToExpected(a * 3.5f, new Vector4(3.5f, -7, 10.5f, -14));
        }

        //Scenario: Multiplying a tuple by a fraction
        //  Given a ← tuple(1, -2, 3, -4)
        //  Then a * 0.5 = tuple(0.5, -1, 1.5, -2)
        [Fact]
        public void ShouldMultiplyTupleByFraction()
        {
            var a = new Vector4(1, -2, 3, -4);
            AssertActualEqualToExpected(a * 0.5f, new Vector4(0.5f, -1, 1.5f, -2));
        }

        //Scenario: Dividing a tuple by a scalar
        //  Given a ← tuple(1, -2, 3, -4)
        //  Then a / 2 = tuple(0.5, -1, 1.5, -2)
        [Fact]
        public void ShouldDivideTupleByScalar()
        {
            var a = new Vector4(1, -2, 3, -4);
            AssertActualEqualToExpected(a / 2, new Vector4(0.5f, -1, 1.5f, -2));
        }

        //Scenario: Computing the magnitude of vector(1, 0, 0)
        //  Given v ← vector(1, 0, 0)
        //  Then magnitude(v) = 1
        //
        //Scenario: Computing the magnitude of vector(0, 1, 0)
        //  Given v ← vector(0, 1, 0)
        //  Then magnitude(v) = 1
        //
        //Scenario: Computing the magnitude of vector(0, 0, 1)
        //  Given v ← vector(0, 0, 1)
        //  Then magnitude(v) = 1
        [Theory]
        [InlineData(1, 0, 0)]
        [InlineData(0, 1, 0)]
        [InlineData(0, 0, 1)]
        public void ShouldComputeMagnitudeOfUnitVector(float x, float y, float z)
        {
            var v = CreateVector(x, y, z);
            v.Length().Should().Be(1);
        }

        //Scenario: Computing the magnitude of vector(-1, -2, -3)
        //  Given v ← vector(-1, -2, -3)
        //  Then magnitude(v) = √14
        [Fact]
        public void ShouldComputeMagnitudeOfVector123()
        {
            var v = CreateVector(1, 2, 3);
            v.Length().Should().BeApproximately(Sqrt(14), Tolerance);
        }

        //Scenario: Computing the magnitude of vector(-1, -2, -3)
        //  Given v ← vector(-1, -2, -3)
        //  Then magnitude(v) = √14
        [Fact]
        public void ShouldComputeMagnitudeOfVectorNegative123()
        {
            var v = CreateVector(-1, -2, -3);
            v.Length().Should().BeApproximately(Sqrt(14), Tolerance);
        }

        //Scenario: Normalizing vector(4, 0, 0) gives (1, 0, 0)
        //  Given v ← vector(4, 0, 0)
        //  Then normalize(v) = vector(1, 0, 0)
        [Fact]
        public void ShouldNormalizeVector400()
        {
            var v = CreateVector(4, 0, 0);
            AssertActualEqualToExpected(Normalize(v), CreateVector(1, 0, 0));
        }

        //Scenario: Normalizing vector(1, 2, 3)
        //  Given v ← vector(1, 2, 3)
        //                                  # vector(1/√14,   2/√14,   3/√14)
        //  Then normalize(v) = approximately vector(0.26726, 0.53452, 0.80178)
        [Fact]
        public void ShouldNormalizeVector123()
        {
            var v = CreateVector(1, 2, 3);
            AssertActualEqualToExpected(Normalize(v), CreateVector(0.26726f, 0.53452f, 0.80178f));
        }

        //Scenario: The magnitude of a normalized vector
        //  Given v ← vector(1, 2, 3)
        //  When norm ← normalize(v)
        //  Then magnitude(norm) = 1
        [Fact]
        public void ShouldGetMagnitudeOfNormalizedVector123()
        {
            var v = CreateVector(1, 2, 3);
            Normalize(v).Length().Should().BeApproximately(1, Tolerance);
        }

        //Scenario: The dot product of two tuples
        //  Given a ← vector(1, 2, 3)
        //    And b ← vector(2, 3, 4)
        //  Then dot(a, b) = 20
        [Fact]
        public void ShouldGetDotProductOfTwoTuples()
        {
            var a = CreateVector(1, 2, 3);
            var b = CreateVector(2, 3, 4);
            Dot(a, b).Should().BeApproximately(20, Tolerance);
        }

        //Scenario: The cross product of two vectors
        //  Given a ← vector(1, 2, 3)
        //    And b ← vector(2, 3, 4)
        //  Then cross(a, b) = vector(-1, 2, -1)
        //    And cross(b, a) = vector(1, -2, 1)
        [Fact]
        public void ShouldGetCrossProductOfTwoVectors()
        {
            var a = CreateVector(1, 2, 3);
            var b = CreateVector(2, 3, 4);
            AssertActualEqualToExpected(Cross(ref a, ref b), CreateVector(-1, 2, -1));
            AssertActualEqualToExpected(Cross(ref b, ref a), CreateVector(1, -2, 1));
        }

        //Scenario: Colors are (red, green, blue) tuples
        //  Given c ← color(-0.5, 0.4, 1.7)
        //  Then c.red = -0.5
        //    And c.green = 0.4
        //    And c.blue = 1.7

        //Scenario: Adding colors
        //  Given c1 ← color(0.9, 0.6, 0.75)
        //    And c2 ← color(0.7, 0.1, 0.25)
        //   Then c1 + c2 = color(1.6, 0.7, 1.0)

        //Scenario: Subtracting colors
        //  Given c1 ← color(0.9, 0.6, 0.75)
        //    And c2 ← color(0.7, 0.1, 0.25)
        //   Then c1 - c2 = color(0.2, 0.5, 0.5)

        //Scenario: Multiplying a color by a scalar
        //  Given c ← color(0.2, 0.3, 0.4)
        //  Then c * 2 = color(0.4, 0.6, 0.8)

        //Scenario: Multiplying colors
        //  Given c1 ← color(1, 0.2, 0.4)
        //    And c2 ← color(0.9, 1, 0.1)
        //   Then c1 * c2 = color(0.9, 0.2, 0.04)

        //Scenario: Reflecting a vector approaching at 45°
        //  Given v ← vector(1, -1, 0)
        //    And n ← vector(0, 1, 0)
        //  When r ← reflect(v, n)
        //  Then r = vector(1, 1, 0)
        [Fact]
        public void ShouldReflectVectorAt45Degrees()
        {
            var v = CreateVector(1, -1, 0);
            var n = CreateVector(0, 1, 0);
            var r = Reflect(v, n);
            AssertActualEqualToExpected(r, CreateVector(1, 1, 0));
        }

        //Scenario: Reflecting a vector off a slanted surface
        //  Given v ← vector(0, -1, 0)
        //    And n ← vector(√2/2, √2/2, 0)
        //  When r ← reflect(v, n)
        //  Then r = vector(1, 0, 0)
        [Fact]
        public void ShouldReflectVectorOffSlantedSurface()
        {
            var v = CreateVector(0, -1, 0);
            var n = CreateVector(Sqrt(2) / 2, Sqrt(2) / 2, 0);
            var r = Reflect(v, n);
            AssertActualEqualToExpected(r, CreateVector(1, 0, 0));
        }
    }
}
