using System.Collections.Generic;
using System.Linq;
using RayTracer.Core;
using RayTracer.Core.Shapes;
using FluentAssertions;
using RayTracer.Core.Patterns;
using RayTracer.Tests.SpecTests.Framework;
using Xunit;
using static System.MathF;
using static System.Numerics.Matrix4x4;
using static System.Numerics.Vector4;
using static RayTracer.Core.Tuples;
using static RayTracer.Tests.SpecTests.Framework.Comparisons;

namespace RayTracer.Tests.SpecTests
{
    /// <summary>
    /// world.feature
    /// </summary>
    public class WorldTests
    {
        //Scenario: Creating a world
        //  Given w ← world()
        //  Then w contains no objects
        //    And w has no light source
        [Fact]
        public void ShouldCreateEmptyWorld()
        {
            var w = new World();
            w.Objects.Should().BeEmpty();
            w.Lights.Should().BeEmpty();
        }

        //Scenario: The default world
        //  Given light ← point_light(point(-10, 10, -10), color(1, 1, 1))
        //    And s1 ← sphere() with:
        //      | material.color     | (0.8, 1.0, 0.6)        |
        //      | material.diffuse   | 0.7                    |
        //      | material.specular  | 0.2                    |
        //    And s2 ← sphere() with:
        //      | transform | scaling(0.5, 0.5, 0.5) |
        //  When w ← default_world()
        //  Then w.light = light
        //    And w contains s1
        //    And w contains s2
        [Fact]
        public void ShouldCreateDefaultWorld()
        {
            var light = new PointLight(CreatePoint(-10, 10, -10), VColor.White);
            var s1 = new Sphere
            {
                Material =
                {
                    Color = VColor.LinearRGB(0.8f,1,0.6f),
                    Diffuse = 0.7f,
                    Specular = 0.2f,
                }
            };
            var s2 = new Sphere
            {
                Transform = CreateScale(0.5f, 0.5f, 0.5f),
            };

            var w = World.CreateDefault();
            w.Lights.Should().Contain(light);
            w.Objects.Count.Should().Be(2);
            w.Objects[0].Material.Should().Be(s1.Material);
            w.Objects[0].Transform.Should().Be(s1.Transform);
            w.Objects[1].Material.Should().Be(s2.Material);
            w.Objects[1].Transform.Should().Be(s2.Transform);
        }

        //Scenario: Intersect a world with a ray
        //  Given w ← default_world()
        //    And r ← ray(point(0, 0, -5), vector(0, 0, 1))
        //  When xs ← intersect_world(w, r)
        //  Then xs.count = 4
        //    And xs[0].t = 4
        //    And xs[1].t = 4.5
        //    And xs[2].t = 5.5
        //    And xs[3].t = 6
        [Fact]
        public void ShouldIntersectWithRay()
        {
            var w = World.CreateDefault();
            var r = CreateRay(CreatePoint(0, 0, -5), CreateVector(0, 0, 1));
            var xs = w.Intersect(r);
            xs.Count.Should().Be(4);
            xs[0].T.Should().BeApproximately(4, Tolerance);
            xs[1].T.Should().BeApproximately(4.5f, Tolerance);
            xs[2].T.Should().BeApproximately(5.5f, Tolerance);
            xs[3].T.Should().BeApproximately(6, Tolerance);
        }

        //Scenario: Shading an intersection
        //  Given w ← default_world()
        //    And r ← ray(point(0, 0, -5), vector(0, 0, 1))
        //    And shape ← the first object in w
        //    And i ← intersection(4, shape)
        //  When comps ← prepare_computations(i, r)
        //    And c ← shade_hit(w, comps)
        //  Then c = color(0.38066, 0.47583, 0.2855)
        [Fact]
        public void ShouldShadeIntersection()
        {
            var w = World.CreateDefault();
            var r = CreateRay(CreatePoint(0, 0, -5), CreateVector(0, 0, 1));
            var shape = w.Objects.First();
            var i = new Intersection(4, shape);
            var comps = Computations.Prepare(i, r);
            var c = w.ShadeHit(comps);
            AssertActualEqualToExpected(c, VColor.LinearRGB(0.38066f, 0.47583f, 0.2855f));
        }

        //Scenario: Shading an intersection from the inside
        //  Given w ← default_world()
        //    And w.light ← point_light(point(0, 0.25, 0), color(1, 1, 1))
        //    And r ← ray(point(0, 0, 0), vector(0, 0, 1))
        //    And shape ← the second object in w
        //    And i ← intersection(0.5, shape)
        //  When comps ← prepare_computations(i, r)
        //    And c ← shade_hit(w, comps)
        //  Then c = color(0.90498, 0.90498, 0.90498)
        [Fact]
        public void ShouldShadeIntersectionFromTheInside()
        {
            var w = World.CreateDefault();
            w.Lights[0] = new PointLight(CreatePoint(0, 0.25f, 0), VColor.White);
            var r = CreateRay(CreatePoint(0, 0, 0), CreateVector(0, 0, 1));
            var shape = w.Objects[1];
            var i = new Intersection(0.5f, shape);
            var comps = Computations.Prepare(i, r);
            var c = w.ShadeHit(comps);
            AssertActualEqualToExpected(c, VColor.LinearRGB(0.90498f, 0.90498f, 0.90498f));
        }

        //Scenario: The color when a ray misses
        //  Given w ← default_world()
        //    And r ← ray(point(0, 0, -5), vector(0, 1, 0))
        //  When c ← color_at(w, r)
        //  Then c = color(0, 0, 0)
        [Fact]
        public void ShouldComputeColorWhenRayMisses()
        {
            var w = World.CreateDefault();
            var r = CreateRay(CreatePoint(0, 0, -5), CreateVector(0, 1, 0));
            var c = w.ComputeColor(r);
            AssertActualEqualToExpected(c, VColor.Black);
        }

        //Scenario: The color when a ray hits
        //  Given w ← default_world()
        //    And r ← ray(point(0, 0, -5), vector(0, 0, 1))
        //  When c ← color_at(w, r)
        //  Then c = color(0.38066, 0.47583, 0.2855)
        [Fact]
        public void ShouldComputeColorWhenRayHits()
        {
            var w = World.CreateDefault();
            var r = CreateRay(CreatePoint(0, 0, -5), CreateVector(0, 0, 1));
            var c = w.ComputeColor(r);
            AssertActualEqualToExpected(c, VColor.LinearRGB(0.38066f, 0.47583f, 0.2855f));
        }

        //Scenario: The color with an intersection behind the ray
        //  Given w ← default_world()
        //    And outer ← the first object in w
        //    And outer.material.ambient ← 1
        //    And inner ← the second object in w
        //    And inner.material.ambient ← 1
        //    And r ← ray(point(0, 0, 0.75), vector(0, 0, -1))
        //  When c ← color_at(w, r)
        //  Then c = inner.material.color
        [Fact]
        public void ShouldComputeColorWhenIntersectionIsBehindRay()
        {
            var w = World.CreateDefault();
            var outer = w.Objects.First();
            outer.Material.Ambient = 1;
            var inner = w.Objects[1];
            inner.Material.Ambient = 1;
            var r = CreateRay(CreatePoint(0, 0, 0.75f), CreateVector(0, 0, -1));
            var c = w.ComputeColor(r);
            AssertActualEqualToExpected(c, inner.Material.Color);
        }

        //Scenario: There is no shadow when nothing is collinear with point and light
        //  Given w ← default_world()
        //    And p ← point(0, 10, 0)
        //   Then is_shadowed(w, p) is false
        //
        //Scenario: The shadow when an object is between the point and the light
        //  Given w ← default_world()
        //    And p ← point(10, -10, 10)
        //   Then is_shadowed(w, p) is true
        //
        //Scenario: There is no shadow when an object is behind the light
        //  Given w ← default_world()
        //    And p ← point(-20, 20, -20)
        //   Then is_shadowed(w, p) is false
        //
        //Scenario: There is no shadow when an object is behind the point
        //  Given w ← default_world()
        //    And p ← point(-2, 2, -2)
        //   Then is_shadowed(w, p) is false
        [Theory]
        [InlineData(0, 10, 0, false)]
        [InlineData(10, -10, 10, true)]
        [InlineData(-20, 20, -20, false)]
        [InlineData(-2, 2, -2, false)]
        public void ShouldDetermineIfPointIsInShadowOfLight(float x, float y, float z, bool expectedInShadow)
        {
            var w = World.CreateDefault();
            var p = CreatePoint(x, y, z);
            w.IsShadowed(w.Lights.First(), p).Should().Be(expectedInShadow);
        }

        //Scenario: shade_hit() is given an intersection in shadow
        //  Given w ← world()
        //    And w.light ← point_light(point(0, 0, -10), color(1, 1, 1))
        //    And s1 ← sphere()
        //    And s1 is added to w
        //    And s2 ← sphere() with:
        //      | transform | translation(0, 0, 10) |
        //    And s2 is added to w
        //    And r ← ray(point(0, 0, 5), vector(0, 0, 1))
        //    And i ← intersection(4, s2)
        //  When comps ← prepare_computations(i, r)
        //    And c ← shade_hit(w, comps)
        //  Then c = color(0.1, 0.1, 0.1)
        [Fact]
        public void ShouldShowShadeIntersectionInShadow()
        {
            var w = new World
            {
                Lights = { new PointLight(CreatePoint(0, 0, -10), VColor.White) },
                Objects =
                {
                    new Sphere(),
                    new Sphere
                    {
                        Transform = CreateTranslation(0,0,10)
                    },
                }
            };
            var r = CreateRay(CreatePoint(0, 0, 5), CreateVector(0, 0, 1));
            var i = new Intersection(4, w.Objects[1]);
            var comps = Computations.Prepare(i, r);
            var c = w.ShadeHit(comps);
            AssertActualEqualToExpected(c, VColor.LinearRGB(0.1f, 0.1f, 0.1f));
        }

        //Scenario: The reflected color for a nonreflective material
        //  Given w ← default_world()
        //    And r ← ray(point(0, 0, 0), vector(0, 0, 1))
        //    And shape ← the second object in w
        //    And shape.material.ambient ← 1
        //    And i ← intersection(1, shape)
        //  When comps ← prepare_computations(i, r)
        //    And color ← reflected_color(w, comps)
        //  Then color = color(0, 0, 0)
        [Fact]
        public void ShouldDetermineReflectedColorForNonReflectiveMaterial()
        {
            var w = World.CreateDefault();
            var r = CreateRay(CreatePoint(0, 0, 0), CreateVector(0, 0, 1));
            var shape = w.Objects[1];
            shape.Material.Ambient = 1;
            var i = new Intersection(1, shape);
            var comps = Computations.Prepare(i, r);
            var color = w.ComputeReflectedColor(comps);
            AssertActualEqualToExpected(color, VColor.Black);
        }

        //Scenario: The reflected color for a reflective material
        //  Given w ← default_world()
        //    And shape ← plane() with:                 
        //      | material.reflective | 0.5                   |
        //      | transform           | translation(0, -1, 0) |   
        //    And shape is added to w
        //    And r ← ray(point(0, 0, -3), vector(0, -√2/2, √2/2))
        //    And i ← intersection(√2, shape)
        //  When comps ← prepare_computations(i, r)
        //    And color ← reflected_color(w, comps)
        //  Then color = color(0.19032, 0.2379, 0.14274)
        [Fact]
        public void ShouldDetermineReflectedColorForReflectiveMaterial()
        {
            var w = World.CreateDefault();
            var shape = new XZPlane
            {
                Material = { Reflective = 0.5f },
                Transform = CreateTranslation(0, -1, 0),
            };
            w.Objects.Add(shape);
            var r = CreateRay(CreatePoint(0, 0, -3), CreateVector(0, -Sqrt(2) / 2, Sqrt(2) / 2));
            var i = new Intersection(Sqrt(2), shape);
            var comps = Computations.Prepare(i, r);
            var color = w.ComputeReflectedColor(comps, 1);
            AssertActualEqualToExpected(color, VColor.LinearRGB(0.19032f, 0.2379f, 0.14274f));
        }

        //Scenario: shade_hit() with a reflective material
        //  Given w ← default_world()
        //    And shape ← plane() with:
        //      | material.reflective | 0.5                   |
        //      | transform           | translation(0, -1, 0) |
        //    And shape is added to w
        //    And r ← ray(point(0, 0, -3), vector(0, -√2/2, √2/2))
        //    And i ← intersection(√2, shape)
        //  When comps ← prepare_computations(i, r)
        //    And color ← shade_hit(w, comps)
        //  Then color = color(0.87677, 0.92436, 0.82918)
        [Fact]
        public void ShouldComputeColorForReflectiveMaterial()
        {
            var w = World.CreateDefault();
            var shape = new XZPlane
            {
                Material = { Reflective = 0.5f },
                Transform = CreateTranslation(0, -1, 0),
            };
            w.Objects.Add(shape);
            var r = CreateRay(CreatePoint(0, 0, -3), CreateVector(0, -Sqrt(2) / 2, Sqrt(2) / 2));
            var i = new Intersection(Sqrt(2), shape);
            var comps = Computations.Prepare(i, r);
            var color = w.ShadeHit(comps, 4);
            AssertActualEqualToExpected(color, VColor.LinearRGB(0.87677f, 0.92436f, 0.82918f));
        }

        //Scenario: color_at() with mutually reflective surfaces
        //  Given w ← world()
        //    And w.light ← point_light(point(0, 0, 0), color(1, 1, 1))
        //    And lower ← plane() with:
        //      | material.reflective | 1                     |
        //      | transform           | translation(0, -1, 0) |
        //    And lower is added to w
        //    And upper ← plane() with:
        //      | material.reflective | 1                    |
        //      | transform           | translation(0, 1, 0) |
        //    And upper is added to w
        //    And r ← ray(point(0, 0, 0), vector(0, 1, 0))
        //  Then color_at(w, r) should terminate successfully
        [Fact]
        public void ShouldPreventInfiniteReflections()
        {
            var w = new World
            {
                Lights = { new PointLight(CreatePoint(0, 0, 0), VColor.White) },
                Objects =
                {
                    new XZPlane
                    {
                        Material = {Reflective = 1},
                        Transform = CreateTranslation(0,-1,0),
                    },
                    new XZPlane
                    {
                        Material = {Reflective = 1},
                        Transform = CreateTranslation(0,1,0),
                    }
                }
            };
            var r = CreateRay(CreatePoint(0, 0, 0), CreateVector(0, 1, 0));
            w.ComputeColor(r);
        }

        //Scenario: The reflected color at the maximum recursive depth
        //  Given w ← default_world()
        //    And shape ← plane() with:
        //      | material.reflective | 0.5                   |
        //      | transform           | translation(0, -1, 0) |
        //    And shape is added to w
        //    And r ← ray(point(0, 0, -3), vector(0, -√2/2, √2/2))
        //    And i ← intersection(√2, shape)
        //  When comps ← prepare_computations(i, r)
        //    And color ← reflected_color(w, comps, 0)    
        //  Then color = color(0, 0, 0)
        [Fact]
        public void ShouldComputeColorForReflectiveMaterialAtMaximumRecursiveDepth()
        {
            var w = World.CreateDefault();
            var shape = new XZPlane
            {
                Material = { Reflective = 0.5f },
                Transform = CreateTranslation(0, -1, 0),
            };
            w.Objects.Add(shape);
            var r = CreateRay(CreatePoint(0, 0, -3), CreateVector(0, -Sqrt(2) / 2, Sqrt(2) / 2));
            var i = new Intersection(Sqrt(2), shape);
            var comps = Computations.Prepare(i, r);
            var color = w.ComputeReflectedColor(comps, 0);
            AssertActualEqualToExpected(color, VColor.Black);
        }

        //Scenario: The refracted color with an opaque surface
        //  Given w ← default_world()
        //    And shape ← the first object in w
        //    And r ← ray(point(0, 0, -5), vector(0, 0, 1))
        //    And xs ← intersections(4:shape, 6:shape)
        //  When comps ← prepare_computations(xs[0], r, xs)
        //    And c ← refracted_color(w, comps, 5)
        //  Then c = color(0, 0, 0)
        [Fact]
        public void ShouldComputeRefractedColorWithOpaqueSurface()
        {
            var w = World.CreateDefault();
            var shape = w.Objects.First();
            var r = new Ray(CreatePoint(0, 0, -5), CreateVector(0, 0, 1));
            var xs = new List<Intersection> { new Intersection(4, shape), new Intersection(6, shape) };
            var comps = Computations.Prepare(xs[0], r, xs);
            var c = w.ComputeRefractedColor(comps, 5);
            AssertActualEqualToExpected(c, VColor.Black);
        }

        //Scenario: The refracted color at the maximum recursive depth
        //  Given w ← default_world()
        //    And shape ← the first object in w
        //    And shape has:
        //      | material.transparency     | 1.0 |
        //      | material.refractive_index | 1.5 |
        //    And r ← ray(point(0, 0, -5), vector(0, 0, 1))
        //    And xs ← intersections(4:shape, 6:shape)
        //  When comps ← prepare_computations(xs[0], r, xs)
        //    And c ← refracted_color(w, comps, 0)
        //  Then c = color(0, 0, 0)
        [Fact]
        public void ShouldComputeRefractedColorAtMaxRecursiveDepth()
        {
            var w = World.CreateDefault();
            var shape = w.Objects.First();
            shape.Material.Transparency = 1;
            shape.Material.RefractiveIndex = 1.5f;
            var r = new Ray(CreatePoint(0, 0, -5), CreateVector(0, 0, 1));
            var xs = new List<Intersection> { new Intersection(4, shape), new Intersection(6, shape) };
            var comps = Computations.Prepare(xs[0], r, xs);
            var c = w.ComputeRefractedColor(comps, 0);
            AssertActualEqualToExpected(c, VColor.Black);
        }

        //Scenario: The refracted color under total internal reflection
        //  Given w ← default_world()
        //    And shape ← the first object in w
        //    And shape has:
        //      | material.transparency     | 1.0 |
        //      | material.refractive_index | 1.5 |
        //    And r ← ray(point(0, 0, √2/2), vector(0, 1, 0))
        //    And xs ← intersections(-√2/2:shape, √2/2:shape)
        //  # NOTE: this time you're inside the sphere, so you need
        //  # to look at the second intersection, xs[1], not xs[0]
        //  When comps ← prepare_computations(xs[1], r, xs)
        //    And c ← refracted_color(w, comps, 5)
        //  Then c = color(0, 0, 0)
        [Fact]
        public void ShouldComputeRefractedColorUnderTotalInternalReflection()
        {
            var w = World.CreateDefault();
            var shape = w.Objects.First();
            shape.Material.Transparency = 1;
            shape.Material.RefractiveIndex = 1.5f;
            var r = new Ray(CreatePoint(0, 0, Sqrt(2) / 2), CreateVector(0, 1, 0));
            var xs = new List<Intersection> { new Intersection(-Sqrt(2) / 2, shape), new Intersection(Sqrt(2) / 2, shape) };
            var comps = Computations.Prepare(xs[1], r, xs);
            var c = w.ComputeRefractedColor(comps, 5);
            AssertActualEqualToExpected(c, VColor.Black);
        }

        //Scenario: The refracted color with a refracted ray
        //  Given w ← default_world()
        //    And A ← the first object in w
        //    And A has:
        //      | material.ambient | 1.0            |
        //      | material.pattern | test_pattern() |
        //    And B ← the second object in w
        //    And B has:
        //      | material.transparency     | 1.0 |
        //      | material.refractive_index | 1.5 |
        //    And r ← ray(point(0, 0, 0.1), vector(0, 1, 0))
        //    And xs ← intersections(-0.9899:A, -0.4899:B, 0.4899:B, 0.9899:A)
        //  When comps ← prepare_computations(xs[2], r, xs)
        //    And c ← refracted_color(w, comps, 5)
        //  Then c = color(0, 0.99888, 0.04725)
        [Fact]
        public void ShouldComputeRefractedColorWithRefractedRay()
        {
            var w = World.CreateDefault();

            var a = w.Objects.First();
            a.Material.Ambient = 1;
            a.Material.Pattern = new TestPattern();

            var b = w.Objects[1];
            b.Material.Transparency = 1;
            b.Material.RefractiveIndex = 1.5f;

            var r = new Ray(CreatePoint(0, 0, 0.1f), CreateVector(0, 1, 0));
            var xs = Intersection.CreateList((-0.9899f, a), (-0.4899f, b), (0.4899f, b), (0.9899f, a));
            var comps = Computations.Prepare(xs[2], r, xs);
            var c = w.ComputeRefractedColor(comps, 5);
            AssertActualEqualToExpected(c, VColor.LinearRGB(0, 0.99888f, 0.04725f));
        }

        //Scenario: shade_hit() with a transparent material
        //  Given w ← default_world()
        //    And floor ← plane() with:
        //      | transform                 | translation(0, -1, 0) |
        //      | material.transparency     | 0.5                   |
        //      | material.refractive_index | 1.5                   |
        //    And floor is added to w
        //    And ball ← sphere() with:
        //      | material.color     | (1, 0, 0)                  |
        //      | material.ambient   | 0.5                        |
        //      | transform          | translation(0, -3.5, -0.5) |
        //    And ball is added to w
        //    And r ← ray(point(0, 0, -3), vector(0, -√2/2, √2/2))
        //    And xs ← intersections(√2:floor)
        //  When comps ← prepare_computations(xs[0], r, xs)
        //    And color ← shade_hit(w, comps, 5)
        //  Then color = color(0.93642, 0.68642, 0.68642)
        [Fact]
        public void ShouldShadeHitWithTransparentMaterial()
        {
            var w = World.CreateDefault();
            var floorPlane = new XZPlane
            {
                Transform = CreateTranslation(0, -1, 0),
                Material =
                {
                    Transparency = 0.5f,
                    RefractiveIndex = 1.5f,
                }
            };
            w.Objects.Add(floorPlane);
            var ball = new Sphere
            {
                Material =
                {
                    Color = VColor.LinearRGB(1,0,0),
                    Ambient =  0.5f,
                },
                Transform = CreateTranslation(0, -3.5f, -0.5f),
            };
            w.Objects.Add(ball);

            var r = new Ray(CreatePoint(0, 0, -3), CreateVector(0, -Sqrt(2) / 2, Sqrt(2) / 2));
            var xs = Intersection.CreateList((Sqrt(2), floorPlane));
            var comps = Computations.Prepare(xs[0], r, xs);
            var color = w.ShadeHit(comps, 5);
            AssertActualEqualToExpected(color, VColor.LinearRGB(0.93642f, 0.68642f, 0.68642f));
        }

        //Scenario: shade_hit() with a reflective, transparent material
        //  Given w ← default_world()
        //    And r ← ray(point(0, 0, -3), vector(0, -√2/2, √2/2))
        //    And floor ← plane() with:
        //      | transform                 | translation(0, -1, 0) |
        //      | material.reflective       | 0.5                   |
        //      | material.transparency     | 0.5                   |
        //      | material.refractive_index | 1.5                   |
        //    And floor is added to w
        //    And ball ← sphere() with:
        //      | material.color     | (1, 0, 0)                  |
        //      | material.ambient   | 0.5                        |
        //      | transform          | translation(0, -3.5, -0.5) |
        //    And ball is added to w
        //    And xs ← intersections(√2:floor)
        //  When comps ← prepare_computations(xs[0], r, xs)
        //    And color ← shade_hit(w, comps, 5)
        //  Then color = color(0.93391, 0.69643, 0.69243)

    }
}