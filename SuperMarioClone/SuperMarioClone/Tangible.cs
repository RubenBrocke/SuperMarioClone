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
        protected Tangible collObject;
        protected int _horizontalPadding = 0;
        protected int _verticalPadding = 0;

        public Tangible() : base()
        {
            
        }

        public bool IsColliding(Level lvl, int offsetX, int offsetY, out Tangible collObject)
        {
            bool collidesWithSolid = false;
            collObject = null;
            foreach (GameObject o in lvl._gameObjects)
            {
                if(o is Tangible)
                {
                    Tangible tangibleObject = (Tangible)o;
                    Rectangle testRect = new Rectangle((int)tangibleObject.Position.X - offsetX, (int)tangibleObject.Position.Y - offsetY, tangibleObject.hitbox.Width, tangibleObject.hitbox.Height);

                    if (testRect.Intersects(hitbox) && tangibleObject != this)
                    {
                        if (o is Solid)
                        {
                            collidesWithSolid = true;
                            collObject = tangibleObject;
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
                                mysteryBlock.Eject((Mario)this, this.velocityY, this.Position.Y);
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
            bool result = false;
            //Horizontal collision
            if (IsColliding(CurrentLevel, (int)Math.Ceiling(velocityX), 0, out collObject) || IsColliding(CurrentLevel, (int)Math.Floor(velocityX), 0, out collObject))
            {
                if (!(collObject is TransFloor))
                {
                    if (velocityX < 0)
                    {
                        Position = new Vector2(collObject.hitbox.Right - _horizontalPadding, Position.Y);
                    }
                    else if (velocityX > 0)
                    {
                        Position = new Vector2(collObject.hitbox.Left - hitbox.Width - _horizontalPadding, Position.Y);
                    }
                    velocityX = 0;
                    result = true; 
                }
            }

            //Vertical collision
            if (IsColliding(CurrentLevel, 0, (int)Math.Ceiling(velocityY), out collObject) || IsColliding(CurrentLevel, 0, (int)Math.Floor(velocityY), out collObject))
            {
                if (!(collObject is TransFloor))
                {
                    if (velocityY > 0)
                    {
                        Position = new Vector2(Position.X, collObject.hitbox.Top - hitbox.Height - _verticalPadding);
                    }
                    else if (velocityY < 0)
                    {
                        Position = new Vector2(Position.X, collObject.hitbox.Bottom - _verticalPadding);
                    }
                    velocityY = 0; 
                }
                else
                {
                    if (velocityY > 0)
                    {
                        Position = new Vector2(Position.X, collObject.hitbox.Top - hitbox.Height - _verticalPadding);
                        velocityY = 0;
                    }
                }
            }
            return result;
        }
    }
}

