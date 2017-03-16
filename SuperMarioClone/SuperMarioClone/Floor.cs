using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SuperMarioClone
{
    public class Floor : Solid
    {
        protected int Width { get; set; }
        protected int Height { get; set; }

        public Floor(float x, float y, int w, int h, Level lvl, ContentManager cm) : base()
        {
            Position = new Vector2(x, y);
            Width = w;
            Height = h;
            CurrentLevel = lvl;
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            Sprite = cm.Load<Texture2D>("GroundSheet");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRect = new Rectangle(16, 16, 16, 16); //properties aanpassen ipv new Rectangle
            for (int x = (int)Position.X; x < Width + (int)Position.X; x += 16)
            {
                for (int y = (int)Position.Y; y < Height + (int)Position.Y; y += 16)
                {
                    if(y == (int)Position.Y)
                    {
                        if (x == (int)Position.X)
                        {
                            sourceRect = new Rectangle(0, 0, 16, 16);
                        }
                        else if (x == (int)Position.X + Width - 16)
                        {
                            sourceRect = new Rectangle(32, 0, 16, 16);
                        }
                        else
                        {
                            sourceRect = new Rectangle(16, 0, 16, 16);
                        }
                    }
                    else if(y == (int)Position.Y + Height - 16)
                    {
                        if (x == (int)Position.X)
                        {
                            sourceRect = new Rectangle(0, 32, 16, 16);
                        }
                        else if (x == (int)Position.X + Width - 16)
                        {
                            sourceRect = new Rectangle(32, 32, 16, 16);
                        }
                        else
                        {
                            sourceRect = new Rectangle(16, 32, 16, 16);
                        }
                    }
                    else if (x == (int)Position.X)
                    {
                        sourceRect = new Rectangle(0, 16, 16, 16);
                    }
                    else if (x == (int)Position.X + Width - 16)
                    {
                        sourceRect = new Rectangle(32, 16, 16, 16);
                    }
                    else
                    {
                        sourceRect = new Rectangle(16, 16, 16, 16);
                    }
                    spriteBatch.Draw(texture: Sprite, position: new Vector2(x, y), sourceRectangle: sourceRect);
                }
            }
        }
    } 
}

