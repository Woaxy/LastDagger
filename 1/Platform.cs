using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FinalProject
{
    public class Platform
    {
        public Rectangle Bounds;
        public Texture2D Texture;
        public Color Color;

        public Platform(Rectangle bounds, Texture2D texture, Color color)
        {
            Bounds = bounds;
            Texture = texture;
            Color = color;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Bounds, Color);
        }
    }
}