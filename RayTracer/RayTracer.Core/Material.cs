using System;
using System.Numerics;
using RayTracer.Core.Patterns;
using static RayTracer.Core.Tuples;
using static System.Numerics.Vector4;
using static System.MathF;

namespace RayTracer.Core
{
    public sealed class Material : IEquatable<Material>
    {
        public Vector4 Color { get; set; } = VColor.White;
        public float Ambient { get; set; } = 0.1f;
        public float Diffuse { get; set; } = 0.9f;
        public float Specular { get; set; } = 0.9f;
        public float Shininess { get; set; } = 200;
        public IPattern Pattern { get; set; } = EmptyPattern.Instance;

        public void Validate()
        {
            if (!Color.IsColor())
                throw new ArgumentException("Color is not a color", nameof(Color));
            if (!Ambient.IsInUnitRange())
                throw new ArgumentOutOfRangeException(nameof(Ambient));
            if (!Diffuse.IsInUnitRange())
                throw new ArgumentOutOfRangeException(nameof(Diffuse));
            if (!Specular.IsInUnitRange())
                throw new ArgumentOutOfRangeException(nameof(Specular));
            if (!Shininess.IsPositive())
                throw new ArgumentOutOfRangeException(nameof(Shininess));
        }

        public Vector4 ComputeColor(PointLight light, Vector4 position, Vector4 eyeVector, Vector4 normal, bool inShadow = false)
        {
#if DEBUG
            Validate();
            if (!position.IsPoint()) throw new ArgumentException("Position must be point", nameof(position));
            if (!eyeVector.IsVector()) throw new ArgumentException("Eye must be vector", nameof(eyeVector));
            if (!normal.IsVector()) throw new ArgumentException("Normal must be vector", nameof(normal));
#endif
            var color = Pattern == EmptyPattern.Instance ? Color : Pattern.GetColorAt(position);

            var effectiveColor = color * light.Intensity;
            var lightVector = Normalize(light.Position - position);
            var ambientContribution = effectiveColor * Ambient;
            var diffuseContribution = VColor.LinearRGB(0, 0, 0);
            var specularContribution = VColor.LinearRGB(0, 0, 0);

            if (!inShadow)
            {
                var lightDotNormal = Dot(lightVector, normal);
                if (lightDotNormal > 0)
                {
                    diffuseContribution = effectiveColor * Diffuse * lightDotNormal;
                    var reflectVector = Reflect(-lightVector, normal);
                    var reflectDotEye = Dot(reflectVector, eyeVector);

                    if (reflectDotEye > 0)
                    {
                        var factor = Pow(reflectDotEye, Shininess);
                        specularContribution = light.Intensity * Specular * factor;
                    }
                }
            }

            return ambientContribution + diffuseContribution + specularContribution;
        }

        #region Equality
        public bool Equals(Material other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return
                Ambient.IsEquivalentTo(other.Ambient) &&
                Color.IsEquivalentTo(other.Color) &&
                Diffuse.IsEquivalentTo(other.Diffuse) &&
                Shininess.IsEquivalentTo(other.Shininess) &&
                Specular.IsEquivalentTo(other.Specular);
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is Material other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Ambient.GetHashCode();
                hashCode = (hashCode * 397) ^ Color.Truncate().GetHashCode();
                hashCode = (hashCode * 397) ^ Diffuse.Truncate().GetHashCode();
                hashCode = (hashCode * 397) ^ Shininess.Truncate().GetHashCode();
                hashCode = (hashCode * 397) ^ Specular.Truncate().GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Material left, Material right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Material left, Material right)
        {
            return !Equals(left, right);
        }
        #endregion
    }
}
