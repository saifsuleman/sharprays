using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Drawing.Imaging;

namespace Renderer
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Scene _scene;
        private Rectangle _tracedSize;
        private Texture2D _canvas;
        private double _resolution = 1.0/16.0;
        private DateTime? lastScreenshot = null;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();

            _tracedSize = GraphicsDevice.PresentationParameters.Bounds;
            _canvas = new Texture2D(GraphicsDevice, _tracedSize.Width, _tracedSize.Height, false, SurfaceFormat.Color);
            var camera = new Location(new Vector(0, 0, 0), 0, 0);
            var light = new LightSource(new Vector(10,10,10), 1.0);
            _scene = new Scene(camera, light);
            
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                        _scene.AddEntity(new Sphere(
                   new Location(new Vector(20 + 15 * i, 0, 20 + 15 * j), 0, 0),
                   6,
                   new Color(1.0, 0.3, 0.2),
                   .4,
                   .4
                   ));
                }
            }


            _scene.AddEntity(new Plane(0.3, 0.2));

            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.CornflowerBlue);
            GraphicsDevice.Textures[0] = null;

            base.Initialize();
        }

        private void Screenshot()
        {
            var now = DateTime.Now;
            if (lastScreenshot != null && now.Subtract((DateTime)this.lastScreenshot).TotalSeconds < 2)
            {
                return;
            }
            var bitmap = Renderer.RenderBitmap(this._scene, 1920, 1080);
            var encoders = ImageCodecInfo.GetImageEncoders();
            foreach (var encoder in encoders)
            {
                if (encoder.MimeType == "image/jpeg")
                {
                    var parameters = new EncoderParameters(1);
                    parameters.Param[0] = new EncoderParameter(Encoder.Quality, 25L);
                    bitmap.Save("out.jpg", encoder, parameters);

                    Process p = new Process
                    {
                        StartInfo = new ProcessStartInfo("out.jpg")
                        {
                            UseShellExecute = true
                        }
                    };
                    p.Start();
                    
                    break;
                }
            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.W))
            {
                var vec = new Vector(0, 0, gameTime.ElapsedGameTime.TotalMilliseconds * 0.1);
                _scene.camera.position = _scene.camera.position.Add(vec.RotateYP(_scene.camera.yaw, 0));
            }
            if (state.IsKeyDown(Keys.S))
            {
                var vec = new Vector(0, 0, gameTime.ElapsedGameTime.TotalMilliseconds * -0.1);
                _scene.camera.position = _scene.camera.position.Add(vec.RotateYP(_scene.camera.yaw, 0));
            }
            if (state.IsKeyDown(Keys.A))
            {
                var vec = new Vector(gameTime.ElapsedGameTime.TotalMilliseconds * -0.1, 0, 0);
                _scene.camera.position = _scene.camera.position.Add(vec.RotateYP(_scene.camera.yaw, 0));
            }
            if (state.IsKeyDown(Keys.D))
            {
                var vec = new Vector(gameTime.ElapsedGameTime.TotalMilliseconds * 0.1, 0, 0);
                _scene.camera.position = _scene.camera.position.Add(vec.RotateYP(_scene.camera.yaw, 0));
            }
            if (state.IsKeyDown(Keys.Space))
            {
                _scene.camera.position = _scene.camera.position.Add(0, gameTime.ElapsedGameTime.TotalMilliseconds * 0.1, 0);
            }
            if (state.IsKeyDown(Keys.LeftShift))
            {
                _scene.camera.position = _scene.camera.position.Add(0, gameTime.ElapsedGameTime.TotalMilliseconds * -0.1, 0);
            }
            if (state.IsKeyDown(Keys.Up))
            {
                _scene.camera.pitch -= 0.1 * gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            if (state.IsKeyDown(Keys.Down))
            {
                _scene.camera.pitch += 0.1 * gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            if (state.IsKeyDown(Keys.Left))
            {
                _scene.camera.yaw -= 0.1 * gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            if (state.IsKeyDown(Keys.Right))
            {
                _scene.camera.yaw += 0.1 * gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            if (state.IsKeyDown(Keys.R))
            {
                System.Diagnostics.Debug.WriteLine("Resolution: 10%");
                _resolution = 0.1D;
            }
            if (state.IsKeyDown(Keys.T))
            {
                System.Diagnostics.Debug.WriteLine("Resolution: 100%");
                _resolution = 1.0D;
            }
            if (state.IsKeyDown(Keys.L))
            {
                Screenshot();
            }

            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            System.Diagnostics.Debug.WriteLine("FPS: " + (1000.0 / ((double)gameTime.ElapsedGameTime.Milliseconds)));

            var colors = Renderer.Render(this._scene, _tracedSize.Width, _tracedSize.Height, _resolution);
            var rect = new Rectangle(0, 0, _tracedSize.Width, _tracedSize.Height);
            _canvas.SetData(0, rect, colors, 0, _tracedSize.Width * _tracedSize.Height);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_canvas, new Rectangle(0, 0, _tracedSize.Width, _tracedSize.Height), Microsoft.Xna.Framework.Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
