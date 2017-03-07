using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Level
{
	protected List<Tangible> _tangibles{ get;	set; }

    protected Texture2D _background{	get; set; }

	public virtual void IsColliding()
	{
		throw new System.NotImplementedException();
	}

	public virtual void AddTangible(Tangible T)
	{
        _tangibles.Add(T);
	}

	public virtual void SetBackground(Texture2D background)
	{
        _background = background;
	}

}

