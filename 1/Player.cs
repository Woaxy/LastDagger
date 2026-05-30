using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace FinalProject
{
    public class Player
    {
        public Vector2 Position; 
        public Vector2 Velocity;

        public Rectangle Bounds
        {
            get
            {
                int hitboxGenisligi = 30; 
                int xKaymasi = 0;

                if (FacingDirection == 1)
                {
                    xKaymasi = 75; 
                }
                else
                {
                    xKaymasi = 80; 
                }

                return new Rectangle(
                    (int)(Position.X + xKaymasi),
                    (int)Position.Y,
                    hitboxGenisligi,
                    90 
                );
            }
        }
        
        public int DaggerCount = 5;
        public int Lives = 3;
        public int FacingDirection = 1;
        public float InvincibilityTimer = 0f;

        private int _jumpCount = 0;
        private const int MaxJumps = 2;

        private AnimationManager _currentAnimation;
        private AnimationManager _idleAnim;
        private AnimationManager _runAnim;
        private AnimationManager _jumpAnim;
        private AnimationManager _slashAnim;

        public bool IsAttacking { get; private set; }
        private bool _hasThrownDaggerThisAttack = false;

        private Vector2 _currentOffset;

        public Player(ContentManager content, Vector2 startPos)
        {
            Position = startPos; 

            _idleAnim = new AnimationManager(content.Load<Texture2D>("player-idle"), 4, 0.15f, true);
            _idleAnim.Scale = 1.5f;

            _runAnim = new AnimationManager(content.Load<Texture2D>("player-Run"), 12, 0.08f, true);
            _runAnim.Scale = 1.5f;

            _jumpAnim = new AnimationManager(content.Load<Texture2D>("player-Jump"), 4, 0.1f, false);
            _jumpAnim.Scale = 1.5f;

            _slashAnim = new AnimationManager(content.Load<Texture2D>("player-Sword Slash"), 6, 0.08f, false);
            _slashAnim.Scale = 1.5f;

            _currentAnimation = _idleAnim;
            _currentOffset = Vector2.Zero; 
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (InvincibilityTimer > 0)
                InvincibilityTimer -= dt;

            Velocity.Y += 1500f * dt;
            bool isMoving = false;

            if (!IsAttacking) 
            {
                if (InputManager.IsKeyDown(Keys.Left))
                {
                    Velocity.X = -300f;
                    FacingDirection = -1;
                    isMoving = true;
                }
                else if (InputManager.IsKeyDown(Keys.Right))
                {
                    Velocity.X = 300f;
                    FacingDirection = 1;
                    isMoving = true;
                }
                else Velocity.X = 0f;

                if (InputManager.IsKeyPressed(Keys.Space) && _jumpCount < MaxJumps)
                {
                    Velocity.Y = -600f; 
                    _jumpCount++;
                }
            }
            else
            {
                Velocity.X = 0f;
            }

            Position += Velocity * dt;
            if (Position.X < 0) Position.X = 0;

            SpriteEffects currentEffect = (FacingDirection == 1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

            
            if (IsAttacking)
            {
                _currentAnimation = _slashAnim;
                
                _currentOffset = new Vector2(-30, 0); 
                
                if (_slashAnim.IsFinished) 
                {
                    IsAttacking = false;
                    _hasThrownDaggerThisAttack = false;
                }
            }
            else if (System.Math.Abs(Velocity.Y) > 100f) 
            {
                _currentAnimation = _jumpAnim;
                _currentOffset = Vector2.Zero; 
            }
            else if (isMoving)
            {
                _currentAnimation = _runAnim;
                _currentOffset = new Vector2(-10, 0); 
            }
            else
            {
                _currentAnimation = _idleAnim;
                _currentOffset = Vector2.Zero; 
            }

            _currentAnimation.Position = new Vector2(Position.X + _currentOffset.X, Position.Y + _currentOffset.Y - 35);
            
            _currentAnimation.Effect = currentEffect;
            _currentAnimation.Update(gameTime);
        }

        public void StartAttack()
        {
            if (!IsAttacking)
            {
                IsAttacking = true;
                _hasThrownDaggerThisAttack = false;
                _slashAnim.Reset();
            }
        }

        public bool CanThrowDagger()
        {
            if (IsAttacking && _slashAnim.CurrentFrame == 3 && !_hasThrownDaggerThisAttack)
            {
                _hasThrownDaggerThisAttack = true; 
                return true;
            }
            return false;
        }

        public void TakeDamage()
        {
            if (InvincibilityTimer <= 0)
            {
                Lives--;
                InvincibilityTimer = 1.5f;
                Velocity.Y = -400f;
            }
        }

        public void ResetJump()
        {
            _jumpCount = 0;
            Velocity.Y = 0;
            _jumpAnim.Reset();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color color = (InvincibilityTimer > 0 && (int)(InvincibilityTimer * 10) % 2 == 0) ? Color.Transparent : Color.White;
            _currentAnimation.Draw(spriteBatch, color);
        }
    }
}