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
        private Texture2D _spriteSheet;
        private SoundEffect _jumpSound;
        private SpriteFont _font;
        private Animator _animator;
        private int _hitboxWidth;
        private int _hitboxHeight;

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

            //Sprite, animation, sound, font and hitbox are set
            _spriteSheet = _contentManager.Load<Texture2D>("MarioSheetRight");
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
            }
            else
            {
                //Add score to mario in exchange for not being able to become big (he already is)
            }

        }

        public override void Update()
        {
            //Update hitbox to match current position and State
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
                    if (IsColliding(CurrentLevel, 0, 1, out collObject))
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
            _jumpSound.Play();
        }

        public void LoseLife()
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