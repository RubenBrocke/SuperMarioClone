﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Threading;


namespace SuperMarioClone
{
    public class Mario : Tangible, IMovable
    {
        //Implementation of IMovable
        public float VelocityX { get; protected set; }
        public float VelocityY { get; private set; }
        public float JumpVelocity { get; private set; }
        public float Gravity { get; private set; }

        //Properties
        public int Coins { get; private set; }
        public int Lives { get; private set; }
        public State CurrentState { get; private set; }
        public Form CurrentForm { get; private set; }

        //Private fields
        private ContentManager _contentManager;
        private float _acc;
        private float _deacc;
        private float _xSpeedMax;
        private float _ySpeedMax;
        private bool _jumpWasPressed;
        private bool _isInvincible;
        private Timer _invincibilityTimer;
        private Texture2D _spriteSheet;
        private SoundEffect _jumpSound;
        private SpriteFont _font;
        private Animator _animator;
        private int _hitboxWidth;
        private int _hitboxHeight;

        //Enumerators
        public enum State
        {
            Idle,
            Crouching,
            Walking,
            Running,
            Jumping,
            Falling,
            FallingStraight
        }

        public enum Form
        {
            Small,
            Big,
            Flower
        }

        public Mario(int x, int y, Level level, ContentManager contentManager, int lives = 3, int coins = 0) : base()
        {
            //Properties and private fields are set
            Position = new Vector2(x * Global.Instance.GridSize, y * Global.Instance.GridSize);
            CurrentLevel = level;

            JumpVelocity = 6.25f;
            Gravity = 0.3f;

            Lives = lives;
            Coins = coins;
            CurrentState = State.Idle;
            CurrentForm = Form.Small;

            _contentManager = contentManager;
            _acc = 0.1f;
            _deacc = 0.2f;
            _xSpeedMax = 3.5f;
            _ySpeedMax = 10f;
            _jumpWasPressed = false;
            _isInvincible = false;
            _invincibilityTimer = new Timer(UnInvincify);

            //Sprite, animation, sound, font and hitbox are set
            _spriteSheet = _contentManager.Load<Texture2D>("MarioSheet");
            _animator = new Animator(_spriteSheet);
            _jumpSound = _contentManager.Load<SoundEffect>("Oink1");
            _font = contentManager.Load<SpriteFont>("Font");
            _hitboxWidth = 14;
            _hitboxHeight = 20;
            _horizontalPadding = 1;
            _verticalPadding = 4;
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, _hitboxWidth, _hitboxHeight);
            
        }

        public void BecomeBig()
        {
            if (CurrentForm == Form.Small)
            {
                CurrentForm = Form.Big;
                UpdateHitBox();
            }
            else
            {
                //TODO: Add score to mario in exchange for not being able to become big (he already is)
            }

        }

        private void UpdateHitBox()
        {
            if (CurrentState == State.Crouching && CurrentForm == Form.Small)
            {
                _hitboxWidth = 14;
                _hitboxHeight = 14;
                _horizontalPadding = 1;
                _verticalPadding = 18;
            }
            else if (CurrentState == State.Crouching && CurrentForm == Form.Big)
            {
                _hitboxWidth = 14;
                _hitboxHeight = 15;
                _horizontalPadding = 2;
                _verticalPadding = 17;
            }
            else if (CurrentForm == Form.Big)
            {

                _hitboxWidth = 16;
                _hitboxHeight = 28;
                _horizontalPadding = 2;
                _verticalPadding = 4;
            }
            else
            {
                _hitboxWidth = 14;
                _hitboxHeight = 20;
                _horizontalPadding = 1;
                _verticalPadding = 12;
            }
            Hitbox = new Rectangle((int)Position.X + _horizontalPadding, (int)Position.Y + _verticalPadding, _hitboxWidth, _hitboxHeight);
        }

        public override void Update()   //TODO: split in seperate functions
        {
            //Update hitbox to match current position and State
            UpdateHitBox();

            //Add gravity to vertical velocity
            VelocityY += Gravity;

            //Limit vertical speed
            if (VelocityY > _ySpeedMax)
            {
                VelocityY = _ySpeedMax;
            }

            //Limit horizontal speed
            if (VelocityX > _xSpeedMax)
            {
                VelocityX = _xSpeedMax;
            }
            else if (VelocityX < -_xSpeedMax)
            {
                VelocityX = -_xSpeedMax;
            }

            //Add movement
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.S))
            {
                if (IsColliding(CurrentLevel, 0, 1, out collObject))
                {
                    if (!(collObject is TransFloor) || Hitbox.Y + Hitbox.Height == collObject.Hitbox.Top)
                    {
                        if (VelocityX > 0)
                        {
                            VelocityX = Math.Max(VelocityX - _deacc, 0);
                        }
                        else if (VelocityX < 0)
                        {
                            VelocityX = Math.Min(VelocityX + _deacc, 0);
                        }
                    } 
                }
            }
            else if (state.IsKeyDown(Keys.D))
            {
                if (VelocityX < 0)
                {
                    VelocityX += _acc * 2;
                }
                else
                {
                    VelocityX += _acc;
                }
                Direction = SpriteEffects.None;
            }
            else if (state.IsKeyDown(Keys.A))
            {
                if (VelocityX > 0)
                {
                    VelocityX -= _acc * 2;
                }
                else
                {
                    VelocityX -= _acc;
                } 
                Direction = SpriteEffects.FlipHorizontally;
            }
            else if (IsColliding(CurrentLevel, 0, 1, out collObject))
            {
                if (!(collObject is TransFloor) || Hitbox.Y + Hitbox.Height == collObject.Hitbox.Top)
                {
                    if (VelocityX > 0)
                    {
                        VelocityX = Math.Max(VelocityX - _deacc, 0);
                    }
                    else if (VelocityX < 0)
                    {
                        VelocityX = Math.Min(VelocityX + _deacc, 0);
                    } 
                }
            }

            //Prevents pogosticking 
            if (_jumpWasPressed)
            {
                if (!state.IsKeyDown(Keys.Space))
                {
                    _jumpWasPressed = false;
                }
            }

            if (state.IsKeyDown(Keys.Space))
            {
                if (!_jumpWasPressed)
                {
                    if (IsColliding(CurrentLevel, 0, 1, out collObject) && VelocityY <= Gravity && VelocityY >= -Gravity)
                    {
                        Jump();
                    }
                }
                _jumpWasPressed = true;
            }

            //Check collision and change velocity
            float vX;
            float vY;
            CheckCollision(this, out vX, out vY);
            VelocityX = vX;
            VelocityY = vY;

            //Update position
            Position = new Vector2(Position.X + VelocityX, Position.Y + VelocityY);

            //Focus camera on Mario
            MainGame.camera.LookAt(Position);

            //Set state
            if (VelocityX == 0)
            {
                CurrentState = Mario.State.Idle;
            }
            if (VelocityX != 0)
            {
                CurrentState = Mario.State.Walking;
            }
            if (VelocityX > 3.4f || VelocityX < -3.4f)
            {
                CurrentState = Mario.State.Running;
            }
            if (VelocityY < 0)
            {
                CurrentState = Mario.State.Jumping;
            }
            if (VelocityY > 0)
            {
                CurrentState = Mario.State.Falling;
            }
            if (VelocityY > 0 && VelocityX < 0.5f && VelocityX > -0.5f)
            {
                CurrentState = Mario.State.FallingStraight;
            }
            if (state.IsKeyDown(Keys.S))
            {
                CurrentState = State.Crouching;
            }

            //Update sprite
            UpdateSprite();
        }

        public void Jump()
        {
            VelocityY = -JumpVelocity;
            if (_jumpSound != null)
            {
                _jumpSound.Play();
            }   
        }

        public void GetHit()
        {
            if (!_isInvincible)
            {
                switch (CurrentForm)
                {
                    case Form.Small:
                        Die();
                        break;
                    case Form.Big:
                        _isInvincible = true;
                        _invincibilityTimer.Change(300, Timeout.Infinite);
                        CurrentForm--;
                        break;
                    case Form.Flower:
                        CurrentForm--;
                        break;
                    default:
                        break;
                } 
            }
        }

        private void UnInvincify(object state)
        {
            _isInvincible = false;
            _invincibilityTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        public void Die()
        {
            Lives--;
            LevelReader lr = new LevelReader(_contentManager);
            MainGame.currentLevel = lr.ReadLevel(0);
            //TODO: Add death animation and death screen showing lives before going back to the menu/level selection
        }

        public void AddCoin()
        {
            Coins++;
        }

        private void UpdateSprite()
        {
            int spriteWidth = 16;
            int spriteHeight = 32;
            int offSetY = 0;
            if (CurrentForm == Form.Big)
            {
                spriteWidth = 20;
                spriteHeight = 32;
                offSetY = 32;
            }
            switch (CurrentState)
            {
                case State.Idle:
                    _animator.SetAnimationSpeed(0);
                    _animator.GetTextures(6 * spriteWidth, offSetY, spriteWidth, spriteHeight, 1, 1);
                    break;
                case State.Crouching:
                    _animator.SetAnimationSpeed(0);
                    _animator.GetTextures(5 * spriteWidth, offSetY, spriteWidth, spriteHeight, 1, 1);
                    break;
                case State.Walking:
                    if (CurrentForm == Form.Big)
                    {
                        _animator.SetAnimationSpeed(190);
                        _animator.GetTextures(6 * spriteWidth, offSetY, spriteWidth, spriteHeight, 3, 1);
                    }
                    else
                    {
                        _animator.SetAnimationSpeed(190);
                        _animator.GetTextures(6 * spriteWidth, offSetY, spriteWidth, spriteHeight, 2, 1);
                    }
                    break;
                case State.Running:
                    _animator.SetAnimationSpeed(80);
                    _animator.GetTextures(0 * spriteWidth, offSetY, spriteWidth, spriteHeight, 2, 1);
                    break;
                case State.Jumping:
                    _animator.SetAnimationSpeed(0);
                    _animator.GetTextures(3 * spriteWidth, offSetY, spriteWidth, spriteHeight, 1, 1);
                    break;
                case State.Falling:
                    _animator.SetAnimationSpeed(0);
                    _animator.GetTextures(2 * spriteWidth, offSetY, spriteWidth, spriteHeight, 1, 1);
                    break;
                case State.FallingStraight:
                    _animator.SetAnimationSpeed(0);
                    _animator.GetTextures(4 * spriteWidth, offSetY, spriteWidth, spriteHeight, 1, 1);
                    break;
                default:
                    _animator.SetAnimationSpeed(0);
                    _animator.GetTextures(6 * spriteWidth, offSetY, spriteWidth, spriteHeight, 1, 1);
                    break;
            }
            Sprite = _animator.GetCurrentTexture();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin();
            spriteBatch.DrawString(_font, String.Format("{0,4}", Coins), new Vector2(768, 0), Color.Black);
            spriteBatch.DrawString(_font, String.Format("{0,4}", Lives), new Vector2(704, 0), Color.Black);
            spriteBatch.End();
            spriteBatch.Begin(transformMatrix: MainGame.camera.GetMatrix(), samplerState: SamplerState.PointClamp);
        }
    }
}