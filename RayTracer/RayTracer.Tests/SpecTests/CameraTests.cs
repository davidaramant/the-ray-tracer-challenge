using System;
using System.Drawing;
using Moq;
using RayTracer.Core;
using RayTracer.Core.Utilities;
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
    /// camera.feature
    /// </summary>
        public class CameraTests
    {
        private static IOutputBuffer CreateMockBuffer(int width, int height)
        {
            var mockBuffer = new Mock<IOutputBuffer>();
            mockBuffer.Setup(b => b.Dimensions).Returns(new Size(width, height));
            return mockBuffer.Object;
        }

        //Scenario: Constructing a camera
        //  Given hsize ← 160
        //    And vsize ← 120
        //    And field_of_view ← π/2
        //  When c ← camera(hsize, vsize, field_of_view)
        //  Then c.hsize = 160
        //    And c.vsize = 120
        //    And c.field_of_view = π/2
        //    And c.transform = identity_matrix
        [Fact]
        public void ShouldConstructCamera()
        {
            var hSize = 160;
            var vSize = 120;
            var fieldOfView = PI / 2;
            var c = new Camera(CreateMockBuffer(hSize, vSize), fieldOfView);
            c.Dimensions.Width.ShouldBe(hSize);
            c.Dimensions.Height.ShouldBe(vSize);
            c.FieldOfView.ShouldBe(fieldOfView, Tolerance);
            AssertActualEqualToExpected(c.Transform, Identity);
        }

        //Scenario: The pixel size for a horizontal canvas
        //  Given c ← camera(200, 125, π/2)
        //  Then c.pixel_size = 0.01
        [Fact]
        public void ShouldCalculatePixelSizeForHorizontalCanvas()
        {
            var c = new Camera(CreateMockBuffer(200, 125), PI / 2);
            c.PixelSize.ShouldBe(0.01f, Tolerance);
        }

        //Scenario: The pixel size for a vertical canvas
        //  Given c ← camera(125, 200, π/2)
        //  Then c.pixel_size = 0.01
        [Fact]
        public void ShouldCalculatePixelSizeForVerticalCanvas()
        {
            var c = new Camera(CreateMockBuffer(125, 200), PI / 2);
            c.PixelSize.ShouldBe(0.01f, Tolerance);
        }

        //Scenario: Constructing a ray through the center of the canvas
        //  Given c ← camera(201, 101, π/2)
        //  When r ← ray_for_pixel(c, 100, 50)
        //  Then r.origin = point(0, 0, 0)
        //    And r.direction = vector(0, 0, -1)
        [Fact]
        public void ShouldCreateRayThroughCenterOfCanvas()
        {
            var c = new Camera(CreateMockBuffer(201, 101), PI / 2);
            var r = c.CreateRayForPixel(100, 50);
            AssertActualEqualToExpected(r.Origin, CreatePoint(0, 0, 0));
            AssertActualEqualToExpected(r.Direction, CreateVector(0, 0, -1));
        }

        //Scenario: Constructing a ray through a corner of the canvas
        //  Given c ← camera(201, 101, π/2)
        //  When r ← ray_for_pixel(c, 0, 0)
        //  Then r.origin = point(0, 0, 0)
        //    And r.direction = vector(0.66519, 0.33259, -0.66851)
        [Fact]
        public void ShouldCreateRayThroughCornerOfCanvas()
        {
            var c = new Camera(CreateMockBuffer(201, 101), PI / 2);
            var r = c.CreateRayForPixel(0, 0);
            AssertActualEqualToExpected(r.Origin, CreatePoint(0, 0, 0));
            AssertActualEqualToExpected(r.Direction, CreateVector(0.66519f, 0.33259f, -0.66851f));
        }

        //Scenario: Constructing a ray when the camera is transformed
        //  Given c ← camera(201, 101, π/2)
        //  When c.transform ← rotation_y(π/4) * translation(0, -2, 5)
        //    And r ← ray_for_pixel(c, 100, 50)
        //  Then r.origin = point(0, 2, -5)
        //    And r.direction = vector(√2/2, 0, -√2/2)
        [Fact]
        public void ShouldCreateRayWhenCameraIsTransformed()
        {
            var c = new Camera(CreateMockBuffer(201, 101), PI / 2)
            {
                Transform = CreateTranslation(0, -2, 5) * CreateRotationY(PI / 4),
            };
            var r = c.CreateRayForPixel(100, 50);
            AssertActualEqualToExpected(r.Origin, CreatePoint(0, 2, -5));
            AssertActualEqualToExpected(r.Direction, CreateVector(Sqrt(2) / 2, 0, -Sqrt(2) / 2));
        }

        //Scenario: Rendering a world with a camera
        //  Given w ← default_world()
        //    And c ← camera(11, 11, π/2)
        //    And from ← point(0, 0, -5)
        //    And to ← point(0, 0, 0)
        //    And up ← vector(0, 1, 0)
        //    And c.transform ← view_transform(from, to, up)
        //  When image ← render(c, w)
        //  Then pixel_at(image, 5, 5) = color(0.38066, 0.47583, 0.2855)

        // Naw, I'm not writing this test.  My framework differs too much from the book
    }
}
