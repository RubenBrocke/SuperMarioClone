using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class Tangible : Moveable
{
	protected Rectangle hitbox
	{
		get;
		set;
	}

    protected Texture2D sprite
    {
        get;
        set;
    }

    public Tangible() : base()
    {

    }
}

