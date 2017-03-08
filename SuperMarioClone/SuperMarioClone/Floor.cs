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
            sprite = cm.Load<Texture2D>("Thicc");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = X; i < _width + X; i += sprite.Width)
            {
                for (int y = Y; y < _height + Y; y += sprite.Height)
                {
                    spriteBatch.Draw(sprite, new Vector2(i, y));
                }
            }
        }
    } 
}

