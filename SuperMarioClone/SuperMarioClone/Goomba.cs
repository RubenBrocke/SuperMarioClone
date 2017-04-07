using System;
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
    public class Goomba : Tangible, IMovable
    {
        //Implementation of IMovable
        public float VelocityX { get; private set; }
        public float VelocityY { get; private set; }
        public float JumpVelocity { get; private set; }
        public float Gravity { get; private set; }

        //Private fields
        private float _speed;
        private bool _isHit;
        private Texture2D _spriteSheet;
        private Animator _animator;
        private SoundEffect _stompSound;

        /// <summary>
        /// Constructor for Goomba, sets the position of the Goomba using the GridSize sets its SpriteSheet and Animation
        /// </summary>
        /// <param name="x">X position of the Goomba</param>
        /// <param name="y">Y position of the Goomba</param>
        /// <param name="level">Level the Goomba should be in</param>
        /// <param name="contentManager">ContentManager used to load SpriteSheet</param>
        public Goomba(int x, int y, Level level, ContentManager contentManager)
        {
            //Properties and private fields are set
            Position = new Vector2(x * Global.Instance.GridSize, y * Global.Instance.GridSize);
            CurrentLevel = level;

            _speed = 2f;
            VelocityX = -_speed;
            Gravity = 0.3f;

            _isHit = false;
            _horizontalPadding = 1;

            //Sprite, animation and hitbox are set
            _spriteSheet = contentManager.Load<Texture2D>("GoombaSheet");
            _animator = new Animator(_spriteSheet, 110);
            _animator.GetTextures(0, 0, Global.Instance.GridSize, Global.Instance.GridSize, 2, 1);
            _stompSound = contentManager.Load<SoundEffect>("Stomp");
            Sprite = _animator.GetCurrentTexture();          
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height);
        }

        /// <summary>
        /// Updates Goomba
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
        /// Updates Goomba's Hitbox to match the current Position
        /// </summary>
        private void UpdateHitbox()
        {
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Hitbox.Width, Hitbox.Height);
        }

        /// <summary>
        /// Adds Gravity to Goomba's vertical velocity
        /// </summary>
        private void AddGravity()
        {
            VelocityY += Gravity;
        }

        /// <summary>
        /// Checks if Goomba is colliding with something and sets his velocity accordingly
        /// </summary>
        private void CollisionCheck()
        {
            float vX;
            float vY;

            if (CheckCollision(this, out vX, out vY))
            {
                if (VelocityX > 0)
                {
                    Direction = SpriteEffects.None;
                    VelocityX = -_speed;
                }
                else
                {
                    Direction = SpriteEffects.FlipHorizontally;
                    VelocityX = _speed;
                }
            }

            VelocityY = vY;
        }

        /// <summary>
        /// Updates Goomba's current Position using his velocity
        /// </summary>
        private void UpdatePosition()
        {
            Position = new Vector2(Position.X + VelocityX, Position.Y + VelocityY);
        }
        
        /// <summary>
        /// Updates Goomba's Sprite using the current texture in the animation
        /// </summary>
        private void UpdateSprite()
        {
            Sprite = _animator.GetCurrentTexture();
        }

        /// <summary>
        /// Checks if Goomba should Die or hit Mario instead
        /// </summary>
        /// <param name="mario">Used to check if Mario jumped on top of Goomba</param>
        public void CheckDeath(Mario mario)
        {
            if (!_isHit)
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
        /// Kills the Goomba
        /// </summary>
        public void Die()
        {
            if (!_isHit)
            {
                if (_stompSound != null)
                {
                    _stompSound.Play();
                }
                CurrentLevel.ToRemoveGameObject(this);
                _isHit = true;
            }
        }
    }
}
