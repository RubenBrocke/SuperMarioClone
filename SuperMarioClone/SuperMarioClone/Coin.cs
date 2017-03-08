using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioClone
{
    public class Coin : Tangible
    {
        private int _value { get; set; }

        private bool _moveable { get; set; }

        public Coin(int _x, int _y, Level lvl) : base()
        {
            X = _x;
            Y = _y;
            currentLevel = lvl;
            sprite = ContentLoader.loadTexture("Coin");
            hitbox = new Rectangle(X, Y, sprite.Width, sprite.Height);
        }

        public void AddCoin(Mario mario)
        {
            mario.addCoin();
            currentLevel.ToRemoveGameObject(this);
        }
    } 
}

