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
        protected int _width { get; set; }
        protected int _height { get; set; }

        public Floor(float _x, float _y, int _w, int _h, Level lvl, ContentManager cm) : base()
        {
            Position = new Vector2(_x, _y);
            _width = _w;
            _height = _h;
            CurrentLevel = lvl;
            hitbox = new Rectangle((int)Position.X, (int)Position.Y, _width, _height);
            Sprite = cm.Load<Texture2D>("GroundSheet");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRect = new Rectangle(16, 16, 16, 16);
            for (int x = (int)Position.X; x < _width + (int)Position.X; x += 16)
            {
                for (int y = (int)Position.Y; y < _height + (int)Position.Y; y += 16)
                {
                    if(y == (int)Position.Y)
                    {
                        if (x == (int)Position.X)
                        {
                            sourceRect = new Rectangle(0, 0, 16, 16);
                        }
                        else if (x == (int)Position.X + _width - 16)
                        {
                            sourceRect = new Rectangle(32, 0, 16, 16);
                        }
                        else
                        {
                            sourceRect = new Rectangle(16, 0, 16, 16);
                        }
                    }
                    else if(y == (int)Position.Y + _height - 16)
                    {
                        if (x == (int)Position.X)
                        {
                            sourceRect = new Rectangle(0, 32, 16, 16);
                        }
                        else if (x == (int)Position.X + _width - 16)
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
                    else if (x == (int)Position.X + _width - 16)
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

