﻿using System;
using System.Numerics;
using NUnit.Framework;

namespace RayTracer.Tests.SpecTests.Framework
{
    static class Comparisons
    {
        public const float Tolerance = 0.00001f;

        public static bool EquivalentTo(this float a, float b) => Math.Abs(a - b) < Tolerance;
        public static bool EquivalentTo(this float a, double b) => Math.Abs(a - b) < Tolerance;

        public static bool IsPoint(this Vector4 tuple) => tuple.W.EquivalentTo(1);
        public static bool IsVector(this Vector4 tuple) => tuple.W.EquivalentTo(0);

        public static void AssertActualEqualToExpected(Vector4 actual, Vector4 expected)
        {
            Assert.That(actual.X, Is.EqualTo(expected.X).Within(Tolerance), "Unexpected X value");
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(Tolerance), "Unexpected Y value");
            Assert.That(actual.Z, Is.EqualTo(expected.Z).Within(Tolerance), "Unexpected Z value");
            Assert.That(actual.W, Is.EqualTo(expected.W).Within(Tolerance), "Unexpected W value");
        }

        public static void AssertActualEqualToExpected(Matrix4x4 actual, Matrix4x4 expected)
        {
            Assert.That(actual.M11, Is.EqualTo(expected.M11).Within(Tolerance), "Unexpected M11 value");
            Assert.That(actual.M12, Is.EqualTo(expected.M12).Within(Tolerance), "Unexpected M12 value");
            Assert.That(actual.M13, Is.EqualTo(expected.M13).Within(Tolerance), "Unexpected M13 value");
            Assert.That(actual.M14, Is.EqualTo(expected.M14).Within(Tolerance), "Unexpected M14 value");

            Assert.That(actual.M21, Is.EqualTo(expected.M21).Within(Tolerance), "Unexpected M21 value");
            Assert.That(actual.M22, Is.EqualTo(expected.M22).Within(Tolerance), "Unexpected M22 value");
            Assert.That(actual.M23, Is.EqualTo(expected.M23).Within(Tolerance), "Unexpected M23 value");
            Assert.That(actual.M24, Is.EqualTo(expected.M24).Within(Tolerance), "Unexpected M24 value");

            Assert.That(actual.M31, Is.EqualTo(expected.M31).Within(Tolerance), "Unexpected M31 value");
            Assert.That(actual.M32, Is.EqualTo(expected.M32).Within(Tolerance), "Unexpected M32 value");
            Assert.That(actual.M33, Is.EqualTo(expected.M33).Within(Tolerance), "Unexpected M33 value");
            Assert.That(actual.M34, Is.EqualTo(expected.M34).Within(Tolerance), "Unexpected M34 value");

            Assert.That(actual.M41, Is.EqualTo(expected.M41).Within(Tolerance), "Unexpected M41 value");
            Assert.That(actual.M42, Is.EqualTo(expected.M42).Within(Tolerance), "Unexpected M42 value");
            Assert.That(actual.M43, Is.EqualTo(expected.M43).Within(Tolerance), "Unexpected M43 value");
            Assert.That(actual.M44, Is.EqualTo(expected.M44).Within(Tolerance), "Unexpected M44 value");
        }
    }
}
