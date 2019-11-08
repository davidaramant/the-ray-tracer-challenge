using RayTracer.Core;
using Shouldly;
using Xunit;
using static System.MathF;
using static System.Numerics.Matrix4x4;
using static System.Numerics.Vector4;
using static RayTracer.Core.Tuples;
using static RayTracer.Tests.SpecTests.Framework.Comparisons;

namespace RayTracer.Tests.SpecTests
{
    /// <summary>
    /// lights.feature
    /// </summary>
        public sealed class LightTests
    {
        //Scenario: A point light has a position and intensity
        //  Given intensity ← color(1, 1, 1)
        //    And position ← point(0, 0, 0)
        //  When light ← point_light(position, intensity)
        //  Then light.position = position
        //    And light.intensity = intensity
        [Fact]
        public void ShouldHavePositionAndIntensityOnPointLight()
        {
            var intensity = VColor.White;
            var position = CreatePoint(0, 0, 0);
            var light = new PointLight(position, intensity);
            light.Position.ShouldBe(position);
            light.Intensity.ShouldBe(intensity);
        }

    }
}