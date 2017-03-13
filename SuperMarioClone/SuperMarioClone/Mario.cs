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
    public class Mario : Tangible
    {
	    private int Coins { get; set; }

        private int Lives { get; set; }

        private bool _jumpWasPressed = false;
        private SoundEffect _jumpSound;
        private float _xSpeedMax = 3.5f;
        private float _ySpeedMax = 10;
        private float _acc = 0.1f;
        private float _deacc = 0.2f;

        private SpriteFont _font;

        private int _animationState = 0;
        private int _spriteImageIndex = 0;
        private bool _isAnimated = false;

        private int _hitboxWidth = 14;
        private int _hitboxHeight = 20;

        private Timer _timer;

        private enum State
        {
            Idle,
            Walking,
            Running,
            Jumping,
            Falling,
            FallingStraight
        }

        private enum Form
        {
            Small,
            Big,
            Flower,
            Tanuki
        }

        private State _state;
        private Form _form;

        //----Hitbox----//
        public Rectangle outRect;

        public Mario(int _x, int _y, Level lvl, ContentManager cm) : base()
        {
            Position = new Vector2(_x, _y);
            Sprite = cm.Load<Texture2D>("MarioSheetRight");
            _jumpSound = cm.Load<SoundEffect>("Oink1");
            _font = cm.Load<SpriteFont>("Font");
            CurrentLevel = lvl;
            JumpVelocity = 6.25f;
            _state = State.Idle;
            _form = Form.Small;
            _timer = new Timer(ChangeAnimationState);
            _timer.Change(0, 100);

            _horizontalPadding = 1;
            _verticalPadding = 2;
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, _hitboxWidth, _hitboxHeight);
            Lives = 3;
            Coins = 0;
        }

        public void becomeBig()
        {
            if (_form == Form.Small)
            {
                _form = Form.Big;
            }
            else
            {
                //Add score to mario in exchange for not being able to become big (he already is)
            }

        }

        public override void Update()
        {            
            //Update Hitbox
            Hitbox = new Rectangle((int)Position.X + _horizontalPadding, (int)Position.Y + _verticalPadding, _hitboxWidth, _hitboxHeight);            

            //Add gravity
            VelocityY += gravity;

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

            if (state.IsKeyDown(Keys.D))
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
                if (VelocityX > 0)
                {
                    VelocityX = Math.Max(VelocityX - _deacc, 0);
                }
                else if (VelocityX < 0)
                {
                    VelocityX = Math.Min(VelocityX + _deacc, 0);
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

            CheckCollision();

            //Add speed to position
            Position = new Vector2(Position.X + VelocityX, Position.Y + VelocityY);

            //Focus camera on Mario
            MainGame.camera.LookAt(Position);

            //Prevents pogosticking
            if(VelocityY == 0)
            {
                _jumpWasPressed = false;
            }

            //Set state
            if (VelocityX == 0)
            {
                _state = State.Idle;
            }
            if (VelocityX != 0)
            {
                _state = State.Walking;
            }
            if (VelocityX > 3.4f || VelocityX < -3.4f)
            {
                _state = State.Running;
            }
            if (VelocityY < 0)
            {
                _state = State.Jumping;
            }
            if (VelocityY > 0)
            {
                _state = State.Falling;
            }
            if (VelocityY > 0 && VelocityX < 0.5f && VelocityX > -0.5f)
            {
                _state = State.FallingStraight;
            }
        }

        public override void Jump()
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
    
        public void addCoin()
        {
            Coins++;
        }

        private void ChangeAnimationState(object state)
        {
            if(_animationState < 1)
            {
                _animationState++;
            }
            else
            {
                _animationState = 0;
            }

            if(_state == State.Running)
            {
                _timer.Change(80, 80);
            }
            else
            {
                _timer.Change(190, 190);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRect;

            switch (_state)
            {
                case State.Idle:
                    _spriteImageIndex = 0;
                    _isAnimated = false;
                    break;
                case State.Walking:
                    _spriteImageIndex = 0;
                    _isAnimated = true;
                    break;
                case State.Running:
                    _spriteImageIndex = 2;
                    _isAnimated = true;
                    break;
                case State.Jumping:
                    _spriteImageIndex = 5;
                    _isAnimated = false;
                    break;
                case State.Falling:
                    _spriteImageIndex = 4;
                    _isAnimated = false;
                    break;
                case State.FallingStraight:
                    _spriteImageIndex = 6;
                    _isAnimated = false;
                    break;
                default:
                    _spriteImageIndex = 0;
                    _isAnimated = false;
                    break;
            }
            sourceRect = new Rectangle(16 * (_animationState * (_isAnimated ? 1 : 0) + _spriteImageIndex), 0, 16, Sprite.Height);

            spriteBatch.Draw(texture: Sprite, position: Position, effects: Direction, sourceRectangle: sourceRect);
            spriteBatch.End();
            spriteBatch.Begin();
            spriteBatch.DrawString(_font, String.Format("{0,4}", Coins), new Vector2(768, 0), Color.Black);
            spriteBatch.DrawString(_font, String.Format("{0,4}", Lives), new Vector2(704, 0), Color.Black);
            spriteBatch.End();
            spriteBatch.Begin(transformMatrix: MainGame.camera.GetMatrix(), samplerState: SamplerState.PointClamp);
        }        
    }

}