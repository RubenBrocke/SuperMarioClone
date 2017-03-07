using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Level
{
	public List<GameObject> Tangibles { get; set; }

    private Texture2D _background { get; set; }

	public virtual void AddGameObject(GameObject g)
	{
        Tangibles.Add(g);

	}

	public virtual void SetBackground(Texture2D background)
	{
        _background = background;
	}

}

