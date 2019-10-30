using NUnit.Framework;
using RayTracer.Core;
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
    [TestFixture]
    public sealed class LightTests
    {
        //Scenario: A point light has a position and intensity
        //  Given intensity ← color(1, 1, 1)
        //    And position ← point(0, 0, 0)
        //  When light ← point_light(position, intensity)
        //  Then light.position = position
        //    And light.intensity = intensity
        [Test]
        public void ShouldHavePositionAndIntensityOnPointLight()
        {
            var intensity = VColor.White;
            var position = CreatePoint(0, 0, 0);
            var light = new PointLight(position, intensity);
            Assert.That(light.Position, Is.EqualTo(position));
            Assert.That(light.Intensity, Is.EqualTo(intensity));
        }

    }
}