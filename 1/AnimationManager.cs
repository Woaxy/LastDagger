using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FinalProject
{
    public class AnimationManager
    {
        private Texture2D _texture;
        private float _timer;
        
        public float Speed { get; set; }
        public int CurrentFrame { get; private set; } 
        
        private int _frameCount; 
        private int _frameWidth;
        private int _frameHeight;

        public Vector2 Position { get; set; } 
        public SpriteEffects Effect { get; set; } 

        public bool IsLooping { get; set; } 
        public bool IsFinished { get; private set; }

        public AnimationManager(Texture2D texture, int frameCount, float speed = 0.1f, bool isLooping = true)
        {
            _texture = texture;
            _frameCount = frameCount;
            Speed = speed;
            IsLooping = isLooping;
            
            _frameWidth = _texture.Width / _frameCount;
            _frameHeight = _texture.Height;
            
            Effect = SpriteEffects.None;
            Reset();
        }

        public void Reset()
        {
            CurrentFrame = 0;
            _timer = 0f;
            IsFinished = false;
        }

        public void Update(GameTime gameTime)
        {
            if (!IsLooping && IsFinished) return;

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer > Speed)
            {
                _timer = 0f;
                CurrentFrame++;

                if (CurrentFrame >= _frameCount)
                {
                    if (IsLooping) CurrentFrame = 0;
                    else
                    {
                        CurrentFrame = _frameCount - 1;
                        IsFinished = true;
                    }
                }
            }
        }


        public float Scale { get; set; } = 1.2f;
        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            Rectangle sourceRectangle = new Rectangle(CurrentFrame * _frameWidth, 0, _frameWidth, _frameHeight);

            spriteBatch.Draw(
                _texture,
                Position, 
                sourceRectangle,
                color,
                0f, 
                Vector2.Zero, 
                Scale,
                Effect, 
                0f
            );
        }
    }
}