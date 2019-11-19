using System;
using System.Drawing;
using System.Numerics;
using Colourful;
using Colourful.Conversion;
using Microsoft.Xna.Framework.Graphics;
using MonoColor = Microsoft.Xna.Framework.Color;

namespace RayTracer.Core.Utilities
{
    public sealed class ScreenBuffer : IOutputBuffer
    {
        readonly ColourfulConverter _converter = new ColourfulConverter { TargetRGBWorkingSpace = RGBWorkingSpaces.sRGB };
        readonly MonoColor[] _buffer;

        public Size Dimensions { get; }
        public int Width => Dimensions.Width;
        public int Height => Dimensions.Height;

        public MonoColor this[Point p] => _buffer[p.Y * Width + p.X];
        public MonoColor this[int x, int y] => _buffer[y * Width + x];

        public void SetPixel(int x, int y, Vector4 color)
        {
            var clampedColor = Vector4.Clamp(color, Vector4.Zero, Vector4.One);
            var linearRgbColor = new LinearRGBColor(clampedColor.X, clampedColor.Y, clampedColor.Z);
            var sRgbColor = _converter.ToRGB(linearRgbColor);

            SetPixel(x,y,new MonoColor((float)sRgbColor.R, (float)sRgbColor.G, (float)sRgbColor.B));
        }

        public void SetPixel(int x, int y, MonoColor c)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                _buffer[y * Width + x] = c;
            }
        }

        public void AddPixel(int x, int y, MonoColor c)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                ref MonoColor current = ref _buffer[y * Width + x];

                current = new MonoColor(
                    Math.Min(current.R + c.R, 255),
                    Math.Min(current.G + c.G, 255),
                    Math.Min(current.B + c.B, 255));
            }
        }

        public ScreenBuffer(Size size)
        {
            Dimensions = size;
            _buffer = new MonoColor[size.Area()];
        }
        
        public void CopyToTexture(Texture2D texture) => texture.SetData(_buffer);
        public void Clear() => Array.Clear(_buffer, 0, _buffer.Length);
        public void Clear(Rectangle area)
        {
            for (int row = 0; row < area.Height; row++)
            {
                Array.Clear(_buffer, (row + area.Y) * Width + area.X, area.Width);
            }
        }

        public void CopyFrom(MonoColor[] texture, Point textureSize, Point destination)
        {
            var xMargin = Width - destination.X;
            var xToCopy = Math.Min(xMargin, textureSize.X);

            var yMargin = Height - destination.Y;
            var yToCopy = Math.Min(yMargin, textureSize.Y);

            for (int y = 0; y < yToCopy; y++)
            {
                Array.Copy(
                    sourceArray: texture, sourceIndex: y * textureSize.X,
                    destinationArray: _buffer, destinationIndex: (destination.Y + y) * Width + destination.X,
                    length: xToCopy);
            }
        }
    }
}
