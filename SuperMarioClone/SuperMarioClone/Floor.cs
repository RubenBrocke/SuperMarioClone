using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SuperMarioClone
{
    public class Floor : Tangible
    {
        //Properties
        protected int Width { get; set; }
        protected int Height { get; set; }

        /// <summary>
        /// Constructor for Floor, sets the position of the Floor using the GridSize and sets its SpriteSheet
        /// </summary>
        /// <param name="x">X position of the Floor</param>
        /// <param name="y">Y position of the Floor</param>
        /// <param name="w">Width of the Floor</param>
        /// <param name="h">Height of the Floor</param>
        /// <param name="level">Level the Floor should be in</param>
        /// <param name="contentManager">ContentManager used to load SpriteSheet</param>
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
            IsSolid = true;
        }

        /// <summary>
        /// Draws the Floor
        /// </summary>
        /// <param name="spriteBatch">Used to Draw the Floor</param>
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

