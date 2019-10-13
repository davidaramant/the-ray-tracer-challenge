using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RayTracer.WpfGui
{
    public static class Extensions
    {
        public static int ToARGBInt(this Color color)
        {
            int colorData = color.R << 16;
            colorData |= color.G << 8;
            colorData |= color.B;
            return colorData;
        }

        public static void Clear(this WriteableBitmap canvas, Color color)
        {
            var sourceRect = new Int32Rect(0, 0, canvas.PixelWidth, canvas.PixelHeight);
            var colorData = new int[sourceRect.Height * sourceRect.Width];
            Array.Fill(colorData, color.ToARGBInt());

            byte[] colorDataBytes = new byte[colorData.Length * sizeof(int)];
            Buffer.BlockCopy(colorData, 0, colorDataBytes, 0, colorDataBytes.Length);

            canvas.WritePixels(sourceRect, colorDataBytes, 4 * canvas.PixelWidth, 0);
        }
    }
}
