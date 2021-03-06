﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Numerics;
using RayTracer.Core;
using RayTracer.Core.Shapes;
using FluentAssertions;
using Xunit;
using static System.MathF;
using static System.Numerics.Matrix4x4;
using static System.Numerics.Vector4;
using static RayTracer.Core.Tuples;
using static RayTracer.Tests.SpecTests.Framework.Comparisons;

namespace RayTracer.Tests.SpecTests
{
    /// <summary>
    /// intersections.feature
    /// </summary>
    public class IntersectionTests
    {
        //Scenario: An intersection encapsulates t and object
        //  Given s ← sphere()
        //  When i ← intersection(3.5, s)
        //  Then i.t = 3.5
        //    And i.object = s

        //Scenario: Precomputing the state of an intersection
        //  Given r ← ray(point(0, 0, -5), vector(0, 0, 1))
        //    And shape ← sphere()
        //    And i ← intersection(4, shape)
        //  When comps ← prepare_computations(i, r)
        //  Then comps.t = i.t
        //    And comps.object = i.object
        //    And comps.point = point(0, 0, -1)
        //    And comps.eyev = vector(0, 0, -1)
        //    And comps.normalv = vector(0, 0, -1)
        [Fact]
        public void ShouldPrecomputeStateOfIntersection()
        {
            var r = CreateRay(CreatePoint(0, 0, -5), CreateVector(0, 0, 1));
            var shape = new Sphere();
            var i = new Intersection(4, shape);

            var comps = Computations.Prepare(i, r);
            comps.Object.Should().Be(shape);
            AssertActualEqualToExpected(comps.Point, CreatePoint(0, 0, -1));
            AssertActualEqualToExpected(comps.EyeV, CreateVector(0, 0, -1));
            AssertActualEqualToExpected(comps.NormalV, CreateVector(0, 0, -1));
        }

        //Scenario: Precomputing the reflection vector
        //  Given shape ← plane()
        //    And r ← ray(point(0, 1, -1), vector(0, -√2/2, √2/2)) 
        //    And i ← intersection(√2, shape)                      
        //  When comps ← prepare_computations(i, r)
        //  Then comps.reflectv = vector(0, √2/2, √2/2)
        [Fact]
        public void ShouldPrecomputeReflectionVector()
        {
            var shape = new XZPlane();
            var r = CreateRay(CreatePoint(0, 1, -1), CreateVector(0, -Sqrt(2) / 2, Sqrt(2) / 2));
            var i = new Intersection(Sqrt(2), shape);
            var comps = Computations.Prepare(i, r);
            AssertActualEqualToExpected(comps.ReflectV, CreateVector(0, Sqrt(2) / 2, Sqrt(2) / 2));

        }

        //Scenario: The hit, when an intersection occurs on the outside
        //  Given r ← ray(point(0, 0, -5), vector(0, 0, 1))
        //    And shape ← sphere()
        //    And i ← intersection(4, shape)
        //  When comps ← prepare_computations(i, r)
        //  Then comps.inside = false
        [Fact]
        public void ShouldIndicateWhenIntersectionOccursOnTheOutside()
        {
            var r = CreateRay(CreatePoint(0, 0, -5), CreateVector(0, 0, 1));
            var shape = new Sphere();
            var i = new Intersection(4, shape);

            var comps = Computations.Prepare(i, r);
            comps.Inside.Should().Be(false);
        }

        //Scenario: The hit, when an intersection occurs on the inside
        //  Given r ← ray(point(0, 0, 0), vector(0, 0, 1))
        //    And shape ← sphere()
        //    And i ← intersection(1, shape)
        //  When comps ← prepare_computations(i, r)
        //  Then comps.point = point(0, 0, 1)
        //    And comps.eyev = vector(0, 0, -1)
        //    And comps.inside = true
        //      # normal would have been (0, 0, 1), but is inverted!
        //    And comps.normalv = vector(0, 0, -1)
        [Fact]
        public void ShouldIndicateWhenIntersectionOccursOnTheInside()
        {
            var r = CreateRay(CreatePoint(0, 0, 0), CreateVector(0, 0, 1));
            var shape = new Sphere();
            var i = new Intersection(1, shape);

            var comps = Computations.Prepare(i, r);
            AssertActualEqualToExpected(comps.Point, CreatePoint(0, 0, 1));
            AssertActualEqualToExpected(comps.EyeV, CreateVector(0, 0, -1));
            comps.Inside.Should().Be(true);
            AssertActualEqualToExpected(comps.NormalV, CreateVector(0, 0, -1));
        }

        //Scenario: The hit should offset the point
        //  Given r ← ray(point(0, 0, -5), vector(0, 0, 1))
        //    And shape ← sphere() with:
        //      | transform | translation(0, 0, 1) |
        //    And i ← intersection(5, shape)
        //  When comps ← prepare_computations(i, r)
        //  Then comps.over_point.z < -EPSILON/2
        //    And comps.point.z > comps.over_point.z
        [Fact]
        public void ShouldOffsetPointInHit()
        {
            var r = CreateRay(CreatePoint(0, 0, -5), CreateVector(0, 0, 1));
            var shape = new Sphere { Transform = CreateTranslation(0, 0, 1) };
            var i = new Intersection(5, shape);

            var comps = Computations.Prepare(i, r);
            comps.OverPoint.Z.Should().BeLessThan(-Tolerance / 2);
            comps.Point.Z.Should().BeGreaterThan(comps.OverPoint.Z);
        }

        //Scenario: The under point is offset below the surface
        //  Given r ← ray(point(0, 0, -5), vector(0, 0, 1))
        //    And shape ← glass_sphere() with:
        //      | transform | translation(0, 0, 1) |
        //    And i ← intersection(5, shape)
        //    And xs ← intersections(i)
        //  When comps ← prepare_computations(i, r, xs)
        //  Then comps.under_point.z > EPSILON/2
        //    And comps.point.z < comps.under_point.z
        [Fact]
        public void ShouldComputeUnderPointBelowTheSurface()
        {
            var r = new Ray(CreatePoint(0, 0, -5f), CreateVector(0, 0, 1));
            var shape = Sphere.CreateGlass();
            shape.Transform = CreateTranslation(0, 0, 1);
            var i = new Intersection(5, shape);
            var xs = new List<Intersection> { i };
            var comps = Computations.Prepare(i, r, xs);
            comps.UnderPoint.Z.Should().BeGreaterThan(Tolerance / 2);
            comps.Point.Z.Should().BeLessThan(comps.UnderPoint.Z);
        }

        //Scenario: Aggregating intersections
        //  Given s ← sphere()
        //    And i1 ← intersection(1, s)
        //    And i2 ← intersection(2, s)
        //  When xs ← intersections(i1, i2)
        //  Then xs.count = 2
        //    And xs[0].t = 1
        //    And xs[1].t = 2
        [Fact]
        public void ShouldAggregateIntersections()
        {
            var s = new Sphere();
            var i1 = new Intersection(1, s);
            var i2 = new Intersection(2, s);
            var xs = new List<Intersection> { i1, i2 };
            xs.Count.Should().Be(2);
            xs.Should().Contain(new[] { i1, i2 });
        }

        //Scenario: The hit, when all intersections have positive t
        //  Given s ← sphere()
        //    And i1 ← intersection(1, s)
        //    And i2 ← intersection(2, s)
        //    And xs ← intersections(i2, i1)
        //  When i ← hit(xs)
        //  Then i = i1
        [Fact]
        public void ShouldReturnHitWhenAllIntersectionsHavePositiveT()
        {
            var s = new Sphere();
            var i1 = new Intersection(1, s);
            var i2 = new Intersection(2, s);
            var xs = new List<Intersection> { i1, i2 };
            xs.TryGetHit().Should().Be(i1);
        }

        //Scenario: The hit, when some intersections have negative t
        //  Given s ← sphere()
        //    And i1 ← intersection(-1, s)
        //    And i2 ← intersection(1, s)
        //    And xs ← intersections(i2, i1)
        //  When i ← hit(xs)
        //  Then i = i2
        [Fact]
        public void ShouldReturnHitWhenSomeIntersectionsHaveNegativeT()
        {
            var s = new Sphere();
            var i1 = new Intersection(-1, s);
            var i2 = new Intersection(1, s);
            var xs = new List<Intersection> { i1, i2 };
            xs.TryGetHit().Should().Be(i2);
        }

        //Scenario: The hit, when all intersections have negative t
        //  Given s ← sphere()
        //    And i1 ← intersection(-2, s)
        //    And i2 ← intersection(-1, s)
        //    And xs ← intersections(i2, i1)
        //  When i ← hit(xs)
        //  Then i is nothing
        [Fact]
        public void ShouldReturnHitWhenAllIntersectionsHaveNegativeT()
        {
            var s = new Sphere();
            var i1 = new Intersection(-2, s);
            var i2 = new Intersection(-1, s);
            var xs = new List<Intersection> { i1, i2 };
            xs.TryGetHit().Should().BeNull();
        }

        //Scenario: The hit is always the lowest nonnegative intersection
        //  Given s ← sphere()
        //  And i1 ← intersection(5, s)
        //  And i2 ← intersection(7, s)
        //  And i3 ← intersection(-3, s)
        //  And i4 ← intersection(2, s)
        //  And xs ← intersections(i1, i2, i3, i4)
        //When i ← hit(xs)
        //Then i = i4
        [Fact]
        public void ShouldReturnHitWhenIntersectionsAreOutOfOrder()
        {
            var s = new Sphere();
            var i1 = new Intersection(5, s);
            var i2 = new Intersection(7, s);
            var i3 = new Intersection(-3, s);
            var i4 = new Intersection(2, s);
            var xs = new List<Intersection> { i1, i2, i3, i4 };
            xs.TryGetHit().Should().Be(i4);
        }

        //Scenario Outline: Finding n1 and n2 at various intersections
        //  Given A ← glass_sphere() with:
        //      | transform                 | scaling(2, 2, 2) |
        //      | material.refractive_index | 1.5              |
        //    And B ← glass_sphere() with:
        //      | transform                 | translation(0, 0, -0.25) |
        //      | material.refractive_index | 2.0                      |
        //    And C ← glass_sphere() with:
        //      | transform                 | translation(0, 0, 0.25) |
        //      | material.refractive_index | 2.5                     |
        //    And r ← ray(point(0, 0, -4), vector(0, 0, 1))
        //    And xs ← intersections(2:A, 2.75:B, 3.25:C, 4.75:B, 5.25:C, 6:A)
        //  When comps ← prepare_computations(xs[<index>], r, xs)  
        //  Then comps.n1 = <n1>
        //    And comps.n2 = <n2>             

        //  Examples:
        //    | index | n1  | n2  |
        //    | 0     | 1.0 | 1.5 |                 
        //    | 1     | 1.5 | 2.0 |
        //    | 2     | 2.0 | 2.5 |
        //    | 3     | 2.5 | 2.5 |
        //    | 4     | 2.5 | 1.5 |
        //    | 5     | 1.5 | 1.0 |
        [Theory]
        [InlineData(0, 1.0f, 1.5f)]
        [InlineData(1, 1.5f, 2.0f)]
        [InlineData(2, 2.0f, 2.5f)]
        [InlineData(3, 2.5f, 2.5f)]
        [InlineData(4, 2.5f, 1.5f)]
        [InlineData(5, 1.5f, 1.0f)]
        public void ShouldFindN1AndN2AtIntersection(int index, float n1, float n2)
        {
            var a = Sphere.CreateGlass();
            a.Transform = CreateScale(2, 2, 2);
            a.Material.RefractiveIndex = 1.5f;

            var b = Sphere.CreateGlass();
            b.Transform = CreateTranslation(0, 0, -0.25f);
            b.Material.RefractiveIndex = 2f;

            var c = Sphere.CreateGlass();
            c.Transform = CreateTranslation(0, 0, 0.25f);
            c.Material.RefractiveIndex = 2.5f;

            var r = new Ray(CreatePoint(0, 0, -4f), CreateVector(0, 0, 1));
            var xs = new List<Intersection>
            {
                new Intersection(2,a),
                new Intersection(2.75f,b),
                new Intersection(3.25f,c),
                new Intersection(4.75f,b),
                new Intersection(5.25f,c),
                new Intersection(6,a),
            };

            var comps = Computations.Prepare(xs[index], r, xs);
            comps.N1.Should().BeApproximately(n1, Tolerance);
            comps.N2.Should().BeApproximately(n2, Tolerance);
        }

        //Scenario: The Schlick approximation under total internal reflection
        //  Given shape ← glass_sphere()
        //    And r ← ray(point(0, 0, √2/2), vector(0, 1, 0))
        //    And xs ← intersections(-√2/2:shape, √2/2:shape)
        //  When comps ← prepare_computations(xs[1], r, xs)
        //    And reflectance ← schlick(comps)
        //  Then reflectance = 1.0
        [Fact]
        public void ShouldComputeSchlickApproximationUnderTotalInternalReflection()
        {
            var shape = Sphere.CreateGlass();
            var r = new Ray(CreatePoint(0, 0, Sqrt(2) / 2), CreateVector(0, 1, 0));
            var xs = Intersection.CreateList((-Sqrt(2) / 2, shape), (Sqrt(2) / 2, shape));
            var comps = Computations.Prepare(xs[1], r, xs);
            var reflectance = comps.GetSchlickReflectance();
            reflectance.Should().BeApproximately(1, Tolerance);
        }

        //Scenario: The Schlick approximation with a perpendicular viewing angle
        //  Given shape ← glass_sphere()
        //    And r ← ray(point(0, 0, 0), vector(0, 1, 0))
        //    And xs ← intersections(-1:shape, 1:shape)
        //  When comps ← prepare_computations(xs[1], r, xs)
        //    And reflectance ← schlick(comps)
        //  Then reflectance = 0.04
        [Fact]
        public void ShouldComputeSchlickApproximationWithPerpendicularViewingAngle()
        {
            var shape = Sphere.CreateGlass();
            var r = new Ray(CreatePoint(0, 0, 0), CreateVector(0, 1, 0));
            var xs = Intersection.CreateList((-1, shape), (1, shape));
            var comps = Computations.Prepare(xs[1], r, xs);
            var reflectance = comps.GetSchlickReflectance();
            reflectance.Should().BeApproximately(0.04f, Tolerance);
        }

        //Scenario: The Schlick approximation with small angle and n2 > n1
        //  Given shape ← glass_sphere()
        //    And r ← ray(point(0, 0.99, -2), vector(0, 0, 1))
        //    And xs ← intersections(1.8589:shape)
        //  When comps ← prepare_computations(xs[0], r, xs)
        //    And reflectance ← schlick(comps)
        //  Then reflectance = 0.48873
        [Fact]
        public void ShouldComputeSchlickApproximationWithSmallAngleAndN2GreaterThanN1()
        {
            var shape = Sphere.CreateGlass();
            var r = new Ray(CreatePoint(0, .99f, -2), CreateVector(0, 0, 1));
            var xs = Intersection.CreateList((1.8589f, shape));
            var comps = Computations.Prepare(xs[0], r, xs);
            var reflectance = comps.GetSchlickReflectance();
            reflectance.Should().BeApproximately(0.48873f, Tolerance);
        }

        //Scenario: An intersection can encapsulate `u` and `v`
        //  Given s ← triangle(point(0, 1, 0), point(-1, 0, 0), point(1, 0, 0))
        //  When i ← intersection_with_uv(3.5, s, 0.2, 0.4)
        //  Then i.u = 0.2
        //    And i.v = 0.4
    }
}
