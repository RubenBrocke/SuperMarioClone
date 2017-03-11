using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarioClone
{
    class MysteryBlock : Solid
    {
        private Type _mysteryObject { get; set; }
        private ContentManager _cm;

        public MysteryBlock(int _x, int _y, Level lvl, ContentManager cm, Type MysteryObject) : base()
        {
            //TODO: add animation later
            _mysteryObject = MysteryObject;
            X = _x;
            Y = _y;
            currentLevel = lvl;
            _cm = cm;
            sprite = _cm.Load<Texture2D>("MysteryBlock");
            hitbox = new Rectangle(X, Y, 16, 16); // TODO: numbers represent pixels, change magic number
        }


        //TODO: Will eject whatever the mystery block contains
        public void Eject(Mario mario, float vY)
        {
            if (vY < 0)
            {
                currentLevel.ToAddGameObject((GameObject)Activator.CreateInstance(_mysteryObject, X + 16, Y - hitbox.Height, currentLevel, _cm));
            } 
        }
    }
}
