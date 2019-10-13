using System;
using System.Drawing;

namespace RayTracer.WpfGui
{
    public delegate void DrawPixel(int x, int y, byte r, byte g, byte b);

    public sealed class GameState
    {
        // Distance from center to side (perpendicularly)
        const int SquareRadius = 4;

        private readonly DrawPixel _drawPixel;
        private readonly int _canvasWidth;
        private readonly int _canvasHeight;
        private Point _centerPosition;

        public GameState(DrawPixel drawPixel, int canvasWidth, int canvasHeight)
        {
            _drawPixel = drawPixel;
            _canvasWidth = canvasWidth;
            _canvasHeight = canvasHeight;

            _centerPosition = new Point(canvasWidth / 2, canvasHeight / 2);
        }

        public void Update(KeysPressed input, TimeSpan elapsed)
        {
            const double pixelsPerMs = 0.5;

            int movement = (int)(pixelsPerMs * elapsed.TotalMilliseconds);

            Size delta = new Size();
            if (input.Up)
                delta.Height -= movement;
            else if (input.Down)
                delta.Height += movement;
            if (input.Left)
                delta.Width -= movement;
            else if (input.Right)
                delta.Width += movement;

            _centerPosition += delta;
            _centerPosition.X = Math.Max(SquareRadius, Math.Min(_canvasWidth - SquareRadius, _centerPosition.X));
            _centerPosition.Y = Math.Max(SquareRadius, Math.Min(_canvasHeight - SquareRadius, _centerPosition.Y));
        }

        public void Render()
        {
            for (int yOffset = -SquareRadius; yOffset < SquareRadius; yOffset++)
            {
                for (int xOffset = -SquareRadius; xOffset < SquareRadius; xOffset++)
                {
                    _drawPixel(_centerPosition.X + xOffset, _centerPosition.Y + yOffset, 0, 0, 0);
                }
            }
        }
    }
}