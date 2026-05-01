using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace FinalProject
{
    public enum GameState { MainMenu, Playing, GameOver, Win }

    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GameState _currentState = GameState.MainMenu;

        private Player _player;
        private List<Platform> _platforms = new List<Platform>();
        private List<Dagger> _daggers = new List<Dagger>();
        private List<Enemy> _enemies = new List<Enemy>();
        private List<Collectible> _collectibles = new List<Collectible>();

        private Texture2D _pixel;
        private int _currentLevel = 1; 

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            ResetGame();
        }

        private void ResetGame()
        {
            _currentLevel = 1;
            _player = new Player(_pixel, new Vector2(100, 800));
            LoadLevel(_currentLevel);
        }

        private void LoadLevel(int level)
        {
            _platforms.Clear();
            _enemies.Clear();
            _collectibles.Clear();
            _daggers.Clear();
            
            _player.Position = new Vector2(50, 800); 
            _player.Velocity = Vector2.Zero;

            // Ortak Zemin
            _platforms.Add(new Platform(new Rectangle(0, 950, 400, 130), _pixel, Color.DarkGray));

            switch (level)
            {
                case 1:
                    _platforms.Add(new Platform(new Rectangle(600, 800, 400, 50), _pixel, Color.Gray));
                    _platforms.Add(new Platform(new Rectangle(1200, 650, 400, 50), _pixel, Color.Gray));
                    _enemies.Add(new Enemy(new Vector2(700, 720), 100f, _pixel)); // 1. Düşman
                    break;
                case 2:
                    _platforms.Add(new Platform(new Rectangle(500, 750, 200, 50), _pixel, Color.Gray));
                    _platforms.Add(new Platform(new Rectangle(900, 600, 500, 50), _pixel, Color.Gray));
                    _enemies.Add(new Enemy(new Vector2(1000, 520), 150f, _pixel)); // Devriye atan düşman
                    _platforms.Add(new Platform(new Rectangle(1600, 850, 300, 50), _pixel, Color.Gray));
                    break;
                case 3:
                    _platforms.Add(new Platform(new Rectangle(500, 850, 300, 50), _pixel, Color.Gray));
                    _platforms.Add(new Platform(new Rectangle(1000, 700, 300, 50), _pixel, Color.Gray));
                    _platforms.Add(new Platform(new Rectangle(1500, 550, 300, 50), _pixel, Color.Gray));
                    
                    _enemies.Add(new Enemy(new Vector2(1050, 620), 50f, _pixel)); 
                    
                    _collectibles.Add(new Collectible(new Rectangle(1650, 500, 40, 40), _pixel, true));
                    break;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();
            InputManager.Update();

            switch (_currentState)
            {
                case GameState.MainMenu:
                case GameState.GameOver:
                case GameState.Win:
                    if (InputManager.IsKeyPressed(Keys.Enter))
                    {
                        ResetGame();
                        _currentState = GameState.Playing;
                    }
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

            foreach (var enemy in _enemies) enemy.Update(gameTime);
            for (int i = _daggers.Count - 1; i >= 0; i--)
            {
                _daggers[i].Update(gameTime);
                if (_daggers[i].Position.X > 2000 || _daggers[i].Position.X < -200) _daggers.RemoveAt(i);
            }

            if (_player.Position.X > 1920)
            {
                if (_currentLevel < 3)
                {
                    _currentLevel++;
                    LoadLevel(_currentLevel);
                }
                else
                {
                    _player.Position.X = 1920 - _player.Bounds.Width; 
                }
            }

            HandleCollisions();

            if (_player.Position.Y > 1100) 
            {
                _player.TakeDamage();
                _player.Position = new Vector2(50, 800);
            }
            if (_player.Lives <= 0) _currentState = GameState.GameOver;
        }

        private void HandleCollisions()
        {
            foreach (var platform in _platforms)
            {
                if (_player.Bounds.Intersects(platform.Bounds) && _player.Velocity.Y > 0)
                {
                    if (_player.Position.Y + _player.Bounds.Height - platform.Bounds.Top < 30)
                    {
                        _player.Position.Y = platform.Bounds.Top - _player.Bounds.Height;
                        _player.ResetJump();
                    }
                }
            }

            for (int i = _daggers.Count - 1; i >= 0; i--)
            {
                for (int j = _enemies.Count - 1; j >= 0; j--)
                {
                    if (_daggers[i].Bounds.Intersects(_enemies[j].Bounds))
                    {
                        _collectibles.Add(new Collectible(new Rectangle((int)_enemies[j].Position.X, (int)_enemies[j].Position.Y + 60, 20, 10), _pixel, false));
                        _enemies.RemoveAt(j);
                        _daggers.RemoveAt(i);
                        break; 
                    }
                }
            }

            foreach (var enemy in _enemies)
            {
                if (_player.Bounds.Intersects(enemy.Bounds))
                {
                    _player.TakeDamage();
                }
            }

            for (int i = _collectibles.Count - 1; i >= 0; i--)
            {
                if (_player.Bounds.Intersects(_collectibles[i].Bounds))
                {
                    if (_collectibles[i].IsGoal)
                        _currentState = GameState.Win; 
                    else
                        _player.DaggerCount += 1; 
                        
                    _collectibles.RemoveAt(i);
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            if (_currentState == GameState.GameOver) GraphicsDevice.Clear(Color.DarkRed);
            else if (_currentState == GameState.Win) GraphicsDevice.Clear(Color.Gold);
            else GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            if (_currentState == GameState.Playing)
            {
                foreach (var p in _platforms) p.Draw(_spriteBatch);
                foreach (var e in _enemies) e.Draw(_spriteBatch);
                foreach (var c in _collectibles) c.Draw(_spriteBatch);
                foreach (var d in _daggers) d.Draw(_spriteBatch);
                _player.Draw(_spriteBatch);
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}