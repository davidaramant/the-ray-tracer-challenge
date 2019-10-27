﻿using System;
using System.Numerics;
using static RayTracer.Core.Graphics;
using static RayTracer.Core.Tuples;
using static System.Numerics.Vector4;
using static System.MathF;

namespace RayTracer.Core
{
    public sealed class Material
    {
        public Vector4 Color { get; set; } = CreateColor(1, 1, 1);
        public float Ambient { get; set; } = 0.1f;
        public float Diffuse { get; set; } = 0.9f;
        public float Specular { get; set; } = 0.9f;
        public float Shininess { get; set; } = 200;

        // TODO: Embed this in properties for debug mode
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

        public Vector4 GetLight(PointLight light, Vector4 position, Vector4 eyeVector, Vector4 normal)
        {
#if DEBUG
            if (!position.IsPoint()) throw new ArgumentException("Position must be point", nameof(position));
            if (!eyeVector.IsVector()) throw new ArgumentException("Eye must be vector", nameof(eyeVector));
            if (!normal.IsVector()) throw new ArgumentException("Normal must be vector", nameof(normal));
#endif

            var effectiveColor = Color * light.Intensity;
            var lightVector = Normalize(light.Position - position);
            var ambientContribution = effectiveColor * Ambient;
            var diffuseContribution = CreateColor(0, 0, 0);
            var specularContribution = CreateColor(0, 0, 0);

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

            return ambientContribution + diffuseContribution + specularContribution;
        }
    }
}
