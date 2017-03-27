using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace SuperMarioClone
{
    class Shell : Tangible, IMovable, ISolid
    {
        //Implementation of IMovable
        public float VelocityX { get; protected set; }
        public float VelocityY { get; private set; }
        public float JumpVelocity { get; private set; }
        public float Gravity { get; private set; }

        //Private fields
        private float _speed;
        private Animator _animator;
        private Texture2D _spriteSheet;

        public Shell(float x, float y, Level level, ContentManager contentManager) : base()
        {
            //Properties and private fields are set
            Position = new Vector2(x, y);
            CurrentLevel = level;

            Gravity = 0.3f;

            _speed = 3f;

            //Sprite, animation and hitbox are set
            _spriteSheet = contentManager.Load<Texture2D>("GreenShellSheet");
            _animator = new Animator(_spriteSheet, 110);
            _animator.GetTextures(0, 0, 16, 16, 4, 1);
            _animator.PauseAnimation();
            Sprite = _animator.GetCurrentTexture();
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height);
        }

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

        private void UpdateHitbox()
        {
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Hitbox.Width, Hitbox.Height);
        }

        private void AddGravity()
        {
            VelocityY += Gravity;
        }

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

        private void UpdatePosition()
        {
            Position = new Vector2(Position.X + VelocityX, Position.Y + VelocityY);
        }

        private void UpdateSprite()
        {
            Sprite = _animator.GetCurrentTexture();
        }

        public void CheckHit(Mario mario)
        {
            if (mario.VelocityY > 0.5)
            {
                VelocityX = 0;
                _animator.PauseAnimation();
            }
            else
            {
                if (VelocityX == 0 && mario.Hitbox.Y > Hitbox.Y - mario.Hitbox.Height)
                 {
                    if (mario.VelocityX > 0)
                    {
                        VelocityX = _speed;
                        _animator.UnpauseAnimation();
                    }
                    else if (mario.VelocityX < 0)
                    {
                        VelocityX = -_speed;
                        _animator.UnpauseAnimation();
                    }
                }
                else
                {
                    if (mario.Hitbox.Y >= Hitbox.Y - Hitbox.Height)
                    {
                        mario.GetHit();
                    }
                }
            }
        }
    }
}
