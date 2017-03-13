using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioClone
{
    public abstract class Moveable : GameObject
    {
        protected float VelocityX { get; set; }

        protected float VelocityY { get; set; }

        protected float JumpVelocity { get; set; }

        protected float gravity = 0.3f; 

        public Moveable() : base()
        {

        }

        public void MoveLeft()
        {
            Position = new Vector2(Position.X - VelocityX, Position.Y);
        }

        public void MoveRight()
        {
            Position = new Vector2(Position.X + VelocityX, Position.Y);
        }

        public virtual void Jump()
        {
            Position = new Vector2(Position.X, Position.Y - VelocityY);
        }

    } 
}

