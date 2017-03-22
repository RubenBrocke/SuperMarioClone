using System;
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
        public int Coins { get; private set; }
        public int Lives { get; private set; }
        public State CurrentState { get; private set; }
        public Form CurrentForm { get; private set; }

        public float VelocityX { get; protected set; }
        public float VelocityY { get; private set; }
        public float JumpVelocity { get; private set; }
        public float Gravity { get; private set; }

        private bool _jumpWasPressed = false;
        private SoundEffect _jumpSound;
        private float _xSpeedMax = 3.5f;
        private float _ySpeedMax = 10;
        private float _acc = 0.1f;
        private float _deacc = 0.2f;

        private SpriteFont _font;

        private Animator _animator;

        private int _hitboxWidth = 14;
        private int _hitboxHeight = 20;

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
            Flower,
            Tanuki
        }

        public Mario(int x, int y, Level level, ContentManager contentManager, int lives = 3, int coins = 0) : base()
        {
            Position = new Vector2(x, y);
            _jumpSound = contentManager.Load<SoundEffect>("Oink1");
            _font = contentManager.Load<SpriteFont>("Font");
            _animator = new Animator(contentManager.Load<Texture2D>("MarioSheetRight"));
            CurrentLevel = level;
            JumpVelocity = 6.25f;
            CurrentState = State.Idle;
            CurrentForm = Form.Small;
            Gravity = 0.3f;
            _horizontalPadding = 1;
            _verticalPadding = 4;
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, _hitboxWidth, _hitboxHeight);
            Lives = lives;
            Coins = coins;
        }

        public void BecomeBig()
        {
            if (CurrentForm == Form.Small)
            {
                CurrentForm = Form.Big;
            }
            else
            {
                //Add score to mario in exchange for not being able to become big (he already is)
            }

        }

        public override void Update()
        {
            //Update Hitbox
            if (CurrentState == State.Crouching)
            {
                _verticalPadding = 10;
                _hitboxHeight = 14;
            }
            else
            {
                _verticalPadding = 4;
                _hitboxHeight = 20;
            }
            Hitbox = new Rectangle((int)Position.X + _horizontalPadding, (int)Position.Y + _verticalPadding, _hitboxWidth, _hitboxHeight);
            //Add gravity
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
            if (state.IsKeyDown(Keys.Space))
            {
                if (!_jumpWasPressed)
                {
                    Jump();
                }
                _jumpWasPressed = true;
            }

            float vX;
            float vY;
            CheckCollision(this, out vX, out vY);
            VelocityX = vX;
            VelocityY = vY;

            //Add speed to position
            Position = new Vector2(Position.X + VelocityX, Position.Y + VelocityY);

            //Focus camera on Mario
            MainGame.camera.LookAt(Position);

            //Prevents pogosticking
            if (VelocityY == 0)
            {
                _jumpWasPressed = false;
            }

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
            UpdateSprite();
        }

        public void Jump()
        {
            if (IsColliding(CurrentLevel, 0, 1, out collObject))
            {
                VelocityY = -JumpVelocity;
                _jumpSound.Play();
            }
        }

        public void LoseLife()
        {
            Lives--;
        }

        public void AddCoin()
        {
            Coins++;
        }

        private void UpdateSprite()
        {
            switch (CurrentState)
            {
                case State.Idle:
                    _animator.SetAnimationSpeed(0);
                    _animator.GetTextures(0, 0, 16, 24, 1, 1);
                    break;
                case State.Crouching:
                    _animator.SetAnimationSpeed(0);
                    _animator.GetTextures(112, 0, 16, 24, 1, 1);
                    break;
                case State.Walking:
                    _animator.SetAnimationSpeed(190);
                    _animator.GetTextures(0, 0, 16, 24, 2, 1);
                    break;
                case State.Running:
                    _animator.SetAnimationSpeed(80);
                    _animator.GetTextures(32, 0, 16, 24, 2, 1);
                    break;
                case State.Jumping:
                    _animator.SetAnimationSpeed(0);
                    _animator.GetTextures(80, 0, 16, 24, 1, 1);
                    break;
                case State.Falling:
                    _animator.SetAnimationSpeed(0);
                    _animator.GetTextures(64, 0, 16, 24, 1, 1);
                    break;
                case State.FallingStraight:
                    _animator.SetAnimationSpeed(0);
                    _animator.GetTextures(96, 0, 16, 24, 1, 1);
                    break;
                default:
                    _animator.SetAnimationSpeed(0);
                    _animator.GetTextures(0, 0, 16, 24, 1, 1);
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