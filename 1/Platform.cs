using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FinalProject
{
    public class Platform
    {
        public Vector2 Position;
        private Texture2D _texture;
        private float _scale;
        private bool _isBushy; 

        public Rectangle Bounds
        {
            get
            {
                int collisionY = (int)Position.Y;

                if (_isBushy)
                {
                    collisionY += (int)(12 * _scale);
                }

                int xKesintisi = 15; 
                int daraltilmisX = (int)Position.X + (int)(xKesintisi * _scale);
                int daraltilmisGenislik = (int)(_texture.Width * _scale) - (int)((xKesintisi * 2) * _scale);

                return new Rectangle(
                    daraltilmisX,          
                    collisionY,           
                    daraltilmisGenislik,   
                    (int)(15 * _scale)    
                );
            }
        }

        public Platform(Texture2D texture, Vector2 position, bool isBushy, float scale = 2f)
        {
            _texture = texture;
            Position = position;
            _scale = scale;
            _isBushy = isBushy; 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position, null, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        }
    }
}