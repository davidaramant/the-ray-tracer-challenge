using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;
using System.Runtime.InteropServices;
using Colourful;
using Colourful.Conversion;

namespace RayTracer.Core.Utilities
{
    public static class FloatExtensions
    {
        public static double CropRange(this double d, double min, double max)
        {
            if (d < 0) return min;
            if (d > max) return max;
            return d;
        }
    }

    public sealed class FastImage
    {

        public const PixelFormat Format = PixelFormat.Format32bppRgb;
        readonly int _pixelSizeInBytes = Image.GetPixelFormatSize(Format) / 8;
        readonly byte[] _pixelBuffer;

        public int Width { get; }
        public int Height { get; }
        public Size Dimensions => new Size(Width, Height);
        public int PixelCount => Width * Height;
        public int Stride { get; }

        public FastImage(Size resolution) : this(resolution.Width, resolution.Height)
        {
        }

        public FastImage(int width, int height)
        {
            Width = width;
            Height = height;
            Stride = width * _pixelSizeInBytes;
            _pixelBuffer = new byte[Stride * height];
        }

        public byte[] GetBuffer() => _pixelBuffer;

        public void SetPixel(int x, int y, Vector4 color)
        {
            var index = y * Stride + x * _pixelSizeInBytes;
            var clampedColor = Vector4.Clamp(color, Vector4.Zero, Vector4.One);

            var linearRgbColor = new LinearRGBColor(clampedColor.X, clampedColor.Y, clampedColor.Z);
            var converter = new ColourfulConverter { TargetRGBWorkingSpace = RGBWorkingSpaces.sRGB };
            var sRgbColor = converter.ToRGB(linearRgbColor);

            var r = (byte)Math.Round(sRgbColor.R * 255).CropRange(0, 255);
            var g = (byte)Math.Round(sRgbColor.G * 255).CropRange(0, 255);
            var b = (byte)Math.Round(sRgbColor.B * 255).CropRange(0, 255);
            var output = Color.FromArgb(r, g, b);

            SetPixelFromIndex(index, output);
        }

        public void SetPixel(int x, int y, Color color)
        {
            var index = y * Stride + x * _pixelSizeInBytes;
            SetPixelFromIndex(index, color);
        }

        public void SetPixel(int pixelIndex, Color color)
        {
            var index = pixelIndex * _pixelSizeInBytes;
            SetPixelFromIndex(index, color);
        }

        private void SetPixelFromIndex(int index, Color color)
        {
            _pixelBuffer[index] = color.B;
            _pixelBuffer[index + 1] = color.G;
            _pixelBuffer[index + 2] = color.R;
        }

        public void Save(string filePath)
        {
            using var bmp = new Bitmap(Width, Height, Format);
            var bmpData = bmp.LockBits(
                new Rectangle(0, 0, Width, Height),
                ImageLockMode.WriteOnly,
                bmp.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Copy the RGB values back to the bitmap
            Marshal.Copy(_pixelBuffer, 0, ptr, _pixelBuffer.Length);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);

            bmp.Save(filePath);
        }
    }
}
