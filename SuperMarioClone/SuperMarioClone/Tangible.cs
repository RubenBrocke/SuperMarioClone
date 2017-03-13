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
        public Rectangle Hitbox { get; set; }
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
            foreach (GameObject o in lvl.GameObjects)
            {
                if(o is Tangible)
                {
                    Tangible tangibleObject = (Tangible)o;
                    Rectangle testRect = new Rectangle((int)tangibleObject.Position.X - offsetX, (int)tangibleObject.Position.Y - offsetY, tangibleObject.Hitbox.Width, tangibleObject.Hitbox.Height);

                    if (testRect.Intersects(Hitbox) && tangibleObject != this)
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
                                mysteryBlock.Eject((Mario)this, this.VelocityY, this.Position.Y);
                            }
                            if (o is Mushroom)
                            {
                                Mushroom mushroom = (Mushroom)o;
                                mushroom.CollectMushroom((Mario)this);
                            }
                        }
                        if (this is Goomba)
                        {
                            if (o is Mario)
                            {
                                Goomba g = (Goomba)this;
                                Mario m = (Mario)o;
                                g.CheckDeath(m, m.VelocityY);
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
            if (IsColliding(CurrentLevel, (int)Math.Ceiling(VelocityX), 0, out collObject) || IsColliding(CurrentLevel, (int)Math.Floor(VelocityX), 0, out collObject))
            {
                if (!(collObject is TransFloor))
                {
                    if (VelocityX < 0)
                    {
                        Position = new Vector2(collObject.Hitbox.Right - _horizontalPadding, Position.Y);
                    }
                    else if (VelocityX > 0)
                    {
                        Position = new Vector2(collObject.Hitbox.Left - Hitbox.Width - _horizontalPadding, Position.Y);
                    }
                    VelocityX = 0;
                    result = true; 
                }
            }

            //Vertical collision
            if (IsColliding(CurrentLevel, 0, (int)Math.Ceiling(VelocityY), out collObject) || IsColliding(CurrentLevel, 0, (int)Math.Floor(VelocityY), out collObject))
            {
                if (!(collObject is TransFloor))
                {
                    if (VelocityY > 0)
                    {
                        Position = new Vector2(Position.X, collObject.Hitbox.Top - Hitbox.Height - _verticalPadding);
                    }
                    else if (VelocityY < 0)
                    {
                        Position = new Vector2(Position.X, collObject.Hitbox.Bottom - _verticalPadding);
                    }
                    VelocityY = 0; 
                }
                else
                {
                    if (VelocityY > 0)
                    {
                        Position = new Vector2(Position.X, collObject.Hitbox.Top - Hitbox.Height - _verticalPadding);
                        VelocityY = 0;
                    }
                }
            }
            return result;
        }
    }
}

