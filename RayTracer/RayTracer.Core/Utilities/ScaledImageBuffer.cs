using System;
using System.Drawing;
using System.Numerics;

namespace RayTracer.Core.Utilities
{
    public sealed class ScaledImageBuffer : IOutputBuffer
    {
        public RenderScale Scale { get; private set; }
        private readonly ImageBuffer _buffer;
        public Size ActualDimensions => _buffer.Dimensions;
        public Size Dimensions { get; private set; }
        public int Stride => _buffer.Stride;
        public byte[] GetBuffer() => _buffer.GetBuffer();

        public ScaledImageBuffer(int width, int height, RenderScale scale = RenderScale.Normal)
            : this(new Size(width, height), scale)
        {
        }

        public ScaledImageBuffer(Size outputDimensions, RenderScale scale = RenderScale.Normal)
        {
            Scale = scale;
            _buffer = new ImageBuffer(outputDimensions);
            UpdateRenderingDimensions();
        }

        public event EventHandler DimensionsUpdated;

        public void SetPixel(int x, int y, Vector4 color)
        {
            if (Scale == RenderScale.Normal)
            {
                _buffer.SetPixel(x, y, color);
                return;
            }
            var scale = (int)Scale;
            var startX = scale * x;
            var startY = scale * y;
            for (int yOffset = 0; yOffset < scale; yOffset++)
            {
                for (int xOffset = 0; xOffset < scale; xOffset++)
                {
                    _buffer.SetPixel(startX + xOffset, startY + yOffset, color);
                }
            }
        }

        private void UpdateRenderingDimensions()
        {
            var divisor = (int)Scale;
            Dimensions = new Size(ActualDimensions.Width / divisor, ActualDimensions.Height / divisor);
            DimensionsUpdated?.Invoke(this, EventArgs.Empty);
        }

        public void SetScale(RenderScale scale)
        {
            Scale = scale;
            UpdateRenderingDimensions();
        }

        public void DecreaseQuality()
        {
            if (Scale != RenderScale.Eighth)
            {
                Scale = (RenderScale)((int)Scale * 2);
                UpdateRenderingDimensions();
            }
        }

        public void IncreaseQuality()
        {
            if (Scale != RenderScale.Normal)
            {
                Scale = (RenderScale)((int)Scale / 2);
                UpdateRenderingDimensions();
            }
        }
    }
}
