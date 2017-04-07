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
        public float VelocityX { get; private set; }
        public float VelocityY { get; private set; }
        public float JumpVelocity { get; private set; }
        public float Gravity { get; private set; }

        //Properties
        public int Coins { get; private set; }
        public int Lives { get; private set; }
        public float BigJumpVelocity { get; private set; }
        public float SmallJumpVelocity { get; private set; }
        public State CurrentState { get; private set; }
        public Form CurrentForm { get; private set; }

        //Private fields
        private ContentManager _contentManager;
        private float _accelerate;
        private float _deaccelerate;
        private float _xSpeedMax;
        private float _ySpeedMax;
        private bool _jumpWasPressed;
        private bool _isInvincible;
        private Timer _invincibilityTimer;
        private bool _isForcedToJump;
        private bool _isDead;
        private Texture2D _spriteSheet;
        private SoundEffect _deathSound;
        private SoundEffect _jumpSound;
        private SoundEffect _weirdJumpSound;
        private SoundEffect _oneUpSound;
        private SoundEffect _gameOverSound;
        private SoundEffect _powerUpSound;
        private SpriteFont _font;
        private Animator _animator;
        private int _hitboxWidth;
        private int _hitboxHeight;

        //Enumerations
        public enum State
        {
            Idle,
            Crouching,
            Walking,
            Running,
            Jumping,
            Falling,
            FallingStraight,
            Dead
        }

        public enum Form
        {
            Small,
            Big,
            Flower
        }

        /// <summary>
        /// Constructor for Mario, sets the position of Mario using the GridSize and sets its SpriteSheet and Animation among other default values
        /// </summary>
        /// <param name="x">X position of the Mario</param>
        /// <param name="y">X position of the Mario</param>
        /// <param name="level">Level the Mario should be in</param>
        /// <param name="contentManager">ContentManager used to load Sprites</param>
        public Mario(int x, int y, Level level, ContentManager contentManager) : base()
        {
            /*
             WATCH OUT ITS A BOA CONSTRUCTOR!
                      __    __    __    __
                     /  \  /  \  /  \  /  \
____________________/  __\/  __\/  __\/  __\_____________________________
___________________/  /__/  /__/  /__/  /________________________________
                   | / \   / \   / \   / \  \____
                   |/   \_/   \_/   \_/   \    o \
                                           \_____/--<
            */
            //Properties and private fields are set
            Position = new Vector2(x * Global.Instance.GridSize, y * Global.Instance.GridSize);
            CurrentLevel = level;

            JumpVelocity = 6.25f;
            BigJumpVelocity = 6.70f;
            SmallJumpVelocity = 6.25f;
            Gravity = 0.3f;

            Lives = 3;
            Coins = 0;
            CurrentState = State.Idle;
            CurrentForm = Form.Small;

            _contentManager = contentManager;
            _accelerate = 0.1f;
            _deaccelerate = 0.2f;
            _xSpeedMax = 3.5f;
            _ySpeedMax = 10f;
            _jumpWasPressed = false;
            _isInvincible = false;
            _invincibilityTimer = new Timer(UnInvincify);
            _isDead = false;

            //Sprite, animation, sound, font and hitbox are set
            _spriteSheet = _contentManager.Load<Texture2D>("MarioSheet");
            _animator = new Animator(_spriteSheet);
            _deathSound = _contentManager.Load<SoundEffect>("Death");
            _weirdJumpSound = _contentManager.Load<SoundEffect>("Oink1");
            _jumpSound = _contentManager.Load<SoundEffect>("Jump");
            _oneUpSound = _contentManager.Load<SoundEffect>("1up");
            _powerUpSound = _contentManager.Load<SoundEffect>("PowerUp");
            _gameOverSound = _contentManager.Load<SoundEffect>("GameOver");
            _font = contentManager.Load<SpriteFont>("MarioFont");
            _hitboxWidth = 14;
            _hitboxHeight = 20;
            _horizontalPadding = 1;
            _verticalPadding = 4;
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, _hitboxWidth, _hitboxHeight);

        }

        /// <summary>
        /// Adds a coin of a value to Mario's coin count
        /// </summary>
        /// <param name="value">Value of the coin</param>
        public void AddCoin(int value)
        {
            Coins += value;
            if (Coins >= 100)
            {
                CurrentLevel.ToAddGameObject(new OneUpMushroom((int)Position.X / 16, (int)Position.Y / 16 - 6, CurrentLevel, _contentManager));
                Coins -= 100;
            }
        }

        /// <summary>
        /// Adds a single life to Mario's life count
        /// </summary>
        public void AddLife()
        {
            if (_oneUpSound != null)
            {
                _oneUpSound.Play();
            }
            Lives++;
        }


        /// <summary>
        /// Updates Mario
        /// </summary>
        public override void Update()
        {
            //Update hitbox to match current position and State
            UpdateHitBox();

            //Update position
            UpdatePosition();

            //Add gravity to vertical velocity
            AddGravity();

            if (!_isDead)
            {
                //Check collision and change velocity
                CollisionCheck();

                //Kills Mario when he falls out of the map
                KillWhenOutOfMap();

                //Limit vertical speed
                LimitSpeed();

                //Prevent pogosticking
                PreventPogoSticking();

                //Adds Movement to Mario
                Move();
            }
            else
            {
                VelocityX = 0;
            }

            //Sets the state of Mario
            SetState();

            //Update sprite
            UpdateSprite();
        }

        /// <summary>
        /// Updates Mario's Hitbox to match his current State and Position
        /// </summary>
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

        /// <summary>
        /// Updates Mario's Position using his velocity
        /// </summary>
        private void UpdatePosition()
        {
            Position = new Vector2(Position.X + VelocityX, Position.Y + VelocityY);
        }

        /// <summary>
        /// Adds Gravity to Mario's vertical velocity
        /// </summary>
        private void AddGravity()
        {
            VelocityY += Gravity;
        }

        /// <summary>
        /// Checks if Mario is colliding with anything and changes his velocity accordingly
        /// </summary>
        private void CollisionCheck()
        {
            float vX;
            float vY;
            CheckCollision(this, out vX, out vY);
            VelocityX = vX;
            if (!_isForcedToJump)
            {
                VelocityY = vY;
            }
            _isForcedToJump = false;
        }
        
        /// <summary>
        /// Kills Mario when he falls outside of the map
        /// </summary>
        private void KillWhenOutOfMap()
        {
            if (Position.Y > 666 && !_isDead)
            {
                Die();
            }
        }

        /// <summary>
        /// Limits Mario's speed so he doesn't run faster than he's able to usually and so he doesn't exceed terminal velocity
        /// </summary>
        private void LimitSpeed()
        {
            //Limits vertical speed
            if (VelocityY > _ySpeedMax)
            {
                VelocityY = _ySpeedMax;
            }

            //Limits horizontal speed
            if (VelocityX > _xSpeedMax)
            {
                VelocityX = _xSpeedMax;
            }
            else if (VelocityX < -_xSpeedMax)
            {
                VelocityX = -_xSpeedMax;
            }
        }

        /// <summary>
        /// Prevents Mario from Jumping constantly when you hold the jump button down
        /// </summary>
        private void PreventPogoSticking()
        {
            KeyboardState state = Keyboard.GetState();
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
        }

        /// <summary>
        /// Listens to keyboard input and changes Mario's velocity accordingly
        /// </summary>
        private void Move()
        {
            KeyboardState state = Keyboard.GetState();

            //Coin cheat
            if (state.IsKeyDown(Keys.G) && state.IsKeyDown(Keys.I) && state.IsKeyDown(Keys.B))
            {
                AddCoin(10);
            }

            //When S is pressed Mario crouches
            if (state.IsKeyDown(Keys.S))
            {
                if (IsColliding(CurrentLevel, 0, 1, out collObject))
                {
                    if (!(collObject is TransFloor || collObject is CloudBlock) || Hitbox.Y + Hitbox.Height == collObject.Hitbox.Top)
                    {
                        if (VelocityX > 0)
                        {
                            VelocityX = Math.Max(VelocityX - _deaccelerate / 2, 0);
                        }
                        else if (VelocityX < 0)
                        {
                            VelocityX = Math.Min(VelocityX + _deaccelerate / 2, 0);
                        }
                    }
                }
            }
            //When D is pressed Mario moves to the right
            else if (state.IsKeyDown(Keys.D))
            {
                if (VelocityX < 0)
                {
                    VelocityX += _accelerate * 2;
                }
                else
                {
                    VelocityX += _accelerate;
                }
                Direction = SpriteEffects.None;
            }
            //When D is pressed Mario moves to the left
            else if (state.IsKeyDown(Keys.A))
            {
                if (VelocityX > 0)
                {
                    VelocityX -= _accelerate * 2;
                }
                else
                {
                    VelocityX -= _accelerate;
                }
                Direction = SpriteEffects.FlipHorizontally;
            }
            //When nothing is pressed but Mario is still colliding the floor, Mario deaccelerates 
            else if (IsColliding(CurrentLevel, 0, 1, out collObject))
            {
                if (!(collObject is TransFloor || collObject is CloudBlock) || Hitbox.Y + Hitbox.Height == collObject.Hitbox.Top)
                {
                    if (VelocityX > 0)
                    {
                        VelocityX = Math.Max(VelocityX - _deaccelerate, 0);
                    }
                    else if (VelocityX < 0)
                    {
                        VelocityX = Math.Min(VelocityX + _deaccelerate, 0);
                    }
                }
            }
        }

        /// <summary>
        /// Looks at Mario's velocity and keyboard input to see which State Mario should have
        /// </summary>
        private void SetState()
        {
            KeyboardState state = Keyboard.GetState();

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
            if (_isDead)
            {
                CurrentState = State.Dead;
            }
        }

        /// <summary>
        /// Updates Mario's Sprite to the proper Animation or Sprite according to his State
        /// </summary>
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
                case State.Dead:
                    _animator.SetAnimationSpeed(190);
                    _animator.GetTextures(9 * spriteWidth, offSetY, spriteWidth, spriteHeight, 2, 1);
                    break;
                default:
                    _animator.SetAnimationSpeed(0);
                    _animator.GetTextures(6 * spriteWidth, offSetY, spriteWidth, spriteHeight, 1, 1);
                    break;
            }
            Sprite = _animator.GetCurrentTexture();
        }

        /// <summary>
        /// Makes Mario become big, if he already is, nothing happens
        /// </summary>
        public void BecomeBig()
        {
            if (CurrentForm == Form.Small)
            {
                if (_powerUpSound != null)
                {
                    _powerUpSound.Play();
                }
                CurrentForm = Form.Big;
                JumpVelocity = BigJumpVelocity;
                UpdateHitBox();
            }
        }

        /// <summary>
        /// Forces Mario to Jump
        /// </summary>
        public void Jump()
        {
            VelocityY = -JumpVelocity;
            _isForcedToJump = true;
            if (Global.Instance.WeirdSounds)
            {
                if (_weirdJumpSound != null && !_isDead)
                {
                    _weirdJumpSound.Play();
                }
            }
            else
            {
                if (_jumpSound != null && !_isDead)
                {
                    _jumpSound.Play();
                }
            }
        }

        /// <summary>
        /// Makes Mario small when he's big and kills him if he's small
        /// </summary>
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
                    case Form.Flower:
                        _isInvincible = true;
                        _invincibilityTimer.Change(1000, Timeout.Infinite);
                        JumpVelocity = SmallJumpVelocity;
                        CurrentForm--;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Removes Mario's invcibility
        /// </summary>
        /// <param name="state"></param>
        private void UnInvincify(object state)
        {
            _isInvincible = false;
            _invincibilityTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Activates the GameOver sequence which shows the black screen with red letters saying 'Game Over'
        /// </summary>
        /// <param name="state"></param>
        private void GameOver(object state)
        {
            if (_gameOverSound != null)
            {
                _gameOverSound.Play();
            }
            Global.Instance.MainGame.gameOver = true;
            Global.Instance.MainGame.currentLevel = null;
        }

        /// <summary>
        /// Makes Mario Die
        /// </summary>
        public void Die()
        {
            if (!_isDead)
            {
                _isDead = true;
                Lives--;
                Jump();
                CurrentForm = Form.Small;
                MediaPlayer.Pause();
                VelocityX = 0;
                if (_deathSound != null)
                {
                    _deathSound.Play();
                }
                if (Lives < 1)
                {
                    Timer gameOverTimer = new Timer(GameOver);
                    gameOverTimer.Change(4000, Timeout.Infinite);
                }
                else
                {
                    Timer menuLoadTimer = new Timer(LoadMenu);
                    menuLoadTimer.Change(4000, Timeout.Infinite);
                }
            }
            VelocityX = 0;
        }

        /// <summary>
        /// Loads the menu after Mario dies
        /// </summary>
        /// <param name="state"></param>
        private void LoadMenu(object state)
        {
            MediaPlayer.Resume();
            LevelReader lr = new LevelReader(_contentManager);
            Global.Instance.MainGame.ChangeCurrentLevel(lr.ReadLevel(0));
        }

        /// <summary>
        /// Changes Mario's CurrentLevel and resets his position and velocity
        /// </summary>
        /// <param name="level"></param>
        public void ChangeCurrentLevel(Level level)
        {
            _isDead = false;
            CurrentLevel = level;
            CurrentLevel.ToAddGameObject(this);
            Position = new Vector2(0, 544);
            VelocityX = 0;
            VelocityY = 0;
        }
    }
}