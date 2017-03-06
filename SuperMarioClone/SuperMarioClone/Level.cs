using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Level
{
	private List<Tangible> _tangibles
	{
		get;
		set;
	}

	private object _background
	{
		get;
		set;
	}


	public virtual void IsColliding()
	{
		throw new System.NotImplementedException();
	}

	public virtual void AddTangible()
	{
		throw new System.NotImplementedException();
	}

	public virtual void SetBackground()
	{
		throw new System.NotImplementedException();
	}

}

