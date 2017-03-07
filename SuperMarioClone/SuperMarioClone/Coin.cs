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

        public Coin() : base()
        {

        }

        public virtual void AddCoin(Mario mario)
        {
            mario.addCoin();
        }
    } 
}

