using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FinalProject
{
    public class Dagger
    {
        public Vector2 Position;
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, 32, 16); 
        
        private float _speed = 800f; 
        private int _direction;
        private Texture2D _texture;

        public Dagger(Vector2 position, int direction, Texture2D texture)
        {
            Position = position;
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
            Vector2 origin = new Vector2(_texture.Width / 2f, _texture.Height / 2f);

            float rotationAngle = (_direction == 1) ? MathHelper.ToRadians(240) : MathHelper.ToRadians(-240);

            SpriteEffects effect = SpriteEffects.None;

            spriteBatch.Draw(
                _texture, 
                Position, 
                null, 
                Color.White, 
                rotationAngle, 
                origin,        
                0.1f,            
                effect, 
                0f 
            );
        }
    }
}