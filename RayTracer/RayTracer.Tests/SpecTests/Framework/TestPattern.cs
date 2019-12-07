// Copyright (c) 2019, David Aramant
// Distributed under the 3-clause BSD license.  For full terms see the file LICENSE. 

using System.Numerics;
using RayTracer.Core;
using RayTracer.Core.Patterns;

namespace RayTracer.Tests.SpecTests.Framework
{
    public sealed class TestPattern : Pattern
    {
        public override Vector4 GetColorAt(Vector4 point) => VColor.LinearRGB(point.X, point.Y, point.Z);
    }
}