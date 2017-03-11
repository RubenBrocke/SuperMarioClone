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
        protected float velocityX { get; set; }

        protected float velocityY { get; set; }

        protected float jumpVelocity { get; set; }

        protected float gravity = 0.3f; 

        public Moveable() : base()
        {

        }

        public void MoveLeft()
        {
            position = new Vector2(position.X - velocityX, position.Y);
        }

        public void MoveRight()
        {
            position = new Vector2(position.X + velocityX, position.Y);
        }

        public virtual void Jump()
        {
            position = new Vector2(position.X, position.Y - velocityY);
        }

    } 
}

