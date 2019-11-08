using System;
using System.Numerics;
using Xunit;
using static System.MathF;
using static System.Numerics.Matrix4x4;
using static System.Numerics.Vector4;
using static RayTracer.Core.Tuples;
using static RayTracer.Tests.SpecTests.Framework.Comparisons;

namespace RayTracer.Tests.SpecTests
{
    /// <summary>
    /// transformations.feature
    /// </summary>
        public class TransformationTests
    {
        //Scenario: Multiplying by a translation matrix
        //  Given transform ← translation(5, -3, 2)
        //    And p ← point(-3, 4, 5)
        //   Then transform * p = point(2, 1, 7)
        [Fact]
        public void ShouldTranslatePoint()
        {
            var translation = CreateTranslation(5, -3, 2);
            var p = CreatePoint(-3, 4, 5);
            AssertActualEqualToExpected(Transform(p, translation), CreatePoint(2, 1, 7));
        }

        //Scenario: Multiplying by the inverse of a translation matrix
        //  Given transform ← translation(5, -3, 2)
        //    And inv ← inverse(transform)
        //    And p ← point(-3, 4, 5)
        //   Then inv * p = point(-8, 7, 3)
        [Fact]
        public void ShouldTranslateByInverseMatrix()
        {
            var translation = CreateTranslation(5, -3, 2);
            Invert(translation, out Matrix4x4 inv);
            var p = CreatePoint(-3, 4, 5);
            AssertActualEqualToExpected(Transform(p, inv), CreatePoint(-8, 7, 3));
        }

        //Scenario: Translation does not affect vectors
        //  Given transform ← translation(5, -3, 2)
        //    And v ← vector(-3, 4, 5)
        //   Then transform * v = v
        [Fact]
        public void ShouldNotTranslateVector()
        {
            var translation = CreateTranslation(5, -3, 2);
            var v = CreateVector(-3, 4, 5);
            AssertActualEqualToExpected(Transform(v, translation), v);
        }

        //Scenario: A scaling matrix applied to a point
        //  Given transform ← scaling(2, 3, 4)
        //    And p ← point(-4, 6, 8)
        //   Then transform * p = point(-8, 18, 32)
        [Fact]
        public void ShouldScaleAPoint()
        {
            var scaling = CreateScale(2, 3, 4);
            var p = CreatePoint(-4, 6, 8);
            AssertActualEqualToExpected(Transform(p, scaling), CreatePoint(-8, 18, 32));
        }

        //Scenario: A scaling matrix applied to a vector
        //  Given transform ← scaling(2, 3, 4)
        //    And v ← vector(-4, 6, 8)
        //   Then transform * v = vector(-8, 18, 32)
        [Fact]
        public void ShouldScaleAVector()
        {
            var scaling = CreateScale(2, 3, 4);
            var v = CreateVector(-4, 6, 8);
            AssertActualEqualToExpected(Transform(v, scaling), CreateVector(-8, 18, 32));
        }

        //Scenario: Multiplying by the inverse of a scaling matrix
        //  Given transform ← scaling(2, 3, 4)
        //    And inv ← inverse(transform)
        //    And v ← vector(-4, 6, 8)
        //   Then inv * v = vector(-2, 2, 2)
        [Fact]
        public void ShouldScaleVectorByInverse()
        {
            var scaling = CreateScale(2, 3, 4);
            Invert(scaling, out Matrix4x4 inv);
            var v = CreateVector(-4, 6, 8);
            AssertActualEqualToExpected(Transform(v, inv), CreateVector(-2, 2, 2));
        }

        //Scenario: Reflection is scaling by a negative value
        //  Given transform ← scaling(-1, 1, 1)
        //    And p ← point(2, 3, 4)
        //   Then transform * p = point(-2, 3, 4)
        [Fact]
        public void ShouldReflectPoint()
        {
            var scaling = CreateScale(-1, 1, 1);
            var p = CreatePoint(2, 3, 4);
            AssertActualEqualToExpected(Transform(p, scaling), CreatePoint(-2, 3, 4));
        }

        //Scenario: Rotating a point around the x axis
        //  Given p ← point(0, 1, 0)
        //    And half_quarter ← rotation_x(π / 4)
        //    And full_quarter ← rotation_x(π / 2)
        //  Then half_quarter * p = point(0, √2/2, √2/2)
        //    And full_quarter * p = point(0, 0, 1)
        [Fact]
        public void ShouldRotatePointAroundXAxis()
        {
            var p = CreatePoint(0, 1, 0);
            var halfQuarter = CreateRotationX(PI / 4);
            var fullQuarter = CreateRotationX(PI / 2);
            AssertActualEqualToExpected(Transform(p, halfQuarter), CreatePoint(0, Sqrt(2) / 2, Sqrt(2) / 2));
            AssertActualEqualToExpected(Transform(p, fullQuarter), CreatePoint(0, 0, 1));
        }

        //Scenario: The inverse of an x-rotation rotates in the opposite direction
        //  Given p ← point(0, 1, 0)
        //    And half_quarter ← rotation_x(π / 4)
        //    And inv ← inverse(half_quarter)
        //  Then inv * p = point(0, √2/2, -√2/2)
        [Fact]
        public void ShouldRotatePointAroundInverseOfXAxis()
        {
            var p = CreatePoint(0, 1, 0);
            var halfQuarter = CreateRotationX(PI / 4);
            Invert(halfQuarter, out var inv);
            AssertActualEqualToExpected(Transform(p, inv), CreatePoint(0, Sqrt(2) / 2, -Sqrt(2) / 2));
        }

        //Scenario: Rotating a point around the y axis
        //  Given p ← point(0, 0, 1)
        //    And half_quarter ← rotation_y(π / 4)
        //    And full_quarter ← rotation_y(π / 2)
        //  Then half_quarter * p = point(√2/2, 0, √2/2)
        //    And full_quarter * p = point(1, 0, 0)
        [Fact]
        public void ShouldRotatePointAroundYAxis()
        {
            var p = CreatePoint(0, 0, 1);
            var halfQuarter = CreateRotationY(PI / 4);
            var fullQuarter = CreateRotationY(PI / 2);
            AssertActualEqualToExpected(Transform(p, halfQuarter), CreatePoint(Sqrt(2) / 2, 0, Sqrt(2) / 2));
            AssertActualEqualToExpected(Transform(p, fullQuarter), CreatePoint(1, 0, 0));
        }

        //Scenario: Rotating a point around the z axis
        //  Given p ← point(0, 1, 0)
        //    And half_quarter ← rotation_z(π / 4)
        //    And full_quarter ← rotation_z(π / 2)
        //  Then half_quarter * p = point(-√2/2, √2/2, 0)
        //    And full_quarter * p = point(-1, 0, 0)
        [Fact]
        public void ShouldRotatePointAroundZAxis()
        {
            var p = CreatePoint(0, 1, 0);
            var halfQuarter = CreateRotationZ(PI / 4);
            var fullQuarter = CreateRotationZ(PI / 2);
            AssertActualEqualToExpected(Transform(p, halfQuarter), CreatePoint(-Sqrt(2) / 2, Sqrt(2) / 2, 0));
            AssertActualEqualToExpected(Transform(p, fullQuarter), CreatePoint(-1, 0, 0));
        }

        //Scenario: A shearing transformation moves x in proportion to y
        //  Given transform ← shearing(1, 0, 0, 0, 0, 0)
        //    And p ← point(2, 3, 4)
        //  Then transform * p = point(5, 3, 4)
        [Fact]
        public void ShouldShearXInProportionToY()
        {
            var transform = CreateShear(1, 0, 0, 0, 0, 0);
            var p = CreatePoint(2, 3, 4);
            AssertActualEqualToExpected(Transform(p, transform), CreatePoint(5, 3, 4));
        }

        //Scenario: A shearing transformation moves x in proportion to z
        //  Given transform ← shearing(0, 1, 0, 0, 0, 0)
        //    And p ← point(2, 3, 4)
        //  Then transform * p = point(6, 3, 4)
        [Fact]
        public void ShouldShearXInProportionToZ()
        {
            var transform = CreateShear(0, 1, 0, 0, 0, 0);
            var p = CreatePoint(2, 3, 4);
            AssertActualEqualToExpected(Transform(p, transform), CreatePoint(6, 3, 4));
        }

        //Scenario: A shearing transformation moves y in proportion to x
        //  Given transform ← shearing(0, 0, 1, 0, 0, 0)
        //    And p ← point(2, 3, 4)
        //  Then transform * p = point(2, 5, 4)
        [Fact]
        public void ShouldShearYInProportionToX()
        {
            var transform = CreateShear(0, 0, 1, 0, 0, 0);
            var p = CreatePoint(2, 3, 4);
            AssertActualEqualToExpected(Transform(p, transform), CreatePoint(2, 5, 4));
        }

        //Scenario: A shearing transformation moves y in proportion to z
        //  Given transform ← shearing(0, 0, 0, 1, 0, 0)
        //    And p ← point(2, 3, 4)
        //  Then transform * p = point(2, 7, 4)
        [Fact]
        public void ShouldShearYInProportionToZ()
        {
            var transform = CreateShear(0, 0, 0, 1, 0, 0);
            var p = CreatePoint(2, 3, 4);
            AssertActualEqualToExpected(Transform(p, transform), CreatePoint(2, 7, 4));
        }

        //Scenario: A shearing transformation moves z in proportion to x
        //  Given transform ← shearing(0, 0, 0, 0, 1, 0)
        //    And p ← point(2, 3, 4)
        //  Then transform * p = point(2, 3, 6)
        [Fact]
        public void ShouldShearZInProportionToX()
        {
            var transform = CreateShear(0, 0, 0, 0, 1, 0);
            var p = CreatePoint(2, 3, 4);
            AssertActualEqualToExpected(Transform(p, transform), CreatePoint(2, 3, 6));
        }

        //Scenario: A shearing transformation moves z in proportion to y
        //  Given transform ← shearing(0, 0, 0, 0, 0, 1)
        //    And p ← point(2, 3, 4)
        //  Then transform * p = point(2, 3, 7)
        [Fact]
        public void ShouldShearZInProportionToY()
        {
            var transform = CreateShear(0, 0, 0, 0, 0, 1);
            var p = CreatePoint(2, 3, 4);
            AssertActualEqualToExpected(Transform(p, transform), CreatePoint(2, 3, 7));
        }

        //Scenario: Individual transformations are applied in sequence
        //  Given p ← point(1, 0, 1)
        //    And A ← rotation_x(π / 2)
        //    And B ← scaling(5, 5, 5)
        //    And C ← translation(10, 5, 7)
        //  # apply rotation first
        //  When p2 ← A * p
        //  Then p2 = point(1, -1, 0)
        //  # then apply scaling
        //  When p3 ← B * p2
        //  Then p3 = point(5, -5, 0)
        //  # then apply translation
        //  When p4 ← C * p3
        //  Then p4 = point(15, 0, 7)
        [Fact]
        public void ShouldApplyTransformationsInSequence()
        {
            var p = CreatePoint(1, 0, 1);
            var a = CreateRotationX(PI / 2);
            var b = CreateScale(5, 5, 5);
            var c = CreateTranslation(10, 5, 7);

            var p2 = Transform(p, a);
            AssertActualEqualToExpected(p2, CreatePoint(1, -1, 0));

            var p3 = Transform(p2, b);
            AssertActualEqualToExpected(p3, CreatePoint(5, -5, 0));

            var p4 = Transform(p3, c);
            AssertActualEqualToExpected(p4, CreatePoint(15, 0, 7));
        }

        //Scenario: Chained transformations must be applied in reverse order
        //  Given p ← point(1, 0, 1)
        //    And A ← rotation_x(π / 2)
        //    And B ← scaling(5, 5, 5)
        //    And C ← translation(10, 5, 7)
        //  When T ← C * B * A
        //  Then T * p = point(15, 0, 7)
        [Fact]
        public void ShouldApplyChainedTransformation()
        {
            var p = CreatePoint(1, 0, 1);
            var a = CreateRotationX(PI / 2);
            var b = CreateScale(5, 5, 5);
            var c = CreateTranslation(10, 5, 7);

            var t = a * b * c;

            AssertActualEqualToExpected(Transform(p, t), CreatePoint(15, 0, 7));
        }

        //Scenario: The transformation matrix for the default orientation
        //  Given from ← point(0, 0, 0)
        //    And to ← point(0, 0, -1)
        //    And up ← vector(0, 1, 0)
        //  When t ← view_transform(from, to, up)
        //  Then t = identity_matrix
        [Fact]
        public void ShouldMakeViewMatrixForDefaultOrientation()
        {
            var from = CreatePoint(0, 0, 0);
            var to = CreatePoint(0, 0, -1);
            var up = CreateVector(0, 1, 0);
            var t = CreateViewTransform(from, to, up);
            AssertActualEqualToExpected(t, Identity);
        }

        //Scenario: A view transformation matrix looking in positive z direction
        //  Given from ← point(0, 0, 0)
        //    And to ← point(0, 0, 1)
        //    And up ← vector(0, 1, 0)
        //  When t ← view_transform(from, to, up)
        //  Then t = scaling(-1, 1, -1)
        [Fact]
        public void ShouldMakeViewMatrixLookingInPositiveZDirection()
        {
            var from = CreatePoint(0, 0, 0);
            var to = CreatePoint(0, 0, 1);
            var up = CreateVector(0, 1, 0);
            var t = CreateViewTransform(from, to, up);
            AssertActualEqualToExpected(t, CreateScale(-1, 1, -1));
        }

        //Scenario: The view transformation moves the world
        //  Given from ← point(0, 0, 8)
        //    And to ← point(0, 0, 0)
        //    And up ← vector(0, 1, 0)
        //  When t ← view_transform(from, to, up)
        //  Then t = translation(0, 0, -8)
        [Fact]
        public void ShouldMoveTheWorldWithViewTransformation()
        {
            var from = CreatePoint(0, 0, 8);
            var to = CreatePoint(0, 0, 0);
            var up = CreateVector(0, 1, 0);
            var t = CreateViewTransform(from, to, up);
            AssertActualEqualToExpected(t, CreateTranslation(0, 0, -8));
        }

        //Scenario: An arbitrary view transformation
        //  Given from ← point(1, 3, 2)
        //    And to ← point(4, -2, 8)
        //    And up ← vector(1, 1, 0)
        //  When t ← view_transform(from, to, up)
        //  Then t is the following 4x4 matrix:
        //      | -0.50709 | 0.50709 |  0.67612 | -2.36643 |
        //      |  0.76772 | 0.60609 |  0.12122 | -2.82843 |
        //      | -0.35857 | 0.59761 | -0.71714 |  0.00000 |
        //      |  0.00000 | 0.00000 |  0.00000 |  1.00000 |
        [Fact]
        public void ShouldCreateArbitraryViewTransformation()
        {
            var from = CreatePoint(1, 3, 2);
            var to = CreatePoint(4, -2, 8);
            var up = CreateVector(1, 1, 0);
            var t = CreateViewTransform(from, to, up);

            var expected = Transpose(new Matrix4x4(
                -0.50709f, 0.50709f, 0.67612f, -2.36643f,
                0.76772f, 0.60609f, 0.12122f, -2.82843f,
                -0.35857f, 0.59761f, -0.71714f, 0.00000f,
                0.00000f, 0.00000f, 0.00000f, 1.00000f));

            AssertActualEqualToExpected(t, expected);
        }

    }
}
