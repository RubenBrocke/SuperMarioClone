using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioClone
{
    public abstract class Tangible : Moveable
    {
        protected Rectangle hitbox { get; set; }

        public Tangible() : base()
        {

        }

        public bool IsColliding(Level lvl, int offsetX, int offsetY, out Rectangle Rect)
        {
            bool succes = false;
            Rect = Rectangle.Empty;
            foreach (Tangible Object in lvl._gameObjects)
            {
                Rectangle testRect = new Rectangle(Object.X - offsetX, Object.Y - offsetY, Object.hitbox.Width, Object.hitbox.Height);

                if (testRect.Intersects(hitbox) && Object != this)
                {
                    succes = true;
                    Rect = Object.hitbox;
                    break;
                }
                else
                {
                    succes = false;
                }
            }
            return succes;
        }
    }
}

