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
        protected Rectangle outRect;

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
                    Rectangle testRect = new Rectangle((int)Object.position.X - offsetX, (int)Object.position.Y - offsetY, Object.hitbox.Width, Object.hitbox.Height);

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
                            if (o is Mushroom)
                            {
                                Mushroom mushroom = (Mushroom)o;
                                mushroom.collectMushroom((Mario)this);
                            }
                        }
                        if (this is Goomba)
                        {
                            if (o is Mario)
                            {
                                Goomba g = (Goomba)this;
                                Mario m = (Mario)o;
                                g.CheckCollision(m, m.velocityY);
                            }
                        }
                    } 
                } 
            }
            return collidesWithSolid;
        }
    }
}

