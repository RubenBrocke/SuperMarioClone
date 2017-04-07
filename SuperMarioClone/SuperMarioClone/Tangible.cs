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
        //Properties
        public Rectangle Hitbox { get; set; }
        public bool IsSolid { get; set; }

        //Protected fields
        protected Tangible collObject;
        protected int _horizontalPadding = 0;
        protected int _verticalPadding = 0;

        public Tangible() : base()
        {
            IsSolid = false;
        }

        public bool IsColliding(Level lvl, int offsetX, int offsetY, out Tangible collObject) //TODO: Improve collision (collider class?)
        {
            bool collidesWithSolid = false;
            collObject = null;
            foreach (GameObject gameObject in lvl.GameObjects)
            {
                if(gameObject is Tangible)
                {
                    Tangible tangibleObject = (Tangible)gameObject;
                    Rectangle testRect = new Rectangle((int)tangibleObject.Position.X - offsetX, (int)tangibleObject.Position.Y - offsetY, tangibleObject.Hitbox.Width, tangibleObject.Hitbox.Height);

                    if (testRect.Intersects(Hitbox) && tangibleObject != this)
                    {
                        if (tangibleObject.IsSolid)
                        {
                            collidesWithSolid = true;
                            collObject = tangibleObject;
                        }
                        if (this is Mario)
                        {
                            Mario mario = (Mario)this;
                            if (tangibleObject is Coin)
                            {
                                Coin coin = (Coin)tangibleObject;
                                coin.AddCoin((Mario)this);
                            }
                            else if (tangibleObject is MysteryBlock)
                            {
                                MysteryBlock mysteryBlock = (MysteryBlock)tangibleObject;
                                mysteryBlock.Eject(mario);
                            }
                            else if (tangibleObject is CoinBlock)
                            {
                                CoinBlock coinBlock = (CoinBlock)tangibleObject;
                                coinBlock.Eject(mario);
                            }
                            else if (tangibleObject is Mushroom)
                            {
                                Mushroom mushroom = (Mushroom)tangibleObject;
                                mushroom.CollectMushroom(mario);
                            }
                            else if (tangibleObject is OneUpMushroom)
                            {
                                OneUpMushroom oneUpMushroom = (OneUpMushroom)tangibleObject;
                                oneUpMushroom.CollectMushroom(mario);
                            }
                            else if (tangibleObject is Shell)
                            {
                                Shell shell = (Shell)tangibleObject;
                                shell.CheckHit(mario);
                            }
                            else if (tangibleObject is Koopa)
                            {
                                Koopa koopa = (Koopa)tangibleObject;
                                koopa.CheckDeath(mario);
                            }
                            else if (tangibleObject is Muncher)
                            {
                                mario.GetHit();
                            }
                            else if (tangibleObject is Goomba)
                            {
                                Goomba goomba = (Goomba)tangibleObject;
                                goomba.CheckDeath(mario);
                            }
                        }
                        else if (this is Shell)
                        {
                            if (tangibleObject is Goomba)
                            {
                                Goomba goomba = (Goomba)tangibleObject;
                                goomba.Die();
                            }
                            if (tangibleObject is Koopa)
                            {
                                Koopa koopa = (Koopa)tangibleObject;
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

