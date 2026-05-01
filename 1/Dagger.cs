using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FinalProject
{
    public class Dagger
    {
        public Vector2 Position;
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, 20, 5);
        
        private float _speed = 700f;
        private int _direction; 
        private Texture2D _texture;

        public Dagger(Vector2 startPosition, int direction, Texture2D texture)
        {
            Position = startPosition;
            _direction = direction;
            _texture = texture;
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position.X += _speed * _direction * dt;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Bounds, Color.LightGray);
        }
    }
}