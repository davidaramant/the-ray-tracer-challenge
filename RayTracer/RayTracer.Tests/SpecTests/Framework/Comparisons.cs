using System;
using System.Numerics;
using NUnit.Framework;

namespace RayTracer.Tests.SpecTests.Framework
{
    static class Comparisons
    {
        public const float Tolerance = 0.0001f;

        public static bool EquivalentTo(this float a, float b) => Math.Abs(a - b) < Tolerance;

        public static bool IsPoint(this Vector4 tuple) => tuple.W.EquivalentTo(1);
        public static bool IsVector(this Vector4 tuple) => tuple.W.EquivalentTo(0);

        public static void AssertActualEqualToExpected(Vector4 actual, Vector4 expected)
        {
            Assert.That(actual.X, Is.EqualTo(expected.X).Within(Tolerance), "Unexpected X value");
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(Tolerance), "Unexpected Y value");
            Assert.That(actual.Z, Is.EqualTo(expected.Z).Within(Tolerance), "Unexpected Z value");
            Assert.That(actual.W, Is.EqualTo(expected.W).Within(Tolerance), "Unexpected W value");
        }
    }
}
