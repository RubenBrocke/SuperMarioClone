using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarioClone.Content
{
    class MysteryBlock : Solid
    {
        private GameObject _mysteryObject { get; set; }
        public MysteryBlock(int _x, int _y, Level lvl, ContentManager cm, GameObject MysteryObject) : base()
        {
            //TODO: add animation later
            _mysteryObject = MysteryObject;
            X = _x;
            Y = _y;
            currentLevel = lvl;
            sprite = cm.Load<Texture2D>("MysteryBlock");
            hitbox = new Rectangle(X, Y, 16, 16); // TODO: numbers represent pixels, change magic number
        }


        //TODO: Will eject whatever the mystery block contains
        public void Eject()
        {

        }
    }
}
