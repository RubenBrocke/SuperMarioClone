using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SuperMarioClone
{
    public class Floor : Tangible, ISolid
    {
        protected int Width { get; set; }
        protected int Height { get; set; }

        public Floor(int x, int y, int w, int h, Level level, ContentManager contentManager) : base()
        {
            //Properties are set
            Position = new Vector2(x * Global.Instance.GridSize, y * Global.Instance.GridSize);
            CurrentLevel = level;

            Width = w * Global.Instance.GridSize;
            Height = h * Global.Instance.GridSize;

            //Sprite and hitbox are set
            Sprite = contentManager.Load<Texture2D>("GroundSheet");
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRect = new Rectangle(Global.Instance.GridSize, Global.Instance.GridSize, Global.Instance.GridSize, Global.Instance.GridSize);
            for (int x = (int)Position.X; x < Width + (int)Position.X; x += Global.Instance.GridSize)
            {
                for (int y = (int)Position.Y; y < Height + (int)Position.Y; y += Global.Instance.GridSize)
                {
                    if (y == (int)Position.Y)
                    {
                        if (x == (int)Position.X)
                        {
                            sourceRect.X = 0 * Global.Instance.GridSize;
                            sourceRect.Y = 0 * Global.Instance.GridSize;
                        }
                        else if (x == (int)Position.X + Width - Global.Instance.GridSize)
                        {
                            sourceRect.X = 2 * Global.Instance.GridSize;
                            sourceRect.Y = 0 * Global.Instance.GridSize;
                        }
                        else
                        {
                            sourceRect.X = 1 * Global.Instance.GridSize;
                            sourceRect.Y = 0 * Global.Instance.GridSize;
                        }
                    }
                    else if (y == (int)Position.Y + Height - Global.Instance.GridSize)
                    {
                        if (x == (int)Position.X)
                        {
                            sourceRect.X = 0 * Global.Instance.GridSize;
                            sourceRect.Y = 2 * Global.Instance.GridSize;
                        }
                        else if (x == (int)Position.X + Width - Global.Instance.GridSize)
                        {
                            sourceRect.X = 2 * Global.Instance.GridSize;
                            sourceRect.Y = 2 * Global.Instance.GridSize;
                        }
                        else
                        {
                            sourceRect.X = 1 * Global.Instance.GridSize;
                            sourceRect.Y = 2 * Global.Instance.GridSize;
                        }
                    }
                    else if (x == (int)Position.X)
                    {
                        sourceRect.X = 0 * Global.Instance.GridSize;
                        sourceRect.Y = 1 * Global.Instance.GridSize;
                    }
                    else if (x == (int)Position.X + Width - Global.Instance.GridSize)
                    {
                        sourceRect.X = 2 * Global.Instance.GridSize;
                        sourceRect.Y = 1 * Global.Instance.GridSize;
                    }
                    else
                    {
                        sourceRect.X = 1 * Global.Instance.GridSize;
                        sourceRect.Y = 1 * Global.Instance.GridSize;
                    }
                    spriteBatch.Draw(texture: Sprite, position: new Vector2(x, y), sourceRectangle: sourceRect);
                }
            }
        }
    } 
}

