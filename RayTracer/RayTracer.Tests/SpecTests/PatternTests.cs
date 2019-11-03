using NUnit.Framework;
using RayTracer.Core;
using RayTracer.Core.Patterns;
using static System.MathF;
using static System.Numerics.Matrix4x4;
using static System.Numerics.Vector4;
using static RayTracer.Core.Tuples;
using static RayTracer.Tests.SpecTests.Framework.Comparisons;

namespace RayTracer.Tests.SpecTests
{
    /// <summary>
    /// patterns.feature
    /// </summary>
    [TestFixture]
    public sealed class PatternTests
    {
        //Scenario: Creating a stripe pattern
        //  Given pattern ← stripe_pattern(white, black)
        //  Then pattern.a = white
        //    And pattern.b = black
        [Test]
        public void ShouldCreateStripePattern()
        {
            var pattern = new StripePattern(VColor.White, VColor.Black);
            AssertActualEqualToExpected(pattern.A, VColor.White);
            AssertActualEqualToExpected(pattern.B, VColor.Black);
        }

        //Scenario: A stripe pattern is constant in y
        //  Given pattern ← stripe_pattern(white, black)
        //  Then stripe_at(pattern, point(0, 0, 0)) = white
        //    And stripe_at(pattern, point(0, 1, 0)) = white
        //    And stripe_at(pattern, point(0, 2, 0)) = white
        [Test]
        public void StripePatternShouldBeConstantInY([Values(0, 1, 2)] float y)
        {
            var pattern = new StripePattern(VColor.White, VColor.Black);
            AssertActualEqualToExpected(pattern.GetColorAt(CreatePoint(0, y, 0)), VColor.White);
        }

        //Scenario: A stripe pattern is constant in z
        //  Given pattern ← stripe_pattern(white, black)
        //  Then stripe_at(pattern, point(0, 0, 0)) = white
        //    And stripe_at(pattern, point(0, 0, 1)) = white
        //    And stripe_at(pattern, point(0, 0, 2)) = white
        [Test]
        public void StripePatternShouldBeConstantInZ([Values(0, 1, 2)] float z)
        {
            var pattern = new StripePattern(VColor.White, VColor.Black);
            AssertActualEqualToExpected(pattern.GetColorAt(CreatePoint(0, 0, z)), VColor.White);
        }

        //Scenario: A stripe pattern alternates in x
        //  Given pattern ← stripe_pattern(white, black)
        //  Then stripe_at(pattern, point(0, 0, 0)) = white
        //    And stripe_at(pattern, point(0.9, 0, 0)) = white
        //    And stripe_at(pattern, point(1, 0, 0)) = black
        //    And stripe_at(pattern, point(-0.1, 0, 0)) = black
        //    And stripe_at(pattern, point(-1, 0, 0)) = black
        //    And stripe_at(pattern, point(-1.1, 0, 0)) = white
        [TestCase(0, true)]
        [TestCase(0.9f, true)]
        [TestCase(1, false)]
        [TestCase(1.1f, false)]
        [TestCase(-0.1f, false)]
        [TestCase(-1, false)]
        [TestCase(-1.1f, true)]
        public void StripePatternShouldAlternateInX(float x, bool shouldBeWhite)
        {
            var pattern = new StripePattern(VColor.White, VColor.Black);
            AssertActualEqualToExpected(
                pattern.GetColorAt(CreatePoint(x, 0, 0)),
                shouldBeWhite ? VColor.White : VColor.Black);
        }

        //Scenario: Stripes with an object transformation
        //  Given object ← sphere()
        //    And set_transform(object, scaling(2, 2, 2))
        //    And pattern ← stripe_pattern(white, black)
        //  When c ← stripe_at_object(pattern, object, point(1.5, 0, 0))
        //  Then c = white

        //Scenario: Stripes with a pattern transformation
        //  Given object ← sphere()
        //    And pattern ← stripe_pattern(white, black)
        //    And set_pattern_transform(pattern, scaling(2, 2, 2))
        //  When c ← stripe_at_object(pattern, object, point(1.5, 0, 0))
        //  Then c = white

        //Scenario: Stripes with both an object and a pattern transformation
        //  Given object ← sphere()
        //    And set_transform(object, scaling(2, 2, 2))
        //    And pattern ← stripe_pattern(white, black)
        //    And set_pattern_transform(pattern, translation(0.5, 0, 0))
        //  When c ← stripe_at_object(pattern, object, point(2.5, 0, 0))
        //  Then c = white

        //Scenario: The default pattern transformation
        //  Given pattern ← test_pattern()
        //  Then pattern.transform = identity_matrix

        //Scenario: Assigning a transformation
        //  Given pattern ← test_pattern()
        //  When set_pattern_transform(pattern, translation(1, 2, 3))
        //  Then pattern.transform = translation(1, 2, 3)

        //Scenario: A pattern with an object transformation
        //  Given shape ← sphere()
        //    And set_transform(shape, scaling(2, 2, 2))
        //    And pattern ← test_pattern()
        //  When c ← pattern_at_shape(pattern, shape, point(2, 3, 4))
        //  Then c = color(1, 1.5, 2)

        //Scenario: A pattern with a pattern transformation
        //  Given shape ← sphere()
        //    And pattern ← test_pattern()
        //    And set_pattern_transform(pattern, scaling(2, 2, 2))
        //  When c ← pattern_at_shape(pattern, shape, point(2, 3, 4))
        //  Then c = color(1, 1.5, 2)

        //Scenario: A pattern with both an object and a pattern transformation
        //  Given shape ← sphere()
        //    And set_transform(shape, scaling(2, 2, 2))
        //    And pattern ← test_pattern()
        //    And set_pattern_transform(pattern, translation(0.5, 1, 1.5))
        //  When c ← pattern_at_shape(pattern, shape, point(2.5, 3, 3.5))
        //  Then c = color(0.75, 0.5, 0.25)

        //Scenario: A gradient linearly interpolates between colors
        //  Given pattern ← gradient_pattern(white, black)
        //  Then pattern_at(pattern, point(0, 0, 0)) = white
        //    And pattern_at(pattern, point(0.25, 0, 0)) = color(0.75, 0.75, 0.75)
        //    And pattern_at(pattern, point(0.5, 0, 0)) = color(0.5, 0.5, 0.5)
        //    And pattern_at(pattern, point(0.75, 0, 0)) = color(0.25, 0.25, 0.25)

        //Scenario: A ring should extend in both x and z
        //  Given pattern ← ring_pattern(white, black)
        //  Then pattern_at(pattern, point(0, 0, 0)) = white
        //    And pattern_at(pattern, point(1, 0, 0)) = black
        //    And pattern_at(pattern, point(0, 0, 1)) = black
        //    # 0.708 = just slightly more than √2/2
        //    And pattern_at(pattern, point(0.708, 0, 0.708)) = black

        //Scenario: Checkers should repeat in x
        //  Given pattern ← checkers_pattern(white, black)
        //  Then pattern_at(pattern, point(0, 0, 0)) = white
        //    And pattern_at(pattern, point(0.99, 0, 0)) = white
        //    And pattern_at(pattern, point(1.01, 0, 0)) = black

        //Scenario: Checkers should repeat in y
        //  Given pattern ← checkers_pattern(white, black)
        //  Then pattern_at(pattern, point(0, 0, 0)) = white
        //    And pattern_at(pattern, point(0, 0.99, 0)) = white
        //    And pattern_at(pattern, point(0, 1.01, 0)) = black

        //Scenario: Checkers should repeat in z
        //  Given pattern ← checkers_pattern(white, black)
        //  Then pattern_at(pattern, point(0, 0, 0)) = white
        //    And pattern_at(pattern, point(0, 0, 0.99)) = white
        //    And pattern_at(pattern, point(0, 0, 1.01)) = black
    }
}
