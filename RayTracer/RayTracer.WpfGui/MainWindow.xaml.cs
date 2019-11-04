using System;
using System.Diagnostics;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RayTracer.Core;
using RayTracer.Core.Utilities;

namespace RayTracer.WpfGui
{
    public partial class MainWindow : Window
    {
        private readonly ScaledImageBuffer _canvas;
        private readonly WriteableBitmap _guiCanvas;
        private readonly World _world = TestScene.CreateTestWorld();
        private readonly Camera _camera;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private Task _renderingTask;
        private readonly Stopwatch _timer = new Stopwatch();
        private TimeSpan _lastElapsed;

        // world units per ms
        private const float MovementSpeed = 0.0001f;
        private const float RotationSpeed = 0.00005f;


        public MainWindow()
        {
            InitializeComponent();

            RenderOptions.SetBitmapScalingMode(OutputImage, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetEdgeMode(OutputImage, EdgeMode.Aliased);

            var width = (int)OutputImage.Width;
            var height = (int)OutputImage.Height;
            _guiCanvas = new WriteableBitmap(
                width,
                height,
                96,
                96,
                PixelFormats.Bgr32,
                null);
            _canvas = new ScaledImageBuffer(width, height);

            OutputImage.Source = _guiCanvas;

            OutputImage.Stretch = Stretch.None;
            OutputImage.HorizontalAlignment = HorizontalAlignment.Left;
            OutputImage.VerticalAlignment = VerticalAlignment.Top;

            _guiCanvas.Clear(Colors.Black);

            _camera = TestScene.CreateCamera(_canvas);

            CompositionTarget.Rendering += GameLoop;
            KeyDown += MainWindow_KeyDown;

            this.Loaded += (s, e) =>
            {
                _timer.Start();
                StartRenderingScene();
            };
        }

        private async void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.OemOpenBrackets && !_canvas.Scale.IsMinimumQuality())
            {
                await CancelRendering();
                _canvas.DecreaseQuality();
                StartRenderingScene();
            }
            else if (e.Key == Key.OemCloseBrackets && !_canvas.Scale.IsMaximumQuality())
            {
                await CancelRendering();
                _canvas.IncreaseQuality();
                StartRenderingScene();
            }
        }

        private async Task CancelRendering()
        {
            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
            }

            await _renderingTask;
        }

        private void StartRenderingScene()
        {
            _renderingTask = RenderSceneAsync();
        }

        private async Task RenderSceneAsync()
        {
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
            await Renderer.TraceScene(_camera, _world, _canvas.SetPixel, _cancellationTokenSource.Token);
        }

        async void GameLoop(object sender, EventArgs e)
        {
            var timeStamp = _timer.Elapsed;
            await UpdateCameraPosition(timeStamp - _lastElapsed);
            _lastElapsed = timeStamp;

            _guiCanvas.WritePixels(new Int32Rect(0, 0, _canvas.ActualDimensions.Width, _canvas.ActualDimensions.Height),
                _canvas.GetBuffer(),
                _canvas.Stride, 0);
        }

        async Task UpdateCameraPosition(TimeSpan frameTime)
        {
            float inlineDistance = MovementSpeed * (float)frameTime.TotalMilliseconds;
            float rotationAmount = RotationSpeed * (float) frameTime.TotalMilliseconds;
            bool moved = false;

            Matrix4x4 movementMatrix = Matrix4x4.Identity;

            if (Keyboard.IsKeyDown(Key.Up))
            {
                moved = true;
                movementMatrix *= Matrix4x4.CreateTranslation(0, 0, inlineDistance);
            }
            else if (Keyboard.IsKeyDown(Key.Down))
            {
                moved = true;
                movementMatrix *= Matrix4x4.CreateTranslation(0, 0, -inlineDistance);
            }

            if (Keyboard.IsKeyDown(Key.Right))
            {
                moved = true;
                movementMatrix *= Matrix4x4.CreateRotationY(-rotationAmount);
            }
            else if (Keyboard.IsKeyDown(Key.Left))
            {
                moved = true;
                movementMatrix *= Matrix4x4.CreateRotationY(rotationAmount);
            }

            if (moved)
            {
                await CancelRendering();
                _camera.Transform *= movementMatrix;
                StartRenderingScene();
            }
        }
    }
}
