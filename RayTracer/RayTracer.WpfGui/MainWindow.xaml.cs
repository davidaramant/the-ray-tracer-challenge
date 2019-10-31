using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using RayTracer.Core;
using RayTracer.Core.Utilities;
using static RayTracer.Core.Tuples;

namespace RayTracer.WpfGui
{
    public partial class MainWindow : Window
    {
        private readonly FastImage _canvas;
        private readonly WriteableBitmap _guiCanvas;
        private readonly World _world = World.CreateTestWorld();
        private readonly Camera _camera;

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
            _canvas = new FastImage(width, height);

            OutputImage.Source = _guiCanvas;

            OutputImage.Stretch = Stretch.None;
            OutputImage.HorizontalAlignment = HorizontalAlignment.Left;
            OutputImage.VerticalAlignment = VerticalAlignment.Top;

            _guiCanvas.Clear(Colors.Black);

            _camera = new Camera(width, height, MathF.PI / 3)
            {
                Transform = CreateViewTransform(
                    from: CreatePoint(0, 1.5f, -5),
                    to: CreatePoint(0, 1, 0),
                    up: CreateVector(0, 1, 0)),
            };

            CompositionTarget.Rendering += GameLoop;

            this.Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, EventArgs e)
        {
            await Renderer.TraceScene(_camera, _world, _canvas.SetPixel);
        }

        void GameLoop(object sender, EventArgs e)
        {
            _guiCanvas.WritePixels(new Int32Rect(0, 0, _canvas.Width, _canvas.Height), _canvas.GetBuffer(), _canvas.Stride, 0);
        }
    }
}
