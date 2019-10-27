﻿using NUnit.Framework;
using System.Numerics;
using RayTracer.Core;
using static System.Numerics.Matrix4x4;
using static RayTracer.Core.Tuples;
using static RayTracer.Tests.SpecTests.Framework.Comparisons;

namespace RayTracer.Tests.SpecTests
{
    /// <summary>
    /// matrices.feature
    /// </summary>
    [TestFixture]
    public class MatricesTests
    {
        //Scenario: Constructing and inspecting a 4x4 matrix
        //  Given the following 4x4 matrix M:
        //    |  1   |  2   |  3   |  4   |
        //    |  5.5 |  6.5 |  7.5 |  8.5 |
        //    |  9   | 10   | 11   | 12   |
        //    | 13.5 | 14.5 | 15.5 | 16.5 |
        //  Then M[0,0] = 1
        //    And M[0,3] = 4
        //    And M[1,0] = 5.5
        //    And M[1,2] = 7.5
        //    And M[2,2] = 11
        //    And M[3,0] = 13.5
        //    And M[3,2] = 15.5
        [Test]
        public void ShouldConstruct4x4Matrix()
        {
            var m = new Matrix4x4(
                1, 2, 3, 4,
                5.5f, 6.5f, 7.5f, 8.5f,
                9, 10, 11, 12,
                13.5f, 14.5f, 15.5f, 16.5f);

            Assert.That(m.M11, Is.EqualTo(1));
            Assert.That(m.M14, Is.EqualTo(4));
            Assert.That(m.M21, Is.EqualTo(5.5f));
            Assert.That(m.M23, Is.EqualTo(7.5f));
            Assert.That(m.M33, Is.EqualTo(11f));
            Assert.That(m.M41, Is.EqualTo(13.5f));
            Assert.That(m.M43, Is.EqualTo(15.5f));
        }

        //Scenario: A 2x2 matrix ought to be representable
        //  Given the following 2x2 matrix M:
        //    | -3 |  5 |
        //    |  1 | -2 |
        //  Then M[0,0] = -3
        //    And M[0,1] = 5
        //    And M[1,0] = 1
        //    And M[1,1] = -2
        [Test]
        public void ShouldConstruct2x2Matrix()
        {
            var m = new Matrix2x2(-3, 5, 1, -2);

            Assert.That(m.M11, Is.EqualTo(-3));
            Assert.That(m.M12, Is.EqualTo(5));
            Assert.That(m.M21, Is.EqualTo(1));
            Assert.That(m.M22, Is.EqualTo(-2));
        }

        //Scenario: A 3x3 matrix ought to be representable
        //  Given the following 3x3 matrix M:
        //    | -3 |  5 |  0 |
        //    |  1 | -2 | -7 |
        //    |  0 |  1 |  1 |
        //  Then M[0,0] = -3
        //    And M[1,1] = -2
        //    And M[2,2] = 1
        [Test]
        public void ShouldConstruct3x3Matrix()
        {
            var m = new Matrix3x3(
                -3, 5, 0,
                 1, -2, -7,
                 0, 1, 1);

            Assert.That(m.M11, Is.EqualTo(-3));
            Assert.That(m.M22, Is.EqualTo(-2));
            Assert.That(m.M33, Is.EqualTo(1));
        }

        //Scenario: Matrix equality with identical matrices
        //  Given the following matrix A:
        //      | 1 | 2 | 3 | 4 |
        //      | 5 | 6 | 7 | 8 |
        //      | 9 | 8 | 7 | 6 |
        //      | 5 | 4 | 3 | 2 |
        //    And the following matrix B:
        //      | 1 | 2 | 3 | 4 |
        //      | 5 | 6 | 7 | 8 |
        //      | 9 | 8 | 7 | 6 |
        //      | 5 | 4 | 3 | 2 |
        //  Then A = B
        [Test]
        public void ShouldDetermineWhen4x4MatricesAreEqual()
        {
            var a = new Matrix4x4(
                1, 2, 3, 4,
                5, 6, 7, 8,
                9, 8, 7, 6,
                5, 4, 3, 2);
            var b = new Matrix4x4(
                1, 2, 3, 4,
                5, 6, 7, 8,
                9, 8, 7, 6,
                5, 4, 3, 2);

            Assert.That(a, Is.EqualTo(b));
        }

        //Scenario: Matrix equality with different matrices
        //  Given the following matrix A:
        //      | 1 | 2 | 3 | 4 |
        //      | 5 | 6 | 7 | 8 |
        //      | 9 | 8 | 7 | 6 |
        //      | 5 | 4 | 3 | 2 |
        //    And the following matrix B:
        //      | 2 | 3 | 4 | 5 |
        //      | 6 | 7 | 8 | 9 |
        //      | 8 | 7 | 6 | 5 |
        //      | 4 | 3 | 2 | 1 |
        //  Then A != B
        [Test]
        public void ShouldDetermineWhen4x4MatricesAreNotEqual()
        {
            var a = new Matrix4x4(
                1, 2, 3, 4,
                5, 6, 7, 8,
                9, 8, 7, 6,
                5, 4, 3, 2);
            var b = new Matrix4x4(
                2, 3, 4, 5,
                6, 7, 8, 9,
                8, 7, 6, 5,
                4, 3, 2, 1);

            Assert.That(a, Is.Not.EqualTo(b));
        }

        //Scenario: Multiplying two matrices
        //  Given the following matrix A:
        //      | 1 | 2 | 3 | 4 |
        //      | 5 | 6 | 7 | 8 |
        //      | 9 | 8 | 7 | 6 |
        //      | 5 | 4 | 3 | 2 |
        //    And the following matrix B:
        //      | -2 | 1 | 2 |  3 |
        //      |  3 | 2 | 1 | -1 |
        //      |  4 | 3 | 6 |  5 |
        //      |  1 | 2 | 7 |  8 |
        //  Then A * B is the following 4x4 matrix:
        //      | 20|  22 |  50 |  48 |
        //      | 44|  54 | 114 | 108 |
        //      | 40|  58 | 110 | 102 |
        //      | 16|  26 |  46 |  42 |
        [Test]
        public void ShouldMultiply4x4Matrices()
        {
            var a = new Matrix4x4(
                1, 2, 3, 4,
                5, 6, 7, 8,
                9, 8, 7, 6,
                5, 4, 3, 2);
            var b = new Matrix4x4(
                -2, 1, 2, 3,
                 3, 2, 1, -1,
                 4, 3, 6, 5,
                 1, 2, 7, 8);
            var expected = new Matrix4x4(
                20, 22, 50, 48,
                44, 54, 114, 108,
                40, 58, 110, 102,
                16, 26, 46, 42);

            Assert.That(a * b, Is.EqualTo(expected));
        }

        //Scenario: A matrix multiplied by a tuple
        //  Given the following matrix A:
        //      | 1 | 2 | 3 | 4 |
        //      | 2 | 4 | 4 | 2 |
        //      | 8 | 6 | 4 | 1 |
        //      | 0 | 0 | 0 | 1 |
        //    And b ← tuple(1, 2, 3, 1)
        //  Then A * b = tuple(18, 24, 33, 1)
        [Test]
        public void ShouldMultiplyMatrixByTuple()
        {
            var a = new Matrix4x4(
                1, 2, 3, 4,
                2, 4, 4, 2,
                8, 6, 4, 1,
                0, 0, 0, 1);

            var b = new Vector4(1, 2, 3, 1);
            var expected = new Vector4(18, 24, 33, 1);

            Assert.That(Multiply(ref a, ref b), Is.EqualTo(expected));
        }

        //Scenario: Multiplying a matrix by the identity matrix
        //  Given the following matrix A:
        //    | 0 | 1 |  2 |  4 |
        //    | 1 | 2 |  4 |  8 |
        //    | 2 | 4 |  8 | 16 |
        //    | 4 | 8 | 16 | 32 |
        //  Then A * identity_matrix = A
        [Test]
        public void ShouldMultiplyMatrixByIdentity()
        {
            var a = new Matrix4x4(
                0, 1, 2, 4,
                1, 2, 4, 8,
                2, 4, 8, 16,
                4, 8, 16, 32);

            Assert.That(a * Identity, Is.EqualTo(a));
        }

        //Scenario: Multiplying the identity matrix by a tuple
        //  Given a ← tuple(1, 2, 3, 4)
        //  Then identity_matrix * a = a
        [Test]
        public void ShouldMultiplyIdentityMatrixByTuple()
        {
            var a = new Vector4(1, 2, 3, 4);
            Assert.That(Multiply(Identity, ref a), Is.EqualTo(a));
        }

        //Scenario: Transposing a matrix
        //  Given the following matrix A:
        //    | 0 | 9 | 3 | 0 |
        //    | 9 | 8 | 0 | 8 |
        //    | 1 | 8 | 5 | 3 |
        //    | 0 | 0 | 5 | 8 |
        //  Then transpose(A) is the following matrix:
        //    | 0 | 9 | 1 | 0 |
        //    | 9 | 8 | 8 | 0 |
        //    | 3 | 0 | 5 | 5 |
        //    | 0 | 8 | 3 | 8 |
        [Test]
        public void ShouldTransposeMatrix()
        {
            var a = new Matrix4x4(
                0, 9, 3, 0,
                9, 8, 0, 8,
                1, 8, 5, 3,
                0, 0, 5, 8);
            var expected = new Matrix4x4(
                0, 9, 1, 0,
                9, 8, 8, 0,
                3, 0, 5, 5,
                0, 8, 3, 8);

            Assert.That(Transpose(a), Is.EqualTo(expected));
        }

        //Scenario: Transposing the identity matrix
        //  Given A ← transpose(identity_matrix)
        //  Then A = identity_matrix
        [Test]
        public void ShouldTransposeIdentityMatrix()
        {
            Assert.That(Transpose(Identity), Is.EqualTo(Identity));
        }

        //Scenario: Calculating the determinant of a 2x2 matrix
        //  Given the following 2x2 matrix A:
        //    |  1 | 5 |
        //    | -3 | 2 |
        //  Then determinant(A) = 17


        //Scenario: A submatrix of a 3x3 matrix is a 2x2 matrix
        //  Given the following 3x3 matrix A:
        //    |  1 | 5 |  0 |
        //    | -3 | 2 |  7 |
        //    |  0 | 6 | -3 |
        //  Then submatrix(A, 0, 2) is the following 2x2 matrix:
        //    | -3 | 2 |
        //    |  0 | 6 |

        //Scenario: A submatrix of a 4x4 matrix is a 3x3 matrix
        //  Given the following 4x4 matrix A:
        //    | -6 |  1 |  1 |  6 |
        //    | -8 |  5 |  8 |  6 |
        //    | -1 |  0 |  8 |  2 |
        //    | -7 |  1 | -1 |  1 |
        //  Then submatrix(A, 2, 1) is the following 3x3 matrix:
        //    | -6 |  1 | 6 |
        //    | -8 |  8 | 6 |
        //    | -7 | -1 | 1 |

        //Scenario: Calculating a minor of a 3x3 matrix
        //  Given the following 3x3 matrix A:
        //      |  3 |  5 |  0 |
        //      |  2 | -1 | -7 |
        //      |  6 | -1 |  5 |
        //    And B ← submatrix(A, 1, 0)
        //  Then determinant(B) = 25
        //    And minor(A, 1, 0) = 25

        //Scenario: Calculating a cofactor of a 3x3 matrix
        //  Given the following 3x3 matrix A:
        //      |  3 |  5 |  0 |
        //      |  2 | -1 | -7 |
        //      |  6 | -1 |  5 |
        //  Then minor(A, 0, 0) = -12
        //    And cofactor(A, 0, 0) = -12
        //    And minor(A, 1, 0) = 25
        //    And cofactor(A, 1, 0) = -25

        //Scenario: Calculating the determinant of a 3x3 matrix
        //  Given the following 3x3 matrix A:
        //    |  1 |  2 |  6 |
        //    | -5 |  8 | -4 |
        //    |  2 |  6 |  4 |
        //  Then cofactor(A, 0, 0) = 56
        //    And cofactor(A, 0, 1) = 12
        //    And cofactor(A, 0, 2) = -46
        //    And determinant(A) = -196

        //Scenario: Calculating the determinant of a 4x4 matrix
        //  Given the following 4x4 matrix A:
        //    | -2 | -8 |  3 |  5 |
        //    | -3 |  1 |  7 |  3 |
        //    |  1 |  2 | -9 |  6 |
        //    | -6 |  7 |  7 | -9 |
        //  Then cofactor(A, 0, 0) = 690
        //    And cofactor(A, 0, 1) = 447
        //    And cofactor(A, 0, 2) = 210
        //    And cofactor(A, 0, 3) = 51
        //    And determinant(A) = -4071
        [Test]
        public void ShouldComputeDeterminantOf4x4Matrix()
        {
            var a = new Matrix4x4(
                -2, -8, 3, 5,
                -3, 1, 7, 3,
                1, 2, -9, 6,
                -6, 7, 7, -9);

            Assert.That(a.GetDeterminant(), Is.EqualTo(-4071));
        }

        //Scenario: Testing an invertible matrix for invertibility
        //  Given the following 4x4 matrix A:
        //    |  6 |  4 |  4 |  4 |
        //    |  5 |  5 |  7 |  6 |
        //    |  4 | -9 |  3 | -7 |
        //    |  9 |  1 |  7 | -6 |
        //  Then determinant(A) = -2120
        //    And A is invertible
        [Test]
        public void ShouldDetermineIfMatrixIsInvertible()
        {
            var a = new Matrix4x4(
                6, 4, 4, 4,
                5, 5, 7, 6,
                4, -9, 3, -7,
                9, 1, 7, -6);
            Assert.That(Invert(a, out _), Is.True);
        }

        //Scenario: Testing a noninvertible matrix for invertibility
        //  Given the following 4x4 matrix A:
        //    | -4 |  2 | -2 | -3 |
        //    |  9 |  6 |  2 |  6 |
        //    |  0 | -5 |  1 | -5 |
        //    |  0 |  0 |  0 |  0 |
        //  Then determinant(A) = 0
        //    And A is not invertible
        [Test]
        public void ShouldDetermineIfMatrixIsNotInvertible()
        {
            var a = new Matrix4x4(
                -4, 2, -2, -3,
                9, 6, 2, 6,
                0, -5, 1, -5,
                0, 0, 0, 0);
            Assert.That(Invert(a, out _), Is.False);
        }

        //Scenario: Calculating the inverse of a matrix
        //  Given the following 4x4 matrix A:
        //      | -5 |  2 |  6 | -8 |
        //      |  1 | -5 |  1 |  8 |
        //      |  7 |  7 | -6 | -7 |
        //      |  1 | -3 |  7 |  4 |
        //    And B ← inverse(A)
        //  Then determinant(A) = 532
        //    And cofactor(A, 2, 3) = -160
        //    And B[3,2] = -160/532
        //    And cofactor(A, 3, 2) = 105
        //    And B[2,3] = 105/532
        //    And B is the following 4x4 matrix:
        //      |  0.21805 |  0.45113 |  0.24060 | -0.04511 |
        //      | -0.80827 | -1.45677 | -0.44361 |  0.52068 |
        //      | -0.07895 | -0.22368 | -0.05263 |  0.19737 |
        //      | -0.52256 | -0.81391 | -0.30075 |  0.30639 |
        [Test]
        public void ShouldCalculateInverseMatrix1()
        {
            var a = new Matrix4x4(
                -5, 2, 6, -8,
                1, -5, 1, 8,
                7, 7, -6, -7,
                1, -3, 7, 4);
            Assert.That(Invert(a, out var b), Is.True);

            var expected = new Matrix4x4(
                0.21805f, 0.45113f, 0.24060f, -0.04511f,
                -0.80827f, -1.45677f, -0.44361f, 0.52068f,
                -0.07895f, -0.22368f, -0.05263f, 0.19737f,
                -0.52256f, -0.81391f, -0.30075f, 0.30639f);

            AssertActualEqualToExpected(b, expected);
        }

        //Scenario: Calculating the inverse of another matrix
        //  Given the following 4x4 matrix A:
        //    |  8 | -5 |  9 |  2 |
        //    |  7 |  5 |  6 |  1 |
        //    | -6 |  0 |  9 |  6 |
        //    | -3 |  0 | -9 | -4 |
        //  Then inverse(A) is the following 4x4 matrix:
        //    | -0.15385 | -0.15385 | -0.28205 | -0.53846 |
        //    | -0.07692 |  0.12308 |  0.02564 |  0.03077 |
        //    |  0.35897 |  0.35897 |  0.43590 |  0.92308 |
        //    | -0.69231 | -0.69231 | -0.76923 | -1.92308 |
        [Test]
        public void ShouldCalculateInverseMatrix2()
        {
            var a = new Matrix4x4(
                8, -5, 9, 2,
                7, 5, 6, 1,
                -6, 0, 9, 6,
                -3, 0, -9, -4);
            Assert.That(Invert(a, out var b), Is.True);

            var expected = new Matrix4x4(
                -0.15385f, -0.15385f, -0.28205f, -0.53846f,
                -0.07692f, 0.12308f, 0.02564f, 0.03077f,
                0.35897f, 0.35897f, 0.43590f, 0.92308f,
                -0.69231f, -0.69231f, -0.76923f, -1.92308f);

            AssertActualEqualToExpected(b, expected);
        }

        //Scenario: Calculating the inverse of a third matrix
        //  Given the following 4x4 matrix A:
        //    |  9 |  3 |  0 |  9 |
        //    | -5 | -2 | -6 | -3 |
        //    | -4 |  9 |  6 |  4 |
        //    | -7 |  6 |  6 |  2 |
        //  Then inverse(A) is the following 4x4 matrix:
        //    | -0.04074 | -0.07778 |  0.14444 | -0.22222 |
        //    | -0.07778 |  0.03333 |  0.36667 | -0.33333 |
        //    | -0.02901 | -0.14630 | -0.10926 |  0.12963 |
        //    |  0.17778 |  0.06667 | -0.26667 |  0.33333 |
        [Test]
        public void ShouldCalculateInverseMatrix3()
        {
            var a = new Matrix4x4(
                9, 3, 0, 9,
                -5, -2, -6, -3,
                -4, 9, 6, 4,
                -7, 6, 6, 2);
            Assert.That(Invert(a, out Matrix4x4 b), Is.True);

            var expected = new Matrix4x4(
                -0.04074f, -0.07778f, 0.14444f, -0.22222f,
                -0.07778f, 0.03333f, 0.36667f, -0.33333f,
                -0.02901f, -0.14630f, -0.10926f, 0.12963f,
                0.17778f, 0.06667f, -0.26667f, 0.33333f);

            AssertActualEqualToExpected(b, expected);
        }

        //Scenario: Multiplying a product by its inverse
        //  Given the following 4x4 matrix A:
        //      |  3 | -9 |  7 |  3 |
        //      |  3 | -8 |  2 | -9 |
        //      | -4 |  4 |  4 |  1 |
        //      | -6 |  5 | -1 |  1 |
        //    And the following 4x4 matrix B:
        //      |  8 |  2 |  2 |  2 |
        //      |  3 | -1 |  7 |  0 |
        //      |  7 |  0 |  5 |  4 |
        //      |  6 | -2 |  0 |  5 |
        //    And C ← A * B
        //  Then C * inverse(B) = A
        [Test]
        public void ShouldMultiplyProductMatrixByItsInverse()
        {
            var a = new Matrix4x4(
                3, -9, 7, 3,
                3, -8, 2, -9,
                -4, 4, 4, 1,
                -6, 5, -1, 1);
            var b = new Matrix4x4(
                8, 2, 2, 2,
                3, -1, 7, 0,
                7, 0, 5, 4,
                6, -2, 0, 5);
            var c = a * b;
            Invert(b, out Matrix4x4 inverseB);
            AssertActualEqualToExpected(c * inverseB, a);
        }
    }
}