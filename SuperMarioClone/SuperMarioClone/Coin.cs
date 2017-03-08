using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SuperMarioClone
{
    public class Coin : Tangible
    {
        private int _value { get; set; }

        private bool _moveable { get; set; }

        private bool _hasBeenPickedUp { get; set; }

        public Coin(int _x, int _y, Level lvl, ContentManager cm) : base()
        {
            X = _x;
            Y = _y;
            currentLevel = lvl;
            _hasBeenPickedUp = false;
            sprite = cm.Load<Texture2D>("MysteryBlock");
            hitbox = new Rectangle(X, Y, sprite.Width, sprite.Height);
        }

        public void AddCoin(Mario mario)
        {
            if (!_hasBeenPickedUp)
            {
                mario.addCoin();
                currentLevel.ToRemoveGameObject(this);
                _hasBeenPickedUp = true;
            }
        }
    } 
}

