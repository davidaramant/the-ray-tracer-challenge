using System;
using System.Numerics;
using RayTracer.Core;
using Shouldly;
using static RayTracer.Core.Tuples;

namespace RayTracer.Tests.SpecTests.Framework
{
    static class Comparisons
    {
        public static void AssertActualEqualToExpected(Vector4 actual, Vector4 expected)
        {
            actual.X.ShouldBe(expected.X, Tolerance);
            actual.Y.ShouldBe(expected.Y, Tolerance);
            actual.Z.ShouldBe(expected.Z, Tolerance);
            actual.W.ShouldBe(expected.W, Tolerance);
        }

        public static void AssertActualEqualToExpected(Matrix4x4 actual, Matrix4x4 expected)
        {
            actual.M11.ShouldBe(expected.M11, Tolerance);
            actual.M12.ShouldBe(expected.M12, Tolerance);
            actual.M13.ShouldBe(expected.M13, Tolerance);
            actual.M14.ShouldBe(expected.M14, Tolerance);

            actual.M21.ShouldBe(expected.M21, Tolerance);
            actual.M22.ShouldBe(expected.M22, Tolerance);
            actual.M23.ShouldBe(expected.M23, Tolerance);
            actual.M24.ShouldBe(expected.M24, Tolerance);

            actual.M31.ShouldBe(expected.M31, Tolerance);
            actual.M32.ShouldBe(expected.M32, Tolerance);
            actual.M33.ShouldBe(expected.M33, Tolerance);
            actual.M34.ShouldBe(expected.M34, Tolerance);

            actual.M41.ShouldBe(expected.M41, Tolerance);
            actual.M42.ShouldBe(expected.M42, Tolerance);
            actual.M43.ShouldBe(expected.M43, Tolerance);
            actual.M44.ShouldBe(expected.M44, Tolerance);
        }

        public static void AssertActualEqualToExpected(Material actual, Material expected)
        {
            AssertActualEqualToExpected(actual.Color, expected.Color);
            actual.Ambient.ShouldBe(expected.Ambient, Tolerance);
            actual.Diffuse.ShouldBe(expected.Diffuse, Tolerance);
            actual.Specular.ShouldBe(expected.Specular, Tolerance);
            actual.Shininess.ShouldBe(expected.Shininess, Tolerance);
        }
    }
}
