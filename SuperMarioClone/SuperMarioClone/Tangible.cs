﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class Tangible : Moveable
{
    protected Rectangle hitbox;

    public Tangible() : base()
    {

    }

    public bool IsColliding(Level lvl, int offsetX, int offsetY, out Rectangle Rect)
    {
        bool succes = true;
        Rect = Rectangle.Empty;
        foreach (Tangible Object in lvl.Tangibles)
        {
            Rectangle testRect = new Rectangle(Object.X - offsetX, Object.Y - offsetY, Object.hitbox.Width, Object.hitbox.Height);

            if (testRect.Intersects(hitbox))
            {
                succes = true;
                Rect = Object.hitbox;
                break;
            }
            else
            {
                succes = false;
            }
        }

        return succes;
    }
}

