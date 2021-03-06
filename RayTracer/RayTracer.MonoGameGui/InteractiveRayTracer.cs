using System;
using System.Collections.Concurrent;
using System.Drawing;
using System.Numerics;
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
        private ImageBuffer _screenBuffer;

        private RenderScale _renderScale = RenderScale.Normal;

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEventSlim _restartRayTracingEvent = new ManualResetEventSlim(false);
        private Task _rayTracingFrameTask;

        readonly ContinuousInputs _continuousInputs = new ContinuousInputs();
        readonly ConcurrentQueue<Action> _renderChangeQueue = new ConcurrentQueue<Action>();

        private World _world;
        private Camera _camera;

        // world units per ms
        private const float MovementSpeed = 0.0005f;
        private const float RotationSpeed = 0.00005f;

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

        private void UpdateScreenBufferWithNewSize(object sender, EventArgs e) 
        {
            EnqueueRenderChange(() =>
            {
                var newSize = CurrentScreenSize;
                if (_screenBuffer.Dimensions != newSize)
                {
                    _outputTexture.Dispose();
                    _outputTexture = new Texture2D(_graphics.GraphicsDevice, 
                            width: newSize.Width,
                            height: newSize.Height);
                    _screenBuffer = new ImageBuffer(newSize.DivideBy(_renderScale));
                    _camera.UpdateOutputBuffer(_screenBuffer);
                }
            });
        }

        protected override void Initialize()
        {
            TargetElapsedTime = TimeSpan.FromSeconds(1 / 60d);
            base.Initialize();
        }

        private void StartRenderingScene()
        {
            var oldSource = _cancellationTokenSource;
            _cancellationTokenSource = new CancellationTokenSource();
            oldSource?.Dispose();
            
            _rayTracingFrameTask = Renderer.TraceScene(_camera, _world, _screenBuffer.SetPixel, maximumReflections: 5, _cancellationTokenSource.Token);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _outputTexture = new Texture2D(_graphics.GraphicsDevice, width: CurrentScreenSize.Width, height: CurrentScreenSize.Height);
            _screenBuffer = new ImageBuffer(CurrentScreenSize);

            _world = TestScene.CreateTestWorld();
            _camera = TestScene.CreateCamera(_screenBuffer);

            Task.Factory.StartNew(RayTracingLoop, TaskCreationOptions.LongRunning);
            StartRenderingScene();
        }

        private async void RayTracingLoop()
        {
            // TODO: Somehow lower the quality "while moving" (based on timer?)
            while (true)
            {
                _restartRayTracingEvent.Wait();

                _cancellationTokenSource.Cancel();

                await _rayTracingFrameTask;

                while (_renderChangeQueue.TryDequeue(out Action change))
                {
                    change();
                }

                _restartRayTracingEvent.Reset();
                StartRenderingScene();
            }
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

            _continuousInputs.Forward = keyboard.IsKeyDown(Keys.Up) || keyboard.IsKeyDown(Keys.W);
            _continuousInputs.Backward = keyboard.IsKeyDown(Keys.Down) || keyboard.IsKeyDown(Keys.S);
            _continuousInputs.TurnLeft = keyboard.IsKeyDown(Keys.Left);
            _continuousInputs.TurnRight = keyboard.IsKeyDown(Keys.Right);
            _continuousInputs.StrafeLeft = keyboard.IsKeyDown(Keys.Q);
            _continuousInputs.StrafeRight = keyboard.IsKeyDown(Keys.E);

            EnqueueMovements(_continuousInputs, gameTime);

            base.Update(gameTime);
        }

        private void EnqueueMovements(ContinuousInputs inputs, GameTime gameTime)
        {
            float inlineDistance = MovementSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            float rotationAmount = RotationSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            bool moved = false;

            Matrix4x4 movementMatrix = Matrix4x4.Identity;

            if (inputs.Forward)
            {
                moved = true;
                movementMatrix *= Matrix4x4.CreateTranslation(0, 0, inlineDistance);
            }
            else if (inputs.Backward)
            {
                moved = true;
                movementMatrix *= Matrix4x4.CreateTranslation(0, 0, -inlineDistance);
            }

            if (inputs.TurnRight)
            {
                moved = true;
                movementMatrix *= Matrix4x4.CreateRotationY(-rotationAmount);
            }
            else if (inputs.TurnLeft)
            {
                moved = true;
                movementMatrix *= Matrix4x4.CreateRotationY(rotationAmount);
            }

            if (moved)
            {
                EnqueueRenderChange(() => _camera.Transform *= movementMatrix);
            }
        }

        private void EnqueueRenderChange(Action change)
        {
            _renderChangeQueue.Enqueue(change);
            _restartRayTracingEvent.Set();
        }

        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(
                sortMode: SpriteSortMode.Immediate,
                blendState: BlendState.Opaque,
                samplerState: SamplerState.PointWrap,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone);

            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                _screenBuffer.CopyToTexture(_outputTexture);
            }

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
