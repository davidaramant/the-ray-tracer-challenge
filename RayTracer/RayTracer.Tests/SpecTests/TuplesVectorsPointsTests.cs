using System.Numerics;
using NUnit.Framework;
using static System.Math;
using static System.Numerics.Vector4;
using static RayTracer.Tuples;
using static RayTracer.Tests.SpecTests.Framework.Comparisons;

namespace RayTracer.Tests.SpecTests
{
    /// <summary>
    /// tuples.feature
    /// </summary>
    [TestFixture]
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
        [Test]
        public void ShouldDetermineIfPoint()
        {
            var a = new Vector4(4.3f, -4.2f, 3.1f, 1.0f);
            Assert.That(a.IsPoint(), Is.True);
            Assert.That(a.IsVector(), Is.False);
        }

        //Scenario: A tuple with w=0 is a vector
        //  Given a ← tuple(4.3, -4.2, 3.1, 0.0)
        //  Then a.x = 4.3
        //    And a.y = -4.2
        //    And a.z = 3.1
        //    And a.w = 0.0
        //    And a is not a point
        //    And a is a vector
        [Test]
        public void ShouldDetermineIfVector()
        {
            var a = new Vector4(4.3f, -4.2f, 3.1f, 0.0f);
            Assert.That(a.IsPoint(), Is.False);
            Assert.That(a.IsVector(), Is.True);
        }

        //Scenario: point() creates tuples with w=1
        //  Given p ← point(4, -4, 3)
        //  Then p = tuple(4, -4, 3, 1)
        [Test]
        public void ShouldMakePoint() =>
            AssertActualEqualToExpected(MakePoint(4, -4, 3), new Vector4(4, -4, 3, 1));

        //Scenario: vector() creates tuples with w=0
        //  Given v ← vector(4, -4, 3)
        //  Then v = tuple(4, -4, 3, 0)
        [Test]
        public void ShouldMakeVector() =>
            AssertActualEqualToExpected(MakeVector(4, -4, 3), new Vector4(4, -4, 3, 0));

        //Scenario: Adding two tuples
        //  Given a1 ← tuple(3, -2, 5, 1)
        //    And a2 ← tuple(-2, 3, 1, 0)
        //   Then a1 + a2 = tuple(1, 1, 6, 1)
        [Test]
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
        [Test]
        public void ShouldSubtractPoints()
        {
            var p1 = MakePoint(3, 2, 1);
            var p2 = MakePoint(5, 6, 7);

            AssertActualEqualToExpected(p1 - p2, MakeVector(-2, -4, -6));
        }

        //Scenario: Subtracting a vector from a point
        //  Given p ← point(3, 2, 1)
        //    And v ← vector(5, 6, 7)
        //  Then p - v = point(-2, -4, -6)
        [Test]
        public void ShouldSubtractVectorFromPoint()
        {
            var p = MakePoint(3, 2, 1);
            var v = MakeVector(5, 6, 7);

            AssertActualEqualToExpected(p - v, MakePoint(-2, -4, -6));
        }

        //Scenario: Subtracting two vectors
        //  Given v1 ← vector(3, 2, 1)
        //    And v2 ← vector(5, 6, 7)
        //  Then v1 - v2 = vector(-2, -4, -6)
        [Test]
        public void ShouldSubtractTwoVectors()
        {
            var v1 = MakeVector(3, 2, 1);
            var v2 = MakeVector(5, 6, 7);

            AssertActualEqualToExpected(v1 - v2, MakeVector(-2, -4, -6));
        }

        //Scenario: Subtracting a vector from the zero vector
        //  Given zero ← vector(0, 0, 0)
        //    And v ← vector(1, -2, 3)
        //  Then zero - v = vector(-1, 2, -3)
        [Test]
        public void ShouldSubtractVectorFromZero() =>
            AssertActualEqualToExpected(Vector4.Zero - MakeVector(1, -2, 3), MakeVector(-1, 2, -3));

        //Scenario: Negating a tuple
        //  Given a ← tuple(1, -2, 3, -4)
        //  Then -a = tuple(-1, 2, -3, 4)
        [Test]
        public void ShouldNegateTuple()
        {
            var a = new Vector4(1, -2, 3, -4);
            AssertActualEqualToExpected(-a, new Vector4(-1, 2, -3, 4));
        }

        //Scenario: Multiplying a tuple by a scalar
        //  Given a ← tuple(1, -2, 3, -4)
        //  Then a * 3.5 = tuple(3.5, -7, 10.5, -14)
        [Test]
        public void ShouldMultiplyTupleByScalar()
        {
            var a = new Vector4(1, -2, 3, -4);
            AssertActualEqualToExpected(a * 3.5f, new Vector4(3.5f, -7, 10.5f, -14));
        }

        //Scenario: Multiplying a tuple by a fraction
        //  Given a ← tuple(1, -2, 3, -4)
        //  Then a * 0.5 = tuple(0.5, -1, 1.5, -2)
        [Test]
        public void ShouldMultiplyTupleByFraction()
        {
            var a = new Vector4(1, -2, 3, -4);
            AssertActualEqualToExpected(a * 0.5f, new Vector4(0.5f, -1, 1.5f, -2));
        }

        //Scenario: Dividing a tuple by a scalar
        //  Given a ← tuple(1, -2, 3, -4)
        //  Then a / 2 = tuple(0.5, -1, 1.5, -2)
        [Test]
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
        [TestCase(1, 0, 0)]
        [TestCase(0, 1, 0)]
        [TestCase(0, 0, 1)]
        public void ShouldComputeMagnitudeOfUnitVector(float x, float y, float z)
        {
            var v = MakeVector(x, y, z);
            Assert.That(v.Length(), Is.EqualTo(1));
        }

        //Scenario: Computing the magnitude of vector(-1, -2, -3)
        //  Given v ← vector(-1, -2, -3)
        //  Then magnitude(v) = √14
        [Test]
        public void ShouldComputeMagnitudeOfVector123()
        {
            var v = MakeVector(1, 2, 3);
            Assert.That(v.Length(), Is.EqualTo(Sqrt(14)).Within(Tolerance));
        }

        //Scenario: Computing the magnitude of vector(-1, -2, -3)
        //  Given v ← vector(-1, -2, -3)
        //  Then magnitude(v) = √14
        [Test]
        public void ShouldComputeMagnitudeOfVectorNegative123()
        {
            var v = MakeVector(-1, -2, -3);
            Assert.That(v.Length(), Is.EqualTo(Sqrt(14)).Within(Tolerance));
        }

        //Scenario: Normalizing vector(4, 0, 0) gives (1, 0, 0)
        //  Given v ← vector(4, 0, 0)
        //  Then normalize(v) = vector(1, 0, 0)
        [Test]
        public void ShouldNormalizeVector400()
        {
            var v = MakeVector(4, 0, 0);
            AssertActualEqualToExpected(Normalize(v), MakeVector(1, 0, 0));
        }

        //Scenario: Normalizing vector(1, 2, 3)
        //  Given v ← vector(1, 2, 3)
        //                                  # vector(1/√14,   2/√14,   3/√14)
        //  Then normalize(v) = approximately vector(0.26726, 0.53452, 0.80178)
        [Test]
        public void ShouldNormalizeVector123()
        {
            var v = MakeVector(1, 2, 3);
            AssertActualEqualToExpected(Normalize(v), MakeVector(0.26726f, 0.53452f, 0.80178f));
        }

        //Scenario: The magnitude of a normalized vector
        //  Given v ← vector(1, 2, 3)
        //  When norm ← normalize(v)
        //  Then magnitude(norm) = 1
        [Test]
        public void ShouldGetMagnitudeOfNormalizedVector123()
        {
            var v = MakeVector(1, 2, 3);
            Assert.That(Normalize(v).Length(), Is.EqualTo(1).Within(Tolerance));
        }

        //Scenario: The dot product of two tuples
        //  Given a ← vector(1, 2, 3)
        //    And b ← vector(2, 3, 4)
        //  Then dot(a, b) = 20
        [Test]
        public void ShouldGetDotProductOfTwoTuples()
        {
            var a = MakeVector(1, 2, 3);
            var b = MakeVector(2, 3, 4);
            Assert.That(Dot(a, b), Is.EqualTo(20).Within(Tolerance));
        }

        //Scenario: The cross product of two vectors
        //  Given a ← vector(1, 2, 3)
        //    And b ← vector(2, 3, 4)
        //  Then cross(a, b) = vector(-1, 2, -1)
        //    And cross(b, a) = vector(1, -2, 1)
        [Test]
        public void ShouldGetCrossProductOfTwoVectors()
        {
            var a = MakeVector(1, 2, 3);
            var b = MakeVector(2, 3, 4);
            AssertActualEqualToExpected(Cross(a, b), MakeVector(-1, 2, -1));
            AssertActualEqualToExpected(Cross(b, a), MakeVector(1, -2, 1));
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

        //Scenario: Reflecting a vector off a slanted surface
        //  Given v ← vector(0, -1, 0)
        //    And n ← vector(√2/2, √2/2, 0)
        //  When r ← reflect(v, n)
        //  Then r = vector(1, 0, 0)
    }
}
