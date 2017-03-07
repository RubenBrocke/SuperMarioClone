using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class GameObject
{
    protected int X { get; set; }

    protected int Y { get; set; }

    protected Texture2D sprite { get; set; }

    protected Level level { get; set; }

    public GameObject() : base()
    {
        
    }

    public Vector2 GetPosition()
    {
        return new Vector2(X, Y);
    }

    public Texture2D GetSprite()
    {
        return sprite;
    }
}

