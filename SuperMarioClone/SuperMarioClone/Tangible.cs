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
        protected int _horizontalPadding = 1;
        protected int _verticalPadding = 2;

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
                                mysteryBlock.Eject((Mario)this, this.velocityY, this.position.Y);
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
                                g.CheckDeath(m, m.velocityY);
                            }
                        }
                    } 
                } 
            }
            return collidesWithSolid;
        }

        public virtual bool CheckCollision()
        {
            //Do collision (to be moved to parent class)
            bool result = false;
            //Horizontal collision
            if (IsColliding(currentLevel, (int)Math.Ceiling(velocityX), 0, out outRect) || IsColliding(currentLevel, (int)Math.Floor(velocityX), 0, out outRect))
            {
                if (velocityX < 0)
                {
                    position = new Vector2(outRect.Right - _horizontalPadding, position.Y);
                }
                else if (velocityX > 0)
                {
                    position = new Vector2(outRect.Left - hitbox.Width - _horizontalPadding, position.Y);
                }
                velocityX = 0;
                result = true;
            }

            //Vertical collision
            if (IsColliding(currentLevel, 0, (int)Math.Ceiling(velocityY), out outRect) || IsColliding(currentLevel, 0, (int)Math.Floor(velocityY), out outRect))
            {
                if (velocityY > 0)
                {
                    position = new Vector2(position.X, outRect.Top - hitbox.Height - _verticalPadding);
                }
                else if (velocityY < 0)
                {
                    position = new Vector2(position.X, outRect.Bottom - _verticalPadding);
                }
                velocityY = 0;
            }
            return result;
        }
    }
}

