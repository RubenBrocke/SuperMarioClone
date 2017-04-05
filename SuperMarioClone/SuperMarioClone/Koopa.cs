using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace SuperMarioClone
{
    public class Koopa : Tangible, IMovable, ISolid
    {
        public float VelocityX { get; protected set; }
        public float VelocityY { get; private set; }
        public float JumpVelocity { get; private set; }
        public float Gravity { get; private set; }
        public bool IsHit { get; private set; }

        private float _speed;
        private ContentManager _contentManager;
        private Texture2D _spriteSheet;
        private Animator _animator;

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

        public void Die()
        {
            if (!IsHit)
            {
                CurrentLevel.ToRemoveGameObject(this);
                CurrentLevel.ToAddGameObject(new Shell(Position.X, Position.Y + Hitbox.Height - Global.Instance.GridSize, CurrentLevel, _contentManager));
                IsHit = true; 
            }
        }

        public void DieWithoutShell()
        {
            if (!IsHit)
            {
                CurrentLevel.ToRemoveGameObject(this);
                IsHit = true;
            }
        }
    }
}

