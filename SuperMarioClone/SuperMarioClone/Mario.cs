using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Mario : Tangible
{
	private int _coins { get; set; }

    private int _lives { get; set; }

    public Mario() : base()
    {

    }

	public virtual void LoseLife()
	{
        _lives--;    
	}
    
    public void addCoin()
    {
        _coins++;
    }

}

