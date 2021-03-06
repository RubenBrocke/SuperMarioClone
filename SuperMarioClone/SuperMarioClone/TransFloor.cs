﻿using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperMarioClone
{
    public class TransFloor : Tangible
    {
        //Properties
        protected int Width { get; set; }
        protected int Height { get; set; }

        /// <summary>
        /// Constructor for TransFloor, sets the position of the TransFloor using the GridSize and sets its SpriteSheet
        /// </summary>
        /// <param name="x">X position of the TransFloor</param>
        /// <param name="y">Y position of the TransFloor</param>
        /// <param name="w">Width of the TransFloor</param>
        /// <param name="h">Height of the TransFloor</param>
        /// <param name="level">Level the TransFloor should be in</param>
        /// <param name="contentManager">ContentManager used to load SpriteSheet</param>
        public TransFloor(int x, int y, int w, int h, Level level, ContentManager contentManager) : base()
        {
            Position = new Vector2(x * Global.Instance.GridSize, y * Global.Instance.GridSize);
            Width = w * Global.Instance.GridSize;
            Height = h * Global.Instance.GridSize;
            CurrentLevel = level;
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            IsSolid = true;
            Sprite = contentManager.Load<Texture2D>("GroundSheet");
        }

        /// <summary>
        /// Draws the TransFloor
        /// </summary>
        /// <param name="spriteBatch"></param>
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
                            sourceRect.Y = 3 * Global.Instance.GridSize;
                        }
                        else if (x == (int)Position.X + Width - Global.Instance.GridSize)
                        {
                            sourceRect.X = 2 * Global.Instance.GridSize;
                            sourceRect.Y = 3 * Global.Instance.GridSize;
                        }
                        else
                        {
                            sourceRect.X = 1 * Global.Instance.GridSize;
                            sourceRect.Y = 3 * Global.Instance.GridSize;
                        }
                    }
                    else if (y == (int)Position.Y + Height - Global.Instance.GridSize)
                    {
                        if (x == (int)Position.X)
                        {
                            sourceRect.X = 0 * Global.Instance.GridSize;
                            sourceRect.Y = 5 * Global.Instance.GridSize;
                        }
                        else if (x == (int)Position.X + Width - Global.Instance.GridSize)
                        {
                            sourceRect.X = 2 * Global.Instance.GridSize;
                            sourceRect.Y = 5 * Global.Instance.GridSize;
                        }
                        else
                        {
                            sourceRect.X = 1 * Global.Instance.GridSize;
                            sourceRect.Y = 5 * Global.Instance.GridSize;
                        }
                    }
                    else if (x == (int)Position.X)
                    {
                        sourceRect.X = 0 * Global.Instance.GridSize;
                        sourceRect.Y = 4 * Global.Instance.GridSize;
                    }
                    else if (x == (int)Position.X + Width - Global.Instance.GridSize)
                    {
                        sourceRect.X = 2 * Global.Instance.GridSize;
                        sourceRect.Y = 4 * Global.Instance.GridSize;
                    }
                    else
                    {
                        sourceRect.X = 1 * Global.Instance.GridSize;
                        sourceRect.Y = 4 * Global.Instance.GridSize;
                    }
                    spriteBatch.Draw(texture: Sprite, position: new Vector2(x, y), sourceRectangle: sourceRect);
                }
            }
        }
    }
}
