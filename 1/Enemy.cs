using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FinalProject
{
    public class Enemy
    {
        public Vector2 Position;
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, 40, 80); 
        
        private float _speed = 100f;
        private int _direction = 1;
        private float _leftLimit;
        private float _rightLimit;
        private Texture2D _texture;

        public Enemy(Vector2 startPosition, float patrolDistance, Texture2D texture)
        {
            Position = startPosition;
            _leftLimit = startPosition.X - patrolDistance;
            _rightLimit = startPosition.X + patrolDistance;
            _texture = texture;
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            Position.X += _speed * _direction * dt;
            
            if (Position.X <= _leftLimit) _direction = 1;
            else if (Position.X >= _rightLimit) _direction = -1;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Bounds, Color.Red); 
        }
    }
}