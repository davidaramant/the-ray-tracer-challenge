using System;
using System.Diagnostics;
using System.Numerics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RayTracer.Core;

namespace RayTracer.WpfGui
{
    public partial class MainWindow : Window
    {
        private readonly WriteableBitmap _canvas;
        private readonly GameState _gameState;
        private readonly KeysPressed _input = new KeysPressed();
        private readonly Stopwatch _timer = new Stopwatch();
        private TimeSpan _lastElapsed;

        public MainWindow()
        {
            InitializeComponent();

            RenderOptions.SetBitmapScalingMode(OutputImage, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetEdgeMode(OutputImage, EdgeMode.Aliased);

            var width = (int)OutputImage.Width;
            var height = (int)OutputImage.Height;
            _canvas = new WriteableBitmap(
                width,
                height,
                96,
                96,
                PixelFormats.Bgr32,
                null);
            _gameState = new GameState(DrawPixel, width, height);

            OutputImage.Source = _canvas;

            OutputImage.Stretch = Stretch.None;
            OutputImage.HorizontalAlignment = HorizontalAlignment.Left;
            OutputImage.VerticalAlignment = VerticalAlignment.Top;

            _canvas.Clear(Colors.White);

            //CompositionTarget.Rendering += GameLoop;
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;

            _timer.Start();
            _lastElapsed = _timer.Elapsed;

            _canvas.Clear(Colors.Black);

            this.Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, EventArgs e)
        {
            await Renderer.TraceScene(new System.Drawing.Size((int)OutputImage.Width, (int)OutputImage.Height), DrawPixel);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // This method avoids any branching
            _input.Up |= e.Key == Key.Up;
            _input.Down |= e.Key == Key.Down;
            _input.Left |= e.Key == Key.Left;
            _input.Right |= e.Key == Key.Right;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            // This method avoids any branching
            _input.Up &= e.Key != Key.Up;
            _input.Down &= e.Key != Key.Down;
            _input.Left &= e.Key != Key.Left;
            _input.Right &= e.Key != Key.Right;
        }

        void GameLoop(object sender, EventArgs e)
        {
            var timeStamp = _timer.Elapsed;
            _gameState.Update(_input, timeStamp - _lastElapsed);
            _lastElapsed = timeStamp;

            _canvas.Clear(Colors.White);

            try
            {
                _canvas.Lock();
                _gameState.Render();
            }
            finally
            {
                _canvas.Unlock();
            }
        }

        private void DrawPixel(int x, int y, Vector4 color)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                using (_canvas.GetBitmapContext())
                {
                    _canvas.SetPixel(x, y, (byte) (255 * color.X), (byte) (255 * color.Y), (byte) (255 * color.Z));
                }
            });
        }
    }
}
