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
        private int _width { get; set; }
        private int _height { get; set; }

        public Floor(int _x, int _y, int _w, int _h, Level lvl, ContentManager cm) : base()
        {
            X = _x;
            Y = _y;
            _width = _w;
            _height = _h;
            currentLevel = lvl;
            hitbox = new Rectangle(X, Y, _width, _height);
            sprite = cm.Load<Texture2D>("GroundSheet");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = X; i < _width + X; i += 16)
            {
                for (int y = Y; y < _height + Y; y += 16)
                {
                    if(y == Y)
                    {
                        if (i == X)
                        {
                            spriteBatch.Draw(sprite, new Vector2(X, Y), sourceRectangle: new Rectangle(0, 48, 16, 16));
                        }
                        else if (i == X + _width - 16)
                        {
                            spriteBatch.Draw(sprite, new Vector2(X + _width - 16, Y), sourceRectangle: new Rectangle(32, 48, 16, 16));
                        }
                        else
                        {
                            spriteBatch.Draw(sprite, new Vector2(i, y), sourceRectangle: new Rectangle(16, 0, 16, 16));
                        }
                    }
                    else
                    {
                        spriteBatch.Draw(sprite, new Vector2(i, y), sourceRectangle: new Rectangle(16, 16, 16, 16));
                    }
                    
                }
            }
            
        }
    } 
}

