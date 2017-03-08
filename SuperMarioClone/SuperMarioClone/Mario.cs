using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using SuperMarioClone;

public class Mario : Tangible
{
	private int _coins { get; set; }

    private int _lives { get; set; }

    private int x_speed_max = 5;
    private int y_speed_max = 10;

    private SpriteFont _font;

    //----Hitbox----//
    public Rectangle outRect;

    public Mario(int _x, int _y, Level lvl, ContentManager cm) : base()
    {
        X = _x;
        Y = _y;
        sprite = cm.Load<Texture2D>("Mario_Stationary");
        _font = cm.Load<SpriteFont>("Font");
        currentLevel = lvl;
        jumpVelocity = 4f;

        hitbox = new Rectangle(X, Y, sprite.Width, sprite.Height);
        _lives = 3;
        _coins = 0;
    }

    public override void Update()
    {
        //Debug
        Console.WriteLine(velocityY);

        //Update Hitbox
        hitbox = new Rectangle(X, Y, sprite.Width, sprite.Height);

        //Add gravity
        if (!IsColliding(currentLevel, 0, 1, out outRect))
        {
            velocityY += gravity;
        }

        //Limit vertical speed
        if (velocityY > y_speed_max)
        {
            velocityY = y_speed_max;
        }

        //Add movement
        KeyboardState state = Keyboard.GetState();

        if (state.IsKeyDown(Keys.D))
        {
            velocityX = 4;
            direction = SpriteEffects.None;
        }
        else if (state.IsKeyDown(Keys.A))
        {
            velocityX = -4;
            direction = SpriteEffects.FlipHorizontally;
        }
        else if (IsColliding(currentLevel, 0, 1, out outRect))
        {
            velocityX = 0;
        }
        if (state.IsKeyDown(Keys.Space))
        {
            Jump();
        }

        //Do collision (to be moved to parent class)
        if (IsColliding(currentLevel, 0, (int)velocityY, out outRect))
        {
            Y = outRect.Top - hitbox.Height;
            velocityY = 0;
        }

        if (IsColliding(currentLevel, (int)velocityX, 0, out outRect))
        {
            if (X > outRect.Right)
            {
                X = outRect.Right;
            }
            else if (X < outRect.Left)
            {
                X = outRect.Left - hitbox.Width;
            }
            velocityX = 0;
        }

        //Add speed to position
        Y += (int)velocityY;
        X += (int)velocityX;

        //Focus camera on Mario
        MainGame.camera.LookAt(X, Y);
        
        //Debug
        Console.WriteLine(velocityY);
    }

    public void Jump()
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

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        spriteBatch.End();
        spriteBatch.Begin();
        spriteBatch.DrawString(_font, String.Format("{0,4}", _coins), new Vector2(32, 512), Color.Black);
        spriteBatch.End();
        spriteBatch.Begin(transformMatrix: MainGame.camera.GetMatrix());
    }

}

