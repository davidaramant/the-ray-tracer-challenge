using System.Numerics;
using NUnit.Framework;
using RayTracer.Core;
using RayTracer.Core.Shapes;
using static System.MathF;
using static System.Numerics.Matrix4x4;
using static System.Numerics.Vector4;
using static RayTracer.Core.Tuples;
using static RayTracer.Tests.SpecTests.Framework.Comparisons;

namespace RayTracer.Tests.SpecTests
{
    /// <summary>
    /// spheres.feature
    /// </summary>
    [TestFixture]
    public class SphereTests
    {
        //Scenario: A ray intersects a sphere at two points
        //  Given r ← ray(point(0, 0, -5), vector(0, 0, 1))
        //    And s ← sphere()
        //  When xs ← intersect(s, r)
        //  Then xs.count = 2
        //    And xs[0] = 4.0
        //    And xs[1] = 6.0
        //
        //Scenario: Intersect sets the object on the intersection
        //  Given r ← ray(point(0, 0, -5), vector(0, 0, 1))
        //    And s ← sphere()
        //  When xs ← intersect(s, r)
        //  Then xs.count = 2
        //    And xs[0].object = s
        //    And xs[1].object = s
        [Test]
        public void ShouldComputeIntersectionsOfRayToSphere()
        {
            var r = CreateRay(CreatePoint(0, 0, -5), CreateVector(0, 0, 1));
            var s = new Sphere();
            var xs = s.Intersect(r);
            Assert.That(xs, Is.EqualTo(new[] { new Intersection(4f, s), new Intersection(6f, s) }));
        }

        //Scenario: A ray intersects a sphere at a tangent
        //  Given r ← ray(point(0, 1, -5), vector(0, 0, 1))
        //    And s ← sphere()
        //  When xs ← intersect(s, r)
        //  Then xs.count = 2
        //    And xs[0] = 5.0
        //    And xs[1] = 5.0
        [Test]
        public void ShouldComputeIntersectionsWhenRayIsTangentToSphere()
        {
            var r = CreateRay(CreatePoint(0, 1, -5), CreateVector(0, 0, 1));
            var s = new Sphere();
            var xs = s.Intersect(r);
            Assert.That(xs, Is.EqualTo(new[] { new Intersection(5f, s), new Intersection(5f, s) }));
        }

        //Scenario: A ray misses a sphere
        //  Given r ← ray(point(0, 2, -5), vector(0, 0, 1))
        //    And s ← sphere()
        //  When xs ← intersect(s, r)
        //  Then xs.count = 0
        [Test]
        public void ShouldComputeIntersectionsWhenRayMissesSphere()
        {
            var r = CreateRay(CreatePoint(0, 2, -5), CreateVector(0, 0, 1));
            var s = new Sphere();
            var xs = s.Intersect(r);
            Assert.That(xs, Is.Empty);
        }

        //Scenario: A ray originates inside a sphere
        //  Given r ← ray(point(0, 0, 0), vector(0, 0, 1))
        //    And s ← sphere()
        //  When xs ← intersect(s, r)
        //  Then xs.count = 2
        //    And xs[0] = -1.0
        //    And xs[1] = 1.0
        [Test]
        public void ShouldComputeIntersectionsWhenRayIsInsideSphere()
        {
            var r = CreateRay(CreatePoint(0, 0, 0), CreateVector(0, 0, 1));
            var s = new Sphere();
            var xs = s.Intersect(r);
            Assert.That(xs, Is.EqualTo(new[] { new Intersection(-1f, s), new Intersection(1f, s) }));
        }

        //Scenario: A sphere is behind a ray
        //  Given r ← ray(point(0, 0, 5), vector(0, 0, 1))
        //    And s ← sphere()
        //  When xs ← intersect(s, r)
        //  Then xs.count = 2
        //    And xs[0] = -6.0
        //    And xs[1] = -4.0
        [Test]
        public void ShouldComputeIntersectionsWhenSphereIsBehindRay()
        {
            var r = CreateRay(CreatePoint(0, 0, 5), CreateVector(0, 0, 1));
            var s = new Sphere();
            var xs = s.Intersect(r);
            Assert.That(xs, Is.EqualTo(new[] { new Intersection(-6f, s), new Intersection(-4f, s) }));
        }

        //Scenario: A sphere's default transformation
        //  Given s ← sphere()
        //  Then s.transform = identity_matrix
        [Test]
        public void ShouldHaveDefaultTransformation()
        {
            var s = new Sphere();
            AssertActualEqualToExpected(s.Transform, Identity);
        }

        //Scenario: Changing a sphere's transformation
        //  Given s ← sphere()
        //    And t ← translation(2, 3, 4)
        //  When set_transform(s, t)
        //  Then s.transform = t
        [Test]
        public void ShouldSetTransformation()
        {
            var s = new Sphere { Transform = CreateTranslation(2, 3, 4) };
            AssertActualEqualToExpected(s.Transform, CreateTranslation(2, 3, 4));
        }

        //Scenario: Intersecting a scaled sphere with a ray
        //  Given r ← ray(point(0, 0, -5), vector(0, 0, 1))
        //    And s ← sphere()
        //  When set_transform(s, scaling(2, 2, 2))
        //    And xs ← intersect(s, r)
        //  Then xs.count = 2
        //    And xs[0].t = 3
        //    And xs[1].t = 7
        [Test]
        public void ShouldIntersectScaledSphereWithARay()
        {
            var r = CreateRay(CreatePoint(0, 0, -5), CreateVector(0, 0, 1));
            var s = new Sphere { Transform = CreateScale(2, 2, 2) };
            var xs = s.Intersect(r);
            Assert.That(xs, Is.EqualTo(new[] { new Intersection(3f, s), new Intersection(7f, s) }));
        }

        //Scenario: Intersecting a translated sphere with a ray
        //  Given r ← ray(point(0, 0, -5), vector(0, 0, 1))
        //    And s ← sphere()
        //  When set_transform(s, translation(5, 0, 0))
        //    And xs ← intersect(s, r)
        //  Then xs.count = 0
        [Test]
        public void ShouldIntersectTranslatedSphereWithARay()
        {
            var r = CreateRay(CreatePoint(0, 0, -5), CreateVector(0, 0, 1));
            var s = new Sphere { Transform = CreateTranslation(5, 0, 0) };
            var xs = s.Intersect(r);
            Assert.That(xs, Is.Empty);
        }

        //Scenario: The normal on a sphere at a point on the x axis
        //  Given s ← sphere()
        //  When n ← normal_at(s, point(1, 0, 0))
        //  Then n = vector(1, 0, 0)
        //
        //Scenario: The normal on a sphere at a point on the y axis
        //  Given s ← sphere()
        //  When n ← normal_at(s, point(0, 1, 0))
        //  Then n = vector(0, 1, 0)
        //
        //Scenario: The normal on a sphere at a point on the z axis
        //  Given s ← sphere()
        //  When n ← normal_at(s, point(0, 0, 1))
        //  Then n = vector(0, 0, 1)
        [TestCase(1, 0, 0)]
        [TestCase(0, 1, 0)]
        [TestCase(0, 0, 1)]
        public void ShouldGetNormalAtAxisPoint(float x, float y, float z)
        {
            var s = new Sphere();
            var n = s.GetNormalAt(CreatePoint(x, y, z));
            AssertActualEqualToExpected(n, CreateVector(x, y, z));
        }

        //Scenario: The normal on a sphere at a nonaxial point
        //  Given s ← sphere()
        //  When n ← normal_at(s, point(√3/3, √3/3, √3/3))
        //  Then n = vector(√3/3, √3/3, √3/3)
        [Test]
        public void ShouldGetNormalAtNonAxialPoint()
        {
            var s = new Sphere();
            var n = s.GetNormalAt(CreatePoint(Sqrt(3) / 3, Sqrt(3) / 3, Sqrt(3) / 3));
            AssertActualEqualToExpected(n, CreateVector(Sqrt(3) / 3, Sqrt(3) / 3, Sqrt(3) / 3));
        }

        //Scenario: The normal is a normalized vector
        //  Given s ← sphere()
        //  When n ← normal_at(s, point(√3/3, √3/3, √3/3))
        //  Then n = normalize(n)
        [Test]
        public void ShouldReturnNormalThatIsNormalized()
        {
            var s = new Sphere();
            var n = s.GetNormalAt(CreatePoint(Sqrt(3) / 3, Sqrt(3) / 3, Sqrt(3) / 3));
            AssertActualEqualToExpected(n, Normalize(n));
        }

        //Scenario: Computing the normal on a translated sphere
        //  Given s ← sphere()
        //    And set_transform(s, translation(0, 1, 0))
        //  When n ← normal_at(s, point(0, 1.70711, -0.70711))
        //  Then n = vector(0, 0.70711, -0.70711)
        [Test]
        public void ShouldComputeNormalOfTranslatedSphere()
        {
            var s = new Sphere { Transform = CreateTranslation(0, 1, 0) };
            var n = s.GetNormalAt(CreatePoint(0, 1.70711f, -0.70711f));
            AssertActualEqualToExpected(n, CreateVector(0, 0.70711f, -0.70711f));
        }

        //Scenario: Computing the normal on a transformed sphere
        //  Given s ← sphere()
        //    And m ← scaling(1, 0.5, 1) * rotation_z(π/5)
        //    And set_transform(s, m)
        //  When n ← normal_at(s, point(0, √2/2, -√2/2))
        //  Then n = vector(0, 0.97014, -0.24254)
        [Test]
        public void ShouldComputeNormalOfTransformedSphere()
        {
            var s = new Sphere { Transform = CreateRotationZ(PI / 5) * CreateScale(1, 0.5f, 1) };
            var n = s.GetNormalAt(CreatePoint(0, Sqrt(2) / 2, -Sqrt(2) / 2));
            AssertActualEqualToExpected(n, CreateVector(0, 0.97014f, -0.24254f));
        }

        //Scenario: A sphere has a default material
        //  Given s ← sphere()
        //  When m ← s.material
        //  Then m = material()
        [Test]
        public void ShouldHaveDefaultMaterial()
        {
            var s = new Sphere();
            AssertActualEqualToExpected(s.Material, new Material());
        }

        //Scenario: A sphere may be assigned a material
        //  Given s ← sphere()
        //    And m ← material()
        //    And m.ambient ← 1
        //  When s.material ← m
        //  Then s.material = m
        [Test]
        public void ShouldSupportAssigningMaterial()
        {
            var m = new Material { Ambient = 1 };
            var s = new Sphere { Material = m };
            AssertActualEqualToExpected(s.Material, m);
        }

        //Scenario: A helper for producing a sphere with a glassy material
        //  Given s ← glass_sphere()
        //  Then s.transform = identity_matrix
        //    And s.material.transparency = 1.0
        //    And s.material.refractive_index = 1.5

    }
}
