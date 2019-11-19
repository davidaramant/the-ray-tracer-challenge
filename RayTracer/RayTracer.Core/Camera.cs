using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using RayTracer.Core.Utilities;
using static System.MathF;
using static RayTracer.Core.Tuples;
using static System.Numerics.Matrix4x4;
using static System.Numerics.Vector4;

namespace RayTracer.Core
{
    public sealed class Camera
    {
        private IOutputBuffer _output;
        public Size Dimensions => _output.Dimensions;
        public float FieldOfView { get; }
        private Matrix4x4 _transform = Identity;
        private Matrix4x4 _inverseTransform = Identity;
        public Matrix4x4 Transform
        {
            get => _transform;
            set
            {
                _transform = value;
                Invert(_transform, out _inverseTransform);
            }
        }

        public float PixelSize { get; private set; }

        private float _halfWidth;
        private float _halfHeight;

        public Camera(IOutputBuffer output, float fieldOfView)
        {
            FieldOfView = fieldOfView;
            _output = output;
            OutputDimensionsUpdated();
        }

        public void UpdateOutputBuffer(IOutputBuffer output)
        {
            _output = output;
            OutputDimensionsUpdated();
        }

        private void OutputDimensionsUpdated()
        {
            var halfView = Tan(FieldOfView / 2);
            var aspectRatio = (float)Dimensions.Width / Dimensions.Height;

            if (aspectRatio >= 1)
            {
                _halfWidth = halfView;
                _halfHeight = halfView / aspectRatio;
            }
            else
            {
                _halfWidth = halfView * aspectRatio;
                _halfHeight = halfView;
            }
            PixelSize = (_halfWidth * 2) / Dimensions.Width;
        }

        public Ray CreateRayForPixel(int pixelX, int pixelY)
        {
            var xOffset = (pixelX + 0.5f) * PixelSize;
            var yOffset = (pixelY + 0.5f) * PixelSize;

            var worldX = _halfWidth - xOffset;
            var worldY = _halfHeight - yOffset;

            var pixel = Vector4.Transform(CreatePoint(worldX, worldY, -1), _inverseTransform);
            var origin = Vector4.Transform(CreatePoint(0, 0, 0), _inverseTransform);
            var direction = Normalize(pixel - origin);

            return new Ray(origin, direction);
        }
    }
}
