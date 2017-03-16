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
        public state State { get; private set; }
        public form Form { get; private set; }

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

        private int _animationState = 0;
        private int _spriteImageIndex = 0;
        private bool _isAnimated = false;

        private int _hitboxWidth = 14;
        private int _hitboxHeight = 20;

        private Timer _timer;

        public enum state
        {
            Idle,
            Walking,
            Running,
            Jumping,
            Falling,
            FallingStraight
        }

        public enum form
        {
            Small,
            Big,
            Flower,
            Tanuki
        }

        public Mario(int x, int y, Level level, ContentManager contentManager) : base()
        {
            Position = new Vector2(x, y);
            Sprite = contentManager.Load<Texture2D>("MarioSheetRight");
            _jumpSound = contentManager.Load<SoundEffect>("Oink1");
            _font = contentManager.Load<SpriteFont>("Font");
            CurrentLevel = level;
            JumpVelocity = 6.25f;
            State = state.Idle;
            Form = form.Small;
            _timer = new Timer(ChangeAnimationState);
            _timer.Change(0, 100);
            Gravity = 0.3f;
            _horizontalPadding = 1;
            _verticalPadding = 2;
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, _hitboxWidth, _hitboxHeight);
            Lives = 3;
            Coins = 0;
        }

        public void BecomeBig()
        {
            if (Form == form.Small)
            {
                Form = form.Big;
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
                State = Mario.state.Idle;
            }
            if (VelocityX != 0)
            {
                State = Mario.state.Walking;
            }
            if (VelocityX > 3.4f || VelocityX < -3.4f)
            {
                State = Mario.state.Running;
            }
            if (VelocityY < 0)
            {
                State = Mario.state.Jumping;
            }
            if (VelocityY > 0)
            {
                State = Mario.state.Falling;
            }
            if (VelocityY > 0 && VelocityX < 0.5f && VelocityX > -0.5f)
            {
                State = Mario.state.FallingStraight;
            }
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

        private void ChangeAnimationState(object state)
        {
            if (_animationState < 1)
            {
                _animationState++;
            }
            else
            {
                _animationState = 0;
            }

            if (State == Mario.state.Running)
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

            switch (State)
            {
                case state.Idle:
                    _spriteImageIndex = 0;
                    _isAnimated = false;
                    break;
                case state.Walking:
                    _spriteImageIndex = 0;
                    _isAnimated = true;
                    break;
                case state.Running:
                    _spriteImageIndex = 2;
                    _isAnimated = true;
                    break;
                case state.Jumping:
                    _spriteImageIndex = 5;
                    _isAnimated = false;
                    break;
                case state.Falling:
                    _spriteImageIndex = 4;
                    _isAnimated = false;
                    break;
                case state.FallingStraight:
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