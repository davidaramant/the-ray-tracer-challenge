using System;
using System.Drawing;
using System.Numerics;
using static System.Numerics.Vector4;
using static RayTracer.Tuples;
using static RayTracer.Graphics;

namespace RayTracer.WpfGui
{
    public sealed class GameState
    {
        const int SquareRadius = 4;

        private readonly DrawPixel _drawPixel;
        private readonly int _canvasWidth;
        private readonly int _canvasHeight;
        private Vector4 _centerPosition;
        

        public GameState(DrawPixel drawPixel, int canvasWidth, int canvasHeight)
        {
            _drawPixel = drawPixel;
            _canvasWidth = canvasWidth;
            _canvasHeight = canvasHeight;

            _centerPosition = MakePoint(canvasHeight / 2f, canvasHeight / 2f, 0);
        }

        public void Update(KeysPressed input, TimeSpan elapsed)
        {
            const float pixelsPerMs = 0.5f;

            float movement = pixelsPerMs * (float)elapsed.TotalMilliseconds;

            var delta = MakeVector(0, 0, 0);
            if (input.Up)
                delta.Y -= movement;
            else if (input.Down)
                delta.Y += movement;
            if (input.Left)
                delta.X -= movement;
            else if (input.Right)
                delta.X += movement;

            _centerPosition += delta;
            _centerPosition.X = Math.Max(SquareRadius, Math.Min(_canvasWidth - SquareRadius, _centerPosition.X));
            _centerPosition.Y = Math.Max(SquareRadius, Math.Min(_canvasHeight - SquareRadius, _centerPosition.Y));
        }

        public void Render()
        {
            var pixelCenter = MakeVector(0.5f, 0.5f, 0);
            var color = MakeColor(0, 0, 1);

            for (int yOffset = -SquareRadius; yOffset < SquareRadius; yOffset++)
            {
                for (int xOffset = -SquareRadius; xOffset < SquareRadius; xOffset++)
                {
                    var offset = MakeVector(xOffset, yOffset, 0);
                    var screenCoords = _centerPosition + offset + pixelCenter;

                    _drawPixel((int)screenCoords.X, (int)screenCoords.Y, color);
                }
            }
        }
    }
}