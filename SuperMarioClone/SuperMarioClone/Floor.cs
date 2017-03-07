using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Floor : Solid
{
    public Floor(int _x, int _y, int _width, int _height) : base()
    {
        X = _x;
        Y = _y;
        Width = _width;
        Height = _height;
    }

    private int Width { get; set; }
    private int Height { get; set; }
}

