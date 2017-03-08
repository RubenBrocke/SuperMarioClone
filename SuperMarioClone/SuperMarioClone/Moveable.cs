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
            X += (int)velocityX;
        }

        public void MoveRight()
        {
            X += (int)velocityX;
        }

        public void Jump()
        {
            Y -= (int)jumpVelocity;
        }

    } 
}

