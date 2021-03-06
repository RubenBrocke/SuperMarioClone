﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace SuperMarioClone
{
    public class Koopa : Tangible, IMovable
    {
        //Implementation of IMovable
        public float VelocityX { get; protected set; }
        public float VelocityY { get; private set; }
        public float JumpVelocity { get; private set; }
        public float Gravity { get; private set; }

        //Properties
        public bool IsHit { get; private set; }

        //Private fields
        private float _speed;
        private ContentManager _contentManager;
        private Texture2D _spriteSheet;
        private Animator _animator;
        private SoundEffect _stompSound;
        private SoundEffect _kickSound;

        /// <summary>
        /// Constructor for Koopa, sets the position of the Koopa using the GridSize sets its SpriteSheet and Animation
        /// </summary>
        /// <param name="x">X position of the Koopa</param>
        /// <param name="y">Y position of the Koopa</param>
        /// <param name="level">Level the Koopa should be in</param>
        /// <param name="contentManager">ContentManager used to load SpriteSheet</param>
        public Koopa(float x, float y, Level level, ContentManager contentManager)
        {
            //Properties and private fields are set
            Position = new Vector2(x * Global.Instance.GridSize, y * Global.Instance.GridSize);
            CurrentLevel = level;

            _speed = 0.5f;
            VelocityX = _speed;
            Gravity = 0.3f;

            IsHit = false;
            _contentManager = contentManager;

            //Sprite, animation and hitbox are set
            _spriteSheet = contentManager.Load<Texture2D>("GreenKoopaSheet");
            _animator = new Animator(_spriteSheet, 200);
            _animator.GetTextures(0, 0, Global.Instance.GridSize, _spriteSheet.Height, 2, 1);
            _stompSound = contentManager.Load<SoundEffect>("Stomp");
            _kickSound = contentManager.Load<SoundEffect>("Kick");
            Sprite = _animator.GetCurrentTexture();
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height);
            IsSolid = true;
        }

        /// <summary>
        /// Updates Koopa
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

            //Update sprite
            UpdateSprite();
        }

        /// <summary>
        /// Updates Koopa's Hitbox to match the current Position
        /// </summary>
        private void UpdateHitbox()
        {
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Hitbox.Width, Hitbox.Height);
        }

        /// <summary>
        /// Adds Gravity to Koopa's vertical velocity
        /// </summary>
        private void AddGravity()
        {
            VelocityY += Gravity;
        }

        /// <summary>
        /// Checks if Koopa is colliding with something and sets his velocity accordingly
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
        /// Updates Koopa's current Position using his velocity
        /// </summary>
        private void UpdatePosition()
        {
            Position = new Vector2(Position.X + VelocityX, Position.Y + VelocityY);
        }

        /// <summary>
        /// Updates Koopa's Sprite using the current texture in the animation
        /// </summary>
        private void UpdateSprite()
        {
            Sprite = _animator.GetCurrentTexture();
        }

        /// <summary>
        /// Checks if Koopa should Die or hit Mario instead
        /// </summary>
        /// <param name="mario">Used to check if Mario jumped on top of Koopa</param>
        public void CheckDeath(Mario mario)
        {
            if (!IsHit)
            {
                if (mario.VelocityY > 0.5)
                {
                    mario.Jump();
                    Die();
                }
                else
                {
                    mario.GetHit();
                }
            }
        }

        /// <summary>
        /// Kills the Koopa and spawns a Shell in its place
        /// </summary>
        public void Die()
        {
            if (!IsHit)
            {
                if (_stompSound != null)
                {
                    _stompSound.Play();
                }
                CurrentLevel.ToRemoveGameObject(this);
                CurrentLevel.ToAddGameObject(new Shell(Position.X, Position.Y + Hitbox.Height - Global.Instance.GridSize, CurrentLevel, _contentManager));
                IsHit = true; 
            }
        }

        /// <summary>
        /// Kills the Koopa and doesn't spawn a shell
        /// </summary>
        public void DieWithoutShell()
        {
            if (!IsHit)
            {
                if (_kickSound != null)
                {
                    _kickSound.Play();
                }
                CurrentLevel.ToRemoveGameObject(this);
                IsHit = true;
            }
        }
    }
}

