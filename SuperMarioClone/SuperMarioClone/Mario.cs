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
	    private int _coins { get; set; }

        private int _lives { get; set; }

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
            position = new Vector2(_x, _y);
            sprite = cm.Load<Texture2D>("MarioSheetRight");
            _jumpSound = cm.Load<SoundEffect>("Oink1");
            _font = cm.Load<SpriteFont>("Font");
            currentLevel = lvl;
            jumpVelocity = 6.25f;
            _state = State.Idle;
            _form = Form.Small;
            _timer = new Timer(ChangeAnimationState);
            _timer.Change(0, 100);
        
            hitbox = new Rectangle((int)position.X, (int)position.Y, _hitboxWidth, _hitboxHeight);
            _lives = 3;
            _coins = 0;
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
            hitbox = new Rectangle((int)position.X + _horizontalPadding, (int)position.Y + _verticalPadding, _hitboxWidth, _hitboxHeight);            

            //Add gravity
            velocityY += gravity;

            //Limit vertical speed
            if (velocityY > _ySpeedMax)
            {
                velocityY = _ySpeedMax;
            }

            //Limit horizontal speed
            if (velocityX > _xSpeedMax)
            {
                velocityX = _xSpeedMax;
            }
            else if (velocityX < -_xSpeedMax)
            {
                velocityX = -_xSpeedMax;
            }

            CheckCollision();

            //Add movement
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.D))
            {
                if (velocityX < 0)
                {
                    velocityX += _acc * 2;
                }
                else
                {
                    velocityX += _acc;
                }
                direction = SpriteEffects.None;
            }
            else if (state.IsKeyDown(Keys.A))
            {
                if (velocityX > 0)
                {
                    velocityX -= _acc * 2;
                }
                else
                {
                    velocityX -= _acc;
                }
                direction = SpriteEffects.FlipHorizontally;
            }
            else if (IsColliding(currentLevel, 0, 1, out outRect))
            {
                if (velocityX > 0)
                {
                    velocityX = Math.Max(velocityX - _deacc, 0);
                }
                else if (velocityX < 0)
                {
                    velocityX = Math.Min(velocityX + _deacc, 0);
                }
            }
            if (state.IsKeyDown(Keys.Space))
            {
                Jump();
            }

            

            //Add speed to position
            position = new Vector2(position.X + velocityX, position.Y + velocityY);

            //Focus camera on Mario
            MainGame.camera.LookAt(position);

            //Set state
            if (velocityX == 0)
            {
                _state = State.Idle;
            }
            if (velocityX != 0)
            {
                _state = State.Walking;
            }
            if (velocityX > 3.4f || velocityX < -3.4f)
            {
                _state = State.Running;
            }
            if (velocityY < 0)
            {
                _state = State.Jumping;
            }
            if (velocityY > 0)
            {
                _state = State.Falling;
            }
            if (velocityY > 0 && velocityX < 0.5f && velocityX > -0.5f)
            {
                _state = State.FallingStraight;
            }
        }

        public override void Jump()
        {
            if (IsColliding(currentLevel, 0, 1, out outRect))
            {
                 velocityY = -jumpVelocity;
                _jumpSound.Play();
            }
        }

        public void LoseLife()
	    {
            _lives--;    
	    }
    
        public void addCoin()
        {
            _coins++;
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
            sourceRect = new Rectangle(16 * (_animationState * (_isAnimated ? 1 : 0) + _spriteImageIndex), 0, 16, sprite.Height);

            spriteBatch.Draw(texture: sprite, position: position, effects: direction, sourceRectangle: sourceRect);
            spriteBatch.End();
            spriteBatch.Begin();
            spriteBatch.DrawString(_font, String.Format("{0,4}", _coins), new Vector2(768, 0), Color.Black);
            spriteBatch.End();
            spriteBatch.Begin(transformMatrix: MainGame.camera.GetMatrix(), samplerState: SamplerState.PointClamp);
        }        
    }

}