using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class Moveable : GameObject
{
	private int _velocityX
	{
		get;
		set;
	}

	private int _velocityY
	{
		get;
		set;
	}

	private int _jumpVelocity
	{
		get;
		set;
	}

	public virtual void MoveLeft()
	{
		throw new System.NotImplementedException();
	}

	public virtual void MoveRight()
	{
		throw new System.NotImplementedException();
	}

	public virtual void Jump()
	{
		throw new System.NotImplementedException();
	}

}

