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

        public Floor(float _x, float _y, int _w, int _h, Level lvl, ContentManager cm) : base()
        {
            position = new Vector2(_x, _y);
            _width = _w;
            _height = _h;
            currentLevel = lvl;
            hitbox = new Rectangle((int)position.X, (int)position.Y, _width, _height);
            sprite = cm.Load<Texture2D>("GroundSheet");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = (int)position.X; i < _width + (int)position.X; i += 16)
            {
                for (int y = (int)position.Y; y < _height + (int)position.Y; y += 16)
                {
                    if(y == (int)position.Y)
                    {
                        if (i == (int)position.X)
                        {
                            spriteBatch.Draw(texture: sprite, position: position, sourceRectangle: new Rectangle(0, 48, 16, 16));
                        }
                        else if (i == (int)position.X + _width - 16)
                        {
                            spriteBatch.Draw(texture: sprite, position: new Vector2(position.X + _width - 16, position.Y), sourceRectangle: new Rectangle(32, 48, 16, 16));
                        }
                        else
                        {
                            spriteBatch.Draw(texture: sprite, position: new Vector2(i, y), sourceRectangle: new Rectangle(16, 0, 16, 16));
                        }
                    }
                    else
                    {
                        spriteBatch.Draw(texture: sprite, position: new Vector2(i, y), sourceRectangle: new Rectangle(16, 16, 16, 16));
                    }
                    
                }
            }
            
        }
    } 
}

