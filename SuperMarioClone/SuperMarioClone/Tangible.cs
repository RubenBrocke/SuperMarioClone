using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class Tangible : Moveable
{
	private Rectangle _hitbox
	{
		get;
		set;
	}

    private Texture2D _sprite
    {
        get;
        set;
    }

    public Tangible() : base()
    {

    }
}

