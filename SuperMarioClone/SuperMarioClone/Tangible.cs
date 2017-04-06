using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioClone
{
    public abstract class Tangible : GameObject
    {
        public Rectangle Hitbox { get; set; }
        protected Tangible collObject;
        protected int _horizontalPadding = 0;
        protected int _verticalPadding = 0;

        public Tangible() : base()
        {
            
        }

        public bool IsColliding(Level lvl, int offsetX, int offsetY, out Tangible collObject) //TODO: Improve collision (collider class?)
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
                        if (o is ISolid)
                        {
                            collidesWithSolid = true;
                            collObject = tangibleObject;
                        }
                        if (this is Mario)
                        {
                            Mario mario = (Mario)this;
                            if (o is Coin)
                            {
                                Coin coin = (Coin)o;
                                coin.AddCoin((Mario)this);
                            }
                            else if (o is MysteryBlock)
                            {
                                MysteryBlock mysteryBlock = (MysteryBlock)o;
                                mysteryBlock.Eject(mario);
                            }
                            else if (o is CoinBlock)
                            {
                                CoinBlock coinBlock = (CoinBlock)o;
                                coinBlock.Eject(mario, mario.VelocityY, mario.Position.Y);
                            }
                            else if (o is Mushroom)
                            {
                                Mushroom mushroom = (Mushroom)o;
                                mushroom.CollectMushroom(mario);
                            }
                            else if (o is Shell)
                            {
                                Shell shell = (Shell)o;
                                shell.CheckHit(mario);
                            }
                            else if (o is Koopa)
                            {
                                Koopa koopa = (Koopa)o;
                                koopa.CheckDeath(mario);
                            }
                            else if (o is Muncher)
                            {
                                mario.GetHit();
                            }
                            else if (o is Goomba)
                            {
                                Goomba goomba = (Goomba)o;
                                goomba.CheckDeath(mario);
                            }
                        }
                        else if (this is Shell)
                        {
                            if (o is Goomba)
                            {
                                Goomba goomba = (Goomba)o;
                                goomba.Die();
                            }
                            if (o is Koopa)
                            {
                                Koopa koopa = (Koopa)o;
                                koopa.DieWithoutShell();
                                collidesWithSolid = false;
                            }
                        }
                    } 
                } 
            }
            return collidesWithSolid;
        }

        public bool CheckCollision(IMovable movableObject, out float velocityX, out float velocityY)
        {
            bool result = false;
            velocityX = movableObject.VelocityX;
            velocityY = movableObject.VelocityY;

            //Horizontal collision
            if (IsColliding(CurrentLevel, (int)Math.Ceiling(velocityX), 0, out collObject) || IsColliding(CurrentLevel, (int)Math.Floor(velocityX), 0, out collObject))
            {
                if (!(collObject is TransFloor || collObject is CloudBlock))
                {
                    if (velocityX < 0)
                    {
                        Position = new Vector2(collObject.Hitbox.Right - _horizontalPadding, Position.Y);
                    }
                    else if (velocityX > 0)
                    {
                        Position = new Vector2(collObject.Hitbox.Left - Hitbox.Width - _horizontalPadding, Position.Y);
                    }
                    velocityX = 0;
                    result = true; 
                }
            }

            //Vertical collision
            if (IsColliding(CurrentLevel, 0, (int)Math.Ceiling(velocityY), out collObject) || IsColliding(CurrentLevel, 0, (int)Math.Floor(velocityY), out collObject))
            {
                if (!(collObject is TransFloor || collObject is CloudBlock))
                {
                    if (velocityY >= 0 && Hitbox.Y <= collObject.Hitbox.Top - Hitbox.Height)
                    {
                        Position = new Vector2(Position.X, collObject.Hitbox.Top - Hitbox.Height - _verticalPadding);
                    }
                    else if (velocityY < 0)
                    {
                        Position = new Vector2(Position.X, collObject.Hitbox.Bottom - _verticalPadding);
                    } //TODO: FIX Koopa jump collision (probably has to do with velocityY reset)
                    velocityY = 0; 
                }
                else
                {
                    if (velocityY >= 0 && Hitbox.Y <= collObject.Hitbox.Top - Hitbox.Height)
                    {
                        Position = new Vector2(Position.X, collObject.Hitbox.Top - Hitbox.Height - _verticalPadding);
                        velocityY = 0;
                    }
                }
            }
            return result;
        }
    }
}

