using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FinalProject
{
    public enum EnemyState { Patrolling, Dying, Dead }

    public class Enemy
    {
        public Vector2 Position;
        public EnemyState State = EnemyState.Patrolling;

        private const int _width = 50;
        private const int _height = 95;
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, _width, _height);
        
        private AnimationManager _patrolAnim;
        private AnimationManager _deathAnim;
        
        private Vector2 _velocity;
        private List<Platform> _platforms;
        private int _facingDirection = 1;

        public Enemy(Texture2D walkTex, Texture2D deathTex, Vector2 startPos, float speed, List<Platform> platforms)
        {
            Position = startPos;
            _platforms = platforms;
            
            _velocity = new Vector2(speed, 0f); 

            _patrolAnim = new AnimationManager(walkTex, 6, 0.12f, true); 
            _patrolAnim.Scale = 1.5f; 

            _deathAnim = new AnimationManager(deathTex, 1, 0.08f, false);
            _deathAnim.Scale = 3f;
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (State == EnemyState.Patrolling)
            {
                Position.X += _velocity.X * _facingDirection * dt;
                Rectangle nextBounds = new Rectangle(Bounds.X + (int)(_velocity.X * _facingDirection * dt), Bounds.Y, Bounds.Width, Bounds.Height);
                int checkX = (_facingDirection == 1) ? nextBounds.Right : nextBounds.Left;
                Rectangle nextGroundCheck = new Rectangle(checkX - 5, nextBounds.Bottom, 10, 5);

                bool willBeOnPlatform = false;
                foreach (var platform in _platforms)
                {
                    if (platform.Bounds.Intersects(nextGroundCheck))
                    {
                        willBeOnPlatform = true;
                        break;
                    }
                }

                if (!willBeOnPlatform) _facingDirection *= -1; 

                _patrolAnim.Position = new Vector2(Position.X, Position.Y - 15);
                _patrolAnim.Effect = (_facingDirection == 1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                _patrolAnim.Update(gameTime);
            }
            else if (State == EnemyState.Dying)
            {
                _deathAnim.Position = Position;
                _deathAnim.Update(gameTime);

                if (_deathAnim.IsFinished)
                {
                    State = EnemyState.Dead;
                }
            }
        }

        public void StartDeath()
        {
            if (State != EnemyState.Dying) 
            {
                State = EnemyState.Dying;
                _deathAnim.Reset(); 
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (State == EnemyState.Patrolling)
            {
                _patrolAnim.Draw(spriteBatch, Color.White);
            }
            else if (State == EnemyState.Dying)
            {
                _deathAnim.Draw(spriteBatch, Color.White); 
            }
        }
    }
}