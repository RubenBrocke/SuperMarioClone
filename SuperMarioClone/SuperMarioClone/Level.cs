using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Level
{
	private List<GameObject> _gameObjects { get; set; }

    private Texture2D _background { get; set; }

    public virtual void IsColliding()
	{
		throw new System.NotImplementedException();
	}

	public virtual void AddGameObject(GameObject g)
	{
        _gameObjects.Add(g);

	}

	public virtual void SetBackground(Texture2D background)
	{
        _background = background;
	}

}

