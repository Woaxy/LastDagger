using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FinalProject
{
    public class Collectible
    {
        public Rectangle Bounds;
        public bool IsGoal; 
        private Texture2D _texture;

        public Collectible(Rectangle bounds, Texture2D texture, bool isGoal = false)
        {
            Bounds = bounds;
            _texture = texture;
            IsGoal = isGoal;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color color = IsGoal ? Color.Gold : Color.LightGray;
            spriteBatch.Draw(_texture, Bounds, color);
        }
    }
}