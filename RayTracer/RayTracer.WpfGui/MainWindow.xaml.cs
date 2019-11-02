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
        private readonly ScaledImageBuffer _canvas;
        private readonly WriteableBitmap _guiCanvas;
        private readonly World _world = TestScene.CreateTestWorld();
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
            _canvas = new ScaledImageBuffer(width, height, RenderScale.Normal);

            OutputImage.Source = _guiCanvas;

            OutputImage.Stretch = Stretch.None;
            OutputImage.HorizontalAlignment = HorizontalAlignment.Left;
            OutputImage.VerticalAlignment = VerticalAlignment.Top;

            _guiCanvas.Clear(Colors.Black);

            _camera = TestScene.CreateCamera(_canvas);

            CompositionTarget.Rendering += GameLoop;

            this.Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, EventArgs e)
        {
            await Renderer.TraceScene(_camera, _world, _canvas.SetPixel);
        }

        void GameLoop(object sender, EventArgs e)
        {
            _guiCanvas.WritePixels(new Int32Rect(0, 0, _canvas.ActualDimensions.Width, _canvas.ActualDimensions.Height), 
                _canvas.GetBuffer(), 
                _canvas.Stride, 0);
        }
    }
}
