using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RayTracer.Core;
using RayTracer.Core.Utilities;
using RayTracer.MonoGameGui.Input;

namespace RayTracer.MonoGameGui
{
    public sealed class InteractiveRayTracer : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _outputTexture;
        private ScreenBuffer _screenBuffer;

        private RenderScale _renderScale = RenderScale.Normal;

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private Task _renderingTask;

        readonly KeyToggles _keyToggles = new KeyToggles();
        readonly ContinuousInputs _continuousInputs = new ContinuousInputs();

        private World _world;
        private Camera _camera;

        private Size CurrentScreenSize => new Size(
            _graphics.PreferredBackBufferWidth,
            _graphics.PreferredBackBufferHeight);

        public InteractiveRayTracer()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 800,
                PreferredBackBufferHeight = 600,
                IsFullScreen = false,
                SynchronizeWithVerticalRetrace = true,
            };
            Content.RootDirectory = "Content";
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += UpdateScreenBufferWithNewSize;
            IsMouseVisible = true;
        }

        private void UpdateScreenBufferWithNewSize(object sender, EventArgs e) =>
            UpdateScreenBuffer(CurrentScreenSize.DivideBy(_renderScale));

        async void UpdateScreenBuffer(Size renderSize)
        {
            if (_screenBuffer.Dimensions != renderSize)
            {
                await CancelRendering();
                _outputTexture.Dispose();
                _outputTexture = new Texture2D(_graphics.GraphicsDevice, width: renderSize.Width, height: renderSize.Height);
                _screenBuffer = new ScreenBuffer(renderSize);
                StartRenderingScene();
            }
        }

        protected override void Initialize()
        {
            TargetElapsedTime = TimeSpan.FromSeconds(1 / 60d);
            base.Initialize();
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
            _camera.UpdateOutputBuffer(_screenBuffer);
            await Renderer.TraceScene(_camera, _world, _screenBuffer.SetPixel, maximumReflections: 5, _cancellationTokenSource.Token);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _outputTexture = new Texture2D(_graphics.GraphicsDevice, width: CurrentScreenSize.Width, height: CurrentScreenSize.Height);
            _screenBuffer = new ScreenBuffer(CurrentScreenSize);

            _world = TestScene.CreateTestWorld();
            _camera = TestScene.CreateCamera(_screenBuffer);

            StartRenderingScene();
        }

        protected override void UnloadContent()
        {
            Content.Unload();
            _graphics.Dispose();
            _spriteBatch.Dispose();
            _outputTexture.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.Escape))
                Exit();
            
            var discreteInput = _keyToggles.Update(keyboard);
            _continuousInputs.Forward = keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W);
            _continuousInputs.Backward = keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S);
            _continuousInputs.TurnLeft = keyboard.IsKeyDown(Keys.Left);
            _continuousInputs.TurnRight = keyboard.IsKeyDown(Keys.Right);
            _continuousInputs.StrafeLeft = keyboard.IsKeyDown(Keys.Q);
            _continuousInputs.StrafeRight = keyboard.IsKeyDown(Keys.E);

            //_playerInfo.Update(_continuousInputs, gameTime);
            //_renderer.Update(_continuousInputs, gameTime);
            //_settings.Update(discreteInput);

            base.Update(gameTime);
        }

        private async void UpdateRenderScale(DiscreteInput input)
        {
            if (input == DiscreteInput.DecreaseRenderFidelity && !_renderScale.IsMinimumQuality())
            {
                await CancelRendering();
                _renderScale.DecreaseQuality();
                StartRenderingScene();
            }
            else if (input == DiscreteInput.IncreaseRenderFidelity && !_renderScale.IsMaximumQuality())
            {
                await CancelRendering();
                _renderScale.IncreaseQuality();
                StartRenderingScene();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(
                sortMode: SpriteSortMode.Immediate,
                blendState: BlendState.Opaque,
                samplerState: SamplerState.PointWrap,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone);

            _screenBuffer.CopyToTexture(_outputTexture);

            _spriteBatch.Draw(
                texture: _outputTexture,
                destinationRectangle: new Microsoft.Xna.Framework.Rectangle(
                    x: 0,
                    y: 0,
                    width: CurrentScreenSize.Width,
                    height: CurrentScreenSize.Height),
                color: Microsoft.Xna.Framework.Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
