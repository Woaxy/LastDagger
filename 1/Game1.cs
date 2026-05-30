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
        private List<Dagger> _daggers = new List<Dagger>();
        private List<Enemy> _enemies = new List<Enemy>();
        private List<Collectible> _collectibles = new List<Collectible>();

        private Texture2D _pixel;
        private SpriteFont _font; 

        private Texture2D _daggerSprite;
        private Texture2D _goldDaggerSprite;

        private Texture2D _enemyWalkSprite;
        private Texture2D _enemyDeathSprite;
        private Texture2D _platform1; 
        private Texture2D _platform2;
        private Texture2D _platform3; 
        private Texture2D _platform4; 
        private Texture2D _bgClouds;
        private Texture2D _bgDarkFacility;
        private Texture2D _bgStructures;
        private List<Platform> _platforms = new List<Platform>();
        
        private int _currentLevel = 1;
        private float _timer = 120f; 

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

            _font = Content.Load<SpriteFont>("Font");

            _daggerSprite = Content.Load<Texture2D>("dagger");
            _goldDaggerSprite = Content.Load<Texture2D>("gold_dagger");

            _enemyWalkSprite = Content.Load<Texture2D>("enemy_patrol");
            _enemyDeathSprite = Content.Load<Texture2D>("enemy_death");

            _platform1 = Content.Load<Texture2D>("tileset_main_1");
            _platform2 = Content.Load<Texture2D>("tileset_main_2");
            _platform3 = Content.Load<Texture2D>("tileset_main_3");
            _platform4 = Content.Load<Texture2D>("tileset_main_4");

            _bgClouds = Content.Load<Texture2D>("clouds");
            _bgDarkFacility = Content.Load<Texture2D>("background6");
            _bgStructures = Content.Load<Texture2D>("back-structures");

            ResetGame();
        }

        private void ResetGame()
        {
            _currentLevel = 1;
            _timer = 120f; 

            _player = new Player(Content, new Vector2(100, 800)); 

            LoadLevel(_currentLevel);
        }

        private void LoadLevel(int level)
        {
            _platforms.Clear();
            _enemies.Clear();
            _daggers.Clear();
            _collectibles.Clear();
            
            _player.Position = new Vector2(100, 700); 
            _player.Velocity = Vector2.Zero;

            switch (level)
            {
                case 1:
                    _platforms.Add(new Platform(_platform4, new Vector2(50, 900), false));
                    
                    _platforms.Add(new Platform(_platform1, new Vector2(450, 750), false));
                    _platforms.Add(new Platform(_platform3, new Vector2(700, 580), true)); 

                    _platforms.Add(new Platform(_platform2, new Vector2(1050, 450), true));
                    _platforms.Add(new Platform(_platform2, new Vector2(1240, 450), true));

                    _platforms.Add(new Platform(_platform4, new Vector2(1700, 800), false));
                    _enemies.Add(new Enemy(_enemyWalkSprite, _enemyDeathSprite, new Vector2(1100, 350), 100f, _platforms));
                    break;
                    
                case 2:
                    _platforms.Add(new Platform(_platform4, new Vector2(50, 850), false));
                    _platforms.Add(new Platform(_platform1, new Vector2(350, 700), false)); 
                    _platforms.Add(new Platform(_platform3, new Vector2(550, 500), true));  
                    
                    _platforms.Add(new Platform(_platform2, new Vector2(850, 750), true));
                    _platforms.Add(new Platform(_platform2, new Vector2(1040, 750), true));

                    _platforms.Add(new Platform(_platform1, new Vector2(1350, 600), false));
                    _platforms.Add(new Platform(_platform4, new Vector2(1650, 400), false)); 

                    _enemies.Add(new Enemy(_enemyWalkSprite, _enemyDeathSprite, new Vector2(900, 667), 100f, _platforms));
                    
                    _enemies.Add(new Enemy(_enemyWalkSprite, _enemyDeathSprite, new Vector2(1625, 305), 70f, _platforms));
                    break;

                case 3:
                    _platforms.Add(new Platform(_platform4, new Vector2(50, 850), false));    
                    _platforms.Add(new Platform(_platform1, new Vector2(420, 750), false));   
                    _platforms.Add(new Platform(_platform3, new Vector2(750, 600), true));    
                    
                    _platforms.Add(new Platform(_platform2, new Vector2(1150, 600), true));  
                    _enemies.Add(new Enemy(_enemyWalkSprite, _enemyDeathSprite, new Vector2(1150, 517), 60f, _platforms));
                    
                    _platforms.Add(new Platform(_platform1, new Vector2(1500, 500), false)); 

                    _platforms.Add(new Platform(_platform4, new Vector2(1700, 350), false));  
                    _enemies.Add(new Enemy(_enemyWalkSprite, _enemyDeathSprite, new Vector2(1710, 255), 60f, _platforms));

                    _platforms.Add(new Platform(_platform1, new Vector2(1350, 250), false));  
                    _platforms.Add(new Platform(_platform3, new Vector2(900, 200), true));  

                    _platforms.Add(new Platform(_platform4, new Vector2(500, 200), false));   
                    _enemies.Add(new Enemy(_enemyWalkSprite, _enemyDeathSprite, new Vector2(490, 105), 70f, _platforms));
                    
                    _collectibles.Add(new Collectible(new Rectangle(550, 180, 40, 40), _goldDaggerSprite, true));
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
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            _timer -= dt;
            _player.Update(gameTime);


            if (InputManager.IsKeyPressed(Keys.F) && _player.DaggerCount > 0)
            {
                _player.StartAttack(); 
            }

            if (_player.CanThrowDagger() && _player.DaggerCount > 0)
            {
                _daggers.Add(new Dagger(_player.Position + new Vector2(20, 60), _player.FacingDirection, _daggerSprite));
                _player.DaggerCount--;
            }

            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                _enemies[i].Update(gameTime); 
                
                if (_enemies[i].State == EnemyState.Dead)
                {
                    _enemies.RemoveAt(i);
                }
            }
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
                else _player.Position.X = 1920 - _player.Bounds.Width;
            }

            HandleCollisions();

            if (_player.Position.Y > 1100) 
            {
                _player.TakeDamage();
                _player.Position = new Vector2(50, 800); 
            }
            
            if (_player.Lives <= 0 || _timer <= 0) 
            {
                _currentState = GameState.GameOver;
            }
        }

        private void HandleCollisions()
        {
            if (_player.Velocity.Y > 0)
            {
                foreach (var platform in _platforms)
                {
                    if (_player.Bounds.Intersects(platform.Bounds))
                    {
                        if (_player.Bounds.Bottom - platform.Bounds.Top < 30)
                        {
                            _player.Position.Y = platform.Bounds.Top - _player.Bounds.Height;
                            _player.Velocity.Y = 0f;
                            _player.ResetJump();
                            break; 
                        }
                    }
                }
            }

            foreach (var enemy in _enemies)
            {
                enemy.Position.Y += 5f; 

                foreach (var platform in _platforms)
                {
                    if (enemy.Bounds.Intersects(platform.Bounds))
                    {
                        enemy.Position.Y = platform.Bounds.Top - enemy.Bounds.Height;
                        break; 
                    }
                }
            }

            for (int i = _daggers.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < _enemies.Count; j++)
                {
                    if (_enemies[j].State == EnemyState.Patrolling && _enemies[j].Bounds.Intersects(_daggers[i].Bounds))
                    {
                        _collectibles.Add(new Collectible(new Rectangle((int)_enemies[j].Position.X, (int)_enemies[j].Position.Y + 60, 30, 30), _daggerSprite, false));
                        _enemies[j].StartDeath();
                        _daggers.RemoveAt(i);
                        break; 
                    }
                }
            }

            foreach (var enemy in _enemies)
            {
                if (_player.Bounds.Intersects(enemy.Bounds)) _player.TakeDamage();
            }

            for (int i = _collectibles.Count - 1; i >= 0; i--)
            {
                if (_player.Bounds.Intersects(_collectibles[i].Bounds))
                {
                    if (_collectibles[i].IsGoal) _currentState = GameState.Win; 
                    else _player.DaggerCount ++; 
                        
                    _collectibles.RemoveAt(i);
                }
            }
        }
    
        protected override void Draw(GameTime gameTime)
        {
            if (_currentState == GameState.GameOver) GraphicsDevice.Clear(Color.DarkRed);
            else if (_currentState == GameState.Win) GraphicsDevice.Clear(Color.Gold);
            else GraphicsDevice.Clear(Color.CornflowerBlue); 

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            switch (_currentState)
            {
                case GameState.MainMenu:
                    _spriteBatch.DrawString(_font, "LAST DAGGER", new Vector2(800, 400), Color.White);
                    _spriteBatch.DrawString(_font, "Press ENTER to Start", new Vector2(820, 450), Color.White);
                    break;

                case GameState.Playing:
                    Rectangle screenRect = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                    
                    _spriteBatch.Draw(_bgClouds, screenRect, Color.White);       

                    if (_currentLevel == 3)
                    {
                        _spriteBatch.Draw(_bgDarkFacility, screenRect, Color.White); 
                    }

                    _spriteBatch.Draw(_bgStructures, screenRect, Color.White);

                    foreach (var p in _platforms)
                    {
                        p.Draw(_spriteBatch);
                    }
                    
                    foreach (var e in _enemies) e.Draw(_spriteBatch);
                    foreach (var c in _collectibles) c.Draw(_spriteBatch);
                    foreach (var d in _daggers) d.Draw(_spriteBatch);
                    _player.Draw(_spriteBatch);

                    _spriteBatch.DrawString(_font, $"LIVES: {_player.Lives}", new Vector2(30, 30), Color.White);
                    _spriteBatch.DrawString(_font, $"DAGGERS: {_player.DaggerCount}", new Vector2(30, 70), Color.White);
                    _spriteBatch.DrawString(_font, $"LEVEL: {_currentLevel}", new Vector2(900, 30), Color.White);
                    _spriteBatch.DrawString(_font, $"TIME: {(int)_timer}", new Vector2(1750, 30), Color.White);
                    break;

                case GameState.GameOver:
                    _spriteBatch.DrawString(_font, "GAME OVER!", new Vector2(850, 400), Color.White);
                    _spriteBatch.DrawString(_font, "Press ENTER to Try Again", new Vector2(800, 450), Color.White);
                    break;

                case GameState.Win:
                    _spriteBatch.DrawString(_font, "YOU WIN!", new Vector2(880, 400), Color.Black);
                    _spriteBatch.DrawString(_font, "Press ENTER to Play Again", new Vector2(800, 450), Color.Black);
                    break;
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}