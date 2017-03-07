using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using 

public class Mario : Tangible
{
	private int _coins { get; set; }

    private int _lives { get; set; }

    public int x_speed_max = 5;
    public int y_speed_max = 10;

    //----Hitbox----//
    public Rectangle outRect;

    //---Current Level---//
    static Level currentLevel;

    public Mario(int _x, int _y) : base()
    {
        X = _x;
        Y = _y;
        

        hitbox = new Rectangle(X, Y, sprite.Width, sprite.Height);
        _lives = 3;
        _coins = 0;
    }   

    public void Update()
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
        }
        else if (state.IsKeyDown(Keys.A))
        {
            velocityX = -4;
        }
        else if (IsColliding(currentLevel, 0, 1, out outRect))
        {
            velocityX = 0;
        }
        if (state.IsKeyDown(Keys.Space))
        {
            jump();
        }

        //Do collision (to be moved to parent class)
        if (IsColliding(currentLevel, 0, (int)velocityY, out outRect))
        {
            Y = outRect.Top - hitbox.Height / 2;
            velocityY = 0;
        }

        if (IsColliding(currentLevel, (int)velocityX, 0, out outRect))
        {
            if (X > outRect.Right)
            {
                X = outRect.Right + hitbox.Width / 2;
            }
            else if (X < outRect.Left)
            {
                X = outRect.Left - hitbox.Width / 2;
            }
            velocityX = 0;
        }

        //Add speed to position
        Y += (int)velocityY;
        X += (int)velocityX;
    }

    public void jump()
    {
        if (IsColliding(currentLevel, 0, 1, out outRect))
        {
            velocityY = jumpVelocity;
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

}

