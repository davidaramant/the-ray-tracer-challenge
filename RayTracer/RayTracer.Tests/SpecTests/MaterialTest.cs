using RayTracer.Core;
using RayTracer.Core.Patterns;
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
    /// materials.feature
    /// </summary>
        public sealed class MaterialTest
    {
        //Scenario: The default material
        //  Given m ← material()
        //  Then m.color = color(1, 1, 1)
        //    And m.ambient = 0.1
        //    And m.diffuse = 0.9
        //    And m.specular = 0.9
        //    And m.shininess = 200.0
        [Fact]
        public void ShouldHaveExpectedValuesForDefaultMaterial()
        {
            var m = new Material();
            AssertActualEqualToExpected(m.Color, VColor.White);
            m.Ambient.Should().BeApproximately(0.1f, Tolerance);
            m.Diffuse.Should().BeApproximately(0.9f, Tolerance);
            m.Specular.Should().BeApproximately(0.9f, Tolerance);
            m.Shininess.Should().BeApproximately(200, Tolerance);
        }

        //Scenario: Reflectivity for the default material
        //  Given m ← material()
        //  Then m.reflective = 0.0

        //Scenario: Transparency and Refractive Index for the default material
        //  Given m ← material()
        //  Then m.transparency = 0.0
        //    And m.refractive_index = 1.0

        //Scenario: Lighting with the eye between the light and the surface
        //  Given eyev ← vector(0, 0, -1)
        //    And normalv ← vector(0, 0, -1)
        //    And light ← point_light(point(0, 0, -10), color(1, 1, 1))
        //  When result ← lighting(m, light, position, eyev, normalv)
        //  Then result = color(1.9, 1.9, 1.9)
        [Fact]
        public void ShouldComputeLightingWithEyeBetweenLightAndSurface()
        {
            var m = new Material();
            var p = CreatePoint(0, 0, 0);
            var eye = CreateVector(0, 0, -1);
            var normalY = CreateVector(0, 0, -1);
            var light = new PointLight(CreatePoint(0, 0, -10), VColor.White);

            var result = m.ComputeColor(light, new Sphere(), p, eye, normalY);
            AssertActualEqualToExpected(result, VColor.LinearRGB(1.9f, 1.9f, 1.9f));
        }

        //Scenario: Lighting with the eye between light and surface, eye offset 45°
        //  Given eyev ← vector(0, √2/2, -√2/2)
        //    And normalv ← vector(0, 0, -1)
        //    And light ← point_light(point(0, 0, -10), color(1, 1, 1))
        //  When result ← lighting(m, light, position, eyev, normalv)
        //  Then result = color(1.0, 1.0, 1.0)
        [Fact]
        public void ShouldComputeLightingWithEyeBetweenLightAndSurfaceAndOffset45Degrees()
        {
            var m = new Material();
            var p = CreatePoint(0, 0, 0);
            var eye = CreateVector(0, Sqrt(2) / 2, Sqrt(2) / 2);
            var normalY = CreateVector(0, 0, -1);
            var light = new PointLight(CreatePoint(0, 0, -10), VColor.White);

            var result = m.ComputeColor(light, new Sphere(), p, eye, normalY);
            AssertActualEqualToExpected(result, VColor.White);
        }

        //Scenario: Lighting with eye opposite surface, light offset 45°
        //  Given eyev ← vector(0, 0, -1)
        //    And normalv ← vector(0, 0, -1)
        //    And light ← point_light(point(0, 10, -10), color(1, 1, 1))
        //  When result ← lighting(m, light, position, eyev, normalv)
        //  Then result = color(0.7364, 0.7364, 0.7364)
        [Fact]
        public void ShouldComputeLightingWithEyeOppositeSurfaceWithLightOffset45Degrees()
        {
            var m = new Material();
            var p = CreatePoint(0, 0, 0);
            var eye = CreateVector(0, 0, -1);
            var normalY = CreateVector(0, 0, -1);
            var light = new PointLight(CreatePoint(0, 10, -10), VColor.White);

            var result = m.ComputeColor(light, new Sphere(), p, eye, normalY);
            AssertActualEqualToExpected(result, VColor.LinearRGB(0.7364f, 0.7364f, 0.7364f));
        }

        //Scenario: Lighting with eye in the path of the reflection vector
        //  Given eyev ← vector(0, -√2/2, -√2/2)
        //    And normalv ← vector(0, 0, -1)
        //    And light ← point_light(point(0, 10, -10), color(1, 1, 1))
        //  When result ← lighting(m, light, position, eyev, normalv)
        //  Then result = color(1.6364, 1.6364, 1.6364)
        [Fact]
        public void ShouldComputeLightingWithEyeInPathOfReflectionVector()
        {
            var m = new Material();
            var p = CreatePoint(0, 0, 0);
            var eye = CreateVector(0, -Sqrt(2) / 2, -Sqrt(2) / 2);
            var normalY = CreateVector(0, 0, -1);
            var light = new PointLight(CreatePoint(0, 10, -10), VColor.White);

            var result = m.ComputeColor(light, new Sphere(), p, eye, normalY);
            AssertActualEqualToExpected(result, VColor.LinearRGB(1.6364f, 1.6364f, 1.6364f));
        }

        //Scenario: Lighting with the light behind the surface
        //  Given eyev ← vector(0, 0, -1)
        //    And normalv ← vector(0, 0, -1)
        //    And light ← point_light(point(0, 0, 10), color(1, 1, 1))
        //  When result ← lighting(m, light, position, eyev, normalv)
        //  Then result = color(0.1, 0.1, 0.1)
        [Fact]
        public void ShouldComputeLightingWithLightBehindSurface()
        {
            var m = new Material();
            var p = CreatePoint(0, 0, 0);
            var eye = CreateVector(0, 0, -1);
            var normalY = CreateVector(0, 0, -1);
            var light = new PointLight(CreatePoint(0, 0, 10), VColor.White);

            var result = m.ComputeColor(light, new Sphere(), p, eye, normalY);
            AssertActualEqualToExpected(result, VColor.LinearRGB(0.1f, 0.1f, 0.1f));
        }

        //Scenario: Lighting with the surface in shadow
        //  Given eyev ← vector(0, 0, -1)
        //    And normalv ← vector(0, 0, -1)
        //    And light ← point_light(point(0, 0, -10), color(1, 1, 1))
        //    And in_shadow ← true
        //  When result ← lighting(m, light, position, eyev, normalv, in_shadow)
        //  Then result = color(0.1, 0.1, 0.1)
        [Fact]
        public void ShouldComputeLightingWithSurfaceInShadow()
        {
            var m = new Material();
            var p = CreatePoint(0, 0, 0);
            var eye = CreateVector(0, 0, -1);
            var normal = CreateVector(0, 0, -1);
            var light = new PointLight(CreatePoint(0, 0, -10), VColor.White);

            var result = m.ComputeColor(light, new Sphere(), p, eye, normal, inShadow: true);
            AssertActualEqualToExpected(result, VColor.LinearRGB(0.1f, 0.1f, 0.1f));
        }

        //Scenario: Lighting with a pattern applied
        //  Given m.pattern ← stripe_pattern(color(1, 1, 1), color(0, 0, 0))
        //    And m.ambient ← 1
        //    And m.diffuse ← 0
        //    And m.specular ← 0
        //    And eyev ← vector(0, 0, -1)
        //    And normalv ← vector(0, 0, -1)
        //    And light ← point_light(point(0, 0, -10), color(1, 1, 1))
        //  When c1 ← lighting(m, light, point(0.9, 0, 0), eyev, normalv, false)
        //    And c2 ← lighting(m, light, point(1.1, 0, 0), eyev, normalv, false)
        //  Then c1 = color(1, 1, 1)
        //    And c2 = color(0, 0, 0)
        [Fact]
        public void ShouldComputeLightingWithPatternApplied()
        {
            var m = new Material
            {
                Pattern = new StripePattern(VColor.White, VColor.Black),
                Ambient = 1,
                Diffuse = 0,
                Specular = 0,
            };
            var eye = CreateVector(0, 0, -1);
            var normal = CreateVector(0, 0, -1);
            var light = new PointLight(CreatePoint(0, 0, -10), VColor.White);

            var c1 = m.ComputeColor(light, new Sphere { Material = m }, CreatePoint(0.9f, 0, 0), eye, normal, inShadow: true);
            AssertActualEqualToExpected(c1, VColor.White);

            var c2 = m.ComputeColor(light, new Sphere { Material = m }, CreatePoint(1.1f, 0, 0), eye, normal, inShadow: true);
            AssertActualEqualToExpected(c2, VColor.Black);
        }
    }
}