using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class Moveable : GameObject
{
	protected int velocityX
	{
		get;
		set;
	}

    protected int velocityY
	{
		get;
		set;
	}

    protected int jumpVelocity
	{
		get;
		set;
	}

    public Moveable() : base()
    {

    }

	public virtual void MoveLeft()
	{
        X += velocityX;
	}

	public virtual void MoveRight()
	{
        X += velocityX;
	}

	public virtual void Jump()
	{
        Y -= jumpVelocity;
	}

}

