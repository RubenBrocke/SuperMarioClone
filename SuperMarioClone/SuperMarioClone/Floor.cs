﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioClone
{
    public class Floor : Solid
    {
        private int _width { get; set; }
        private int _height { get; set; }

        public Floor(int _x, int _y, int _w, int _h) : base()
        {
            X = _x;
            Y = _y;
            _width = _w;
            _height = _h;
            hitbox = new Rectangle(X, Y, _width, _height);
            sprite = ContentLoader.loadTexture("Thicc");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int i = X; i < _width + X; i += sprite.Width)
            {
                for (int y = Y; y < _height + Y; y += sprite.Height)
                {
                    spriteBatch.Draw(sprite, new Vector2(i, y));
                }
            }
            spriteBatch.End();
        }
    } 
}

