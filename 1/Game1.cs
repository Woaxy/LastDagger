using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace FinalProject
{
    public enum GameState { MainMenu, Playing }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GameState _currentState = GameState.MainMenu;

        private Player _player;
        private List<Platform> _platforms;
        private List<Dagger> _daggers;

        private Texture2D _pixel;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
        }

        protected override void Initialize()
        {
            _platforms = new List<Platform>();
            _daggers = new List<Dagger>();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            _player = new Player(_pixel, new Vector2(100, 800));

            _platforms.Add(new Platform(new Rectangle(0, 950, 800, 130), _pixel, Color.DarkGray)); 
            _platforms.Add(new Platform(new Rectangle(900, 800, 400, 50), _pixel, Color.Gray));
            _platforms.Add(new Platform(new Rectangle(1400, 650, 400, 50), _pixel, Color.Gray));
            _platforms.Add(new Platform(new Rectangle(800, 450, 300, 40), _pixel, Color.Gray));
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
            InputManager.Update();

            switch (_currentState)
            {
                case GameState.MainMenu:
                    if (InputManager.IsKeyPressed(Keys.Enter))
                        _currentState = GameState.Playing;
                    break;

                case GameState.Playing:
                    UpdatePlayingState(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        private void UpdatePlayingState(GameTime gameTime)
        {
            _player.Update(gameTime);

            if (InputManager.IsKeyPressed(Keys.F) && _player.DaggerCount > 0)
            {
                _daggers.Add(new Dagger(_player.Position + new Vector2(20, 30), _player.FacingDirection, _pixel));
                _player.DaggerCount--;
            }

            for (int i = _daggers.Count - 1; i >= 0; i--)
            {
                _daggers[i].Update(gameTime);
                if (_daggers[i].Position.X > 2000 || _daggers[i].Position.X < -200)
                    _daggers.RemoveAt(i);
            }

            foreach (var platform in _platforms)
            {
                if (_player.Bounds.Intersects(platform.Bounds))
                {
                    if (_player.Velocity.Y > 0 && _player.Position.Y + _player.Bounds.Height - platform.Bounds.Top < 30)
                    {
                        _player.Position.Y = platform.Bounds.Top - _player.Bounds.Height;
                        _player.ResetJump();
                    }
                }
            }

            if (_player.Position.Y > 1100)
            {
                _player.Position = new Vector2(100, 800);
                _player.Velocity = Vector2.Zero;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            if (_currentState == GameState.Playing)
            {
                foreach (var platform in _platforms) platform.Draw(_spriteBatch);
                foreach (var dagger in _daggers) dagger.Draw(_spriteBatch);
                _player.Draw(_spriteBatch);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}