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
            bool collidesWithSolid = false;
            Rect = Rectangle.Empty;
            foreach (GameObject o in lvl._gameObjects)
            {
                if(o is Tangible)
                {
                    Tangible Object = (Tangible)o;
                    Rectangle testRect = new Rectangle(Object.X - offsetX, Object.Y - offsetY, Object.hitbox.Width, Object.hitbox.Height);

                    if (testRect.Intersects(hitbox) && Object != this)
                    {
                        if (o is Solid)
                        {
                            collidesWithSolid = true;
                            Rect = Object.hitbox;
                        }

                        if (this is Mario)
                        {
                            if (o is Coin)
                            {
                                Coin coin = (Coin)o;
                                coin.AddCoin((Mario)this);
                            }
                            if (o is MysteryBlock)
                            {
                                MysteryBlock mysteryBlock = (MysteryBlock)o;
                                mysteryBlock.Eject((Mario)this, velocityY);
                            }
                        }
                    } 
                } 
            }
            return collidesWithSolid;
        }
    }
}

