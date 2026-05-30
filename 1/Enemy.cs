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
        private float _speed;
        private List<Platform> _platforms;
        private int _facingDirection = 1;

        private float _patrolTimer = 0f;
        private float _patrolDuration = 1.7f; 

        public Enemy(Texture2D walkTex, Texture2D deathTex, Vector2 startPos, float speed, List<Platform> platforms)
        {
            Position = startPos;
            _platforms = platforms;
            
            _speed = speed; 

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
                _patrolTimer += dt;

                if (_patrolTimer >= _patrolDuration)
                {
                    _facingDirection *= -1; 
                    _patrolTimer = 0f;      
                }

                Position.X += _speed * _facingDirection * dt;

                _patrolAnim.Position = new Vector2(Position.X, Position.Y);
                _patrolAnim.Effect = (_facingDirection == 1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                _patrolAnim.Update(gameTime);
            }
            else if (State == EnemyState.Dying)
            {
                _deathAnim.Position = new Vector2(Position.X, Position.Y);
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