using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace FinalProject
{
    public class Level
    {
        private Texture2D _tilesetTexture;
        private int _tileSize; 
        private int _tilesetTilesPerRow; 
        private float _scale; 
        private float _drawTileSize; 

        private int[,] _levelMap;

        private List<Rectangle> _solidRects;

        public Level(Texture2D tilesetTex, int tileSize, float scale, int[,] mapData)
        {
            _tilesetTexture = tilesetTex;
            _tileSize = tileSize;
            _scale = scale;
            _levelMap = mapData;

            _tilesetTilesPerRow = _tilesetTexture.Width / _tileSize;
            _drawTileSize = _tileSize * _scale;
            _solidRects = new List<Rectangle>();

            GenerateCollisionData(); 
        }

        private void GenerateCollisionData()
        {
            _solidRects.Clear();
            int rows = _levelMap.GetLength(0);
            int cols = _levelMap.GetLength(1);

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    int tileId = _levelMap[y, x];
                    if (tileId > 0) 
                    {
                        Rectangle baseRect = new Rectangle(
                            (int)(x * _drawTileSize),
                            (int)(y * _drawTileSize),
                            (int)_drawTileSize,
                            (int)_drawTileSize
                        );

                        if (tileId >= 20 && tileId <= 30)
                        {
                            int hitboxWidth = (int)(_drawTileSize * 0.7f); 
                            int hitboxHeight = (int)(_drawTileSize * 0.2f); 
                            int hitboxX = baseRect.X + (baseRect.Width - hitboxWidth) / 2;
                            int hitboxY = baseRect.Y; // En üst yüzey

                            _solidRects.Add(new Rectangle(hitboxX, hitboxY, hitboxWidth, hitboxHeight));
                        }
                        else
                        {
                            _solidRects.Add(baseRect);
                        }
                    }
                }
            }
        }

        public List<Rectangle> GetSolidRects()
        {
            return _solidRects;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int rows = _levelMap.GetLength(0);
            int cols = _levelMap.GetLength(1);

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    int tileId = _levelMap[y, x];
                    if (tileId > 0)
                    {
                        int actualTileId = tileId - 1; 
                        int sourceX = (actualTileId % _tilesetTilesPerRow) * _tileSize;
                        int sourceY = (actualTileId / _tilesetTilesPerRow) * _tileSize;
                        Rectangle sourceRectangle = new Rectangle(sourceX, sourceY, _tileSize, _tileSize);

                        Vector2 drawPosition = new Vector2(x * _drawTileSize, y * _drawTileSize);

                        spriteBatch.Draw(
                            _tilesetTexture,
                            drawPosition, 
                            sourceRectangle,
                            Color.White,
                            0f,
                            Vector2.Zero,
                            _scale, 
                            SpriteEffects.None,
                            0f
                        );
                    }
                }
            }
        }
    }
}