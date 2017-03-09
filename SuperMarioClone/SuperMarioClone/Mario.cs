using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using SuperMarioClone;
using System.Threading;

public class Mario : Tangible
{
	private int _coins { get; set; }

    private int _lives { get; set; }

    private float _xSpeedMax = 3.5f;
    private float _ySpeedMax = 10;
    private float xAcc = 0.1f;

    private SpriteFont _font;

    private int _spriteImageIndex = 0;

    private int _hitboxWidth = 14;
    private int _hitboxHeight = 20;
    private int _horizontalPadding = 1;
    private int _verticalPadding = 2;

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

    private State _state;

    //----Hitbox----//
    public Rectangle outRect;

    public Mario(int _x, int _y, Level lvl, ContentManager cm) : base()
    {
        X = _x;
        Y = _y;
        sprite = cm.Load<Texture2D>("MarioSheetRight");
        _font = cm.Load<SpriteFont>("Font");
        currentLevel = lvl;
        jumpVelocity = 7f;
        _state = State.Idle;
        _timer = new Timer(ChangeSpriteIndex);
        _timer.Change(0, 100);
        
        hitbox = new Rectangle(X, Y, _hitboxWidth, _hitboxHeight);
        _lives = 3;
        _coins = 0;
    }

    public override void Update()
    {
        //Update Hitbox
        hitbox = new Rectangle(X + _horizontalPadding, Y + _verticalPadding, _hitboxWidth, _hitboxHeight);

        //Add gravity
        if (!IsColliding(currentLevel, 0, 1, out outRect))
        {
            //velocityY += gravity;
        }
        
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

        //Add movement
        KeyboardState state = Keyboard.GetState();

        if (state.IsKeyDown(Keys.D))
        {
            velocityX += xAcc;
            direction = SpriteEffects.None;
        }
        else if (state.IsKeyDown(Keys.A))
        {
            velocityX -= xAcc;
            direction = SpriteEffects.FlipHorizontally;
        }
        else if (IsColliding(currentLevel, 0, 1, out outRect))
        {
            if (velocityX > 0)
            {
                velocityX -= xAcc;
            }
            if (velocityX < 0)
            {
                velocityX += xAcc;
            }
        }
        if (state.IsKeyDown(Keys.Space))
        {
            Jump();
        }

        //Do collision (to be moved to parent class)
        if (IsColliding(currentLevel, (int)velocityX, 0, out outRect))
        {
            if (velocityX < 0)//hitbox.X > outRect.Right
            {
                X = outRect.Right - _horizontalPadding;
            }
            else if (velocityX > 0)//hitbox.X < outRect.Left
            {
                X = outRect.Left - hitbox.Width - _horizontalPadding;
            }
            velocityX = 0;
        }

        if (IsColliding(currentLevel, 0, (int)Math.Ceiling(velocityY), out outRect))
        {
            if (velocityY > 0)
            {
                Y = outRect.Top - hitbox.Height - _verticalPadding;
            }
            else if (velocityY < 0)
            {
                Y = outRect.Bottom - _verticalPadding;
            }
            velocityY = 0;
        }

        //Add speed to position
        Y += (int)velocityY;
        X += (int)velocityX;

        //Focus camera on Mario
        MainGame.camera.LookAt(X, Y);

        //Debug
        Console.WriteLine(velocityX);

        //Set state
        if (velocityX == 0)
        {
            _state = State.Idle;
        }
        if (velocityX != 0)
        {
            _state = State.Walking;
        }
        if (velocityX > 3.4 || velocityX < -3.4)
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

    private void ChangeSpriteIndex(object state)
    {
        if(_spriteImageIndex == 0)
        {
            _spriteImageIndex++;
        }
        else
        {
            _spriteImageIndex = 0;
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Rectangle sourceRect;
        switch (_state)
        {
            case State.Idle:
                sourceRect = new Rectangle(0, 0, 16, 22);
                break;
            case State.Walking:
                sourceRect = new Rectangle(16 * _spriteImageIndex, 0, 16, 22);
                break;
            case State.Running:
                sourceRect = new Rectangle(16 * _spriteImageIndex + 32, 0, 16, 22);
                break;
            case State.Jumping:
                sourceRect = new Rectangle(80, 0, 16, 22);
                break;
            case State.Falling:
                sourceRect = new Rectangle(64, 0, 16, 22);
                break;
            case State.FallingStraight:
                sourceRect = new Rectangle(96, 0, 16, 22);
                break;
            default:
                sourceRect = new Rectangle(0, 0, 16, 22);
                break;
        }
        spriteBatch.Draw(texture: sprite, position: new Vector2(X, Y), effects: direction, sourceRectangle: sourceRect);
        spriteBatch.End();
        spriteBatch.Begin();
        spriteBatch.DrawString(_font, String.Format("{0,4}", _coins), new Vector2(32, 512), Color.Black);
        spriteBatch.End();
        spriteBatch.Begin(transformMatrix: MainGame.camera.GetMatrix());
    }

}

