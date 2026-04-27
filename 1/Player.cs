using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FinalProject
{
    public class Player
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, 40, 80);
        
        public int DaggerCount = 3; 
        public int FacingDirection = 1; 

        private Texture2D _texture;
        private int _jumpCount = 0;
        private const int MaxJumps = 2; 

        public Player(Texture2D texture, Vector2 startPos)
        {
            _texture = texture;
            Position = startPos;
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Velocity.Y += 1500f * dt;

            if (InputManager.IsKeyDown(Keys.Left))
            {
                Velocity.X = -300f;
                FacingDirection = -1;
            }
            else if (InputManager.IsKeyDown(Keys.Right))
            {
                Velocity.X = 300f;
                FacingDirection = 1;
            }
            else
            {
                Velocity.X = 0f;
            }

            if (InputManager.IsKeyPressed(Keys.Space) && _jumpCount < MaxJumps)
            {
                Velocity.Y = -600f; // Zıplama gücü
                _jumpCount++;
            }

            Position += Velocity * dt;
        }

        public void ResetJump()
        {
            _jumpCount = 0;
            Velocity.Y = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Bounds, Color.Blue);
        }
    }
}