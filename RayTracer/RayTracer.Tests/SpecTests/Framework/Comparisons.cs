using System;
using System.Numerics;
using RayTracer.Core;
using FluentAssertions;
using static RayTracer.Core.Tuples;

namespace RayTracer.Tests.SpecTests.Framework
{
    static class Comparisons
    {
        public static void AssertActualEqualToExpected(Vector4 actual, Vector4 expected)
        {
            actual.X.Should().BeApproximately(expected.X, Tolerance);
            actual.Y.Should().BeApproximately(expected.Y, Tolerance);
            actual.Z.Should().BeApproximately(expected.Z, Tolerance);
            actual.W.Should().BeApproximately(expected.W, Tolerance);
        }

        public static void AssertActualEqualToExpected(Matrix4x4 actual, Matrix4x4 expected)
        {
            actual.M11.Should().BeApproximately(expected.M11, Tolerance);
            actual.M12.Should().BeApproximately(expected.M12, Tolerance);
            actual.M13.Should().BeApproximately(expected.M13, Tolerance);
            actual.M14.Should().BeApproximately(expected.M14, Tolerance);

            actual.M21.Should().BeApproximately(expected.M21, Tolerance);
            actual.M22.Should().BeApproximately(expected.M22, Tolerance);
            actual.M23.Should().BeApproximately(expected.M23, Tolerance);
            actual.M24.Should().BeApproximately(expected.M24, Tolerance);

            actual.M31.Should().BeApproximately(expected.M31, Tolerance);
            actual.M32.Should().BeApproximately(expected.M32, Tolerance);
            actual.M33.Should().BeApproximately(expected.M33, Tolerance);
            actual.M34.Should().BeApproximately(expected.M34, Tolerance);

            actual.M41.Should().BeApproximately(expected.M41, Tolerance);
            actual.M42.Should().BeApproximately(expected.M42, Tolerance);
            actual.M43.Should().BeApproximately(expected.M43, Tolerance);
            actual.M44.Should().BeApproximately(expected.M44, Tolerance);
        }

        public static void AssertActualEqualToExpected(Material actual, Material expected)
        {
            AssertActualEqualToExpected(actual.Color, expected.Color);
            actual.Ambient.Should().BeApproximately(expected.Ambient, Tolerance);
            actual.Diffuse.Should().BeApproximately(expected.Diffuse, Tolerance);
            actual.Specular.Should().BeApproximately(expected.Specular, Tolerance);
            actual.Shininess.Should().BeApproximately(expected.Shininess, Tolerance);
        }
    }
}
