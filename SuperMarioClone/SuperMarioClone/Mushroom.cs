﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace SuperMarioClone
{
    public class Mushroom : Tangible, IMovable
    {
        //Implementation of IMovable
        public float VelocityX { get; private set; }
        public float VelocityY { get; private set; }
        public float JumpVelocity { get; private set; }
        public float Gravity { get; private set; }

        //Properties
        private bool HasBeenPickedUp { get; set; }

        //Private fields
        private float _speed;

        /// <summary>
        /// Constructor for Mushroom, sets the position of the Mushroom using the GridSize and sets its Sprite
        /// </summary>
        /// <param name="x">X position of the Mushroom</param>
        /// <param name="y">Y position of the Mushroom</param>
        /// <param name="level">Level the Mushroom should be in</param>
        /// <param name="contentManager">ContentManager used to load Sprite</param>
        public Mushroom(int x, int y, Level level, ContentManager contentManager) : base()
        {
            //Properties and private fields are set
            Position = new Vector2(x * Global.Instance.GridSize, y * Global.Instance.GridSize);
            CurrentLevel = level;

            _speed = 1.5f;
            VelocityX = _speed;
            VelocityY = 1f;
            Gravity = 0.3f;
            
            HasBeenPickedUp = false;

            //Sprite, sound and hitbox are set
            Sprite = contentManager.Load<Texture2D>("Mushroom");
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height);
        }

        /// <summary>
        /// Makes Mario big and removes the Mushroom from the Level
        /// </summary>
        /// <param name="mario">Mario to make big</param>
        public void CollectMushroom(Mario mario)
        {
            if (!HasBeenPickedUp)
            {
                mario.BecomeBig();
                HasBeenPickedUp = true;
                CurrentLevel.ToRemoveGameObject(this);
            }
        }

        /// <summary>
        /// Updates the Mushroom
        /// </summary>
        public override void Update()
        {
            //Update hitbox to match current position
            UpdateHitbox();

            //Add gravity to vertical velocity
            AddGravity();

            //Check collision and change direction if needed
            CollisionCheck();

            //Update position
            UpdatePosition();
        }

        /// <summary>
        /// Updates Mushroom's Hitbox to match the current Position
        /// </summary>
        private void UpdateHitbox()
        {
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Hitbox.Width, Hitbox.Height);
        }

        /// <summary>
        /// Adds Gravity to the vertical velocity
        /// </summary>
        private void AddGravity()
        {
            VelocityY += Gravity;
        }

        /// <summary>
        /// Checks collision and changes direction if needed
        /// </summary>
        private void CollisionCheck()
        {
            float vX;
            float vY;

            if (CheckCollision(this, out vX, out vY))
            {
                if (VelocityX > 0)
                {
                    Direction = SpriteEffects.FlipHorizontally;
                    VelocityX = -_speed;
                }
                else
                {
                    Direction = SpriteEffects.None;
                    VelocityX = _speed;
                }
            }
            VelocityY = vY;
        }

        /// <summary>
        /// Updates the position using the velocity
        /// </summary>
        private void UpdatePosition()
        {
            Position = new Vector2(Position.X + VelocityX, Position.Y + VelocityY);
        }
    }
}
