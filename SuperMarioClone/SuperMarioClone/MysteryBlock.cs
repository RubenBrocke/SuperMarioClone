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
        public MysteryBlock(int _x, int _y, Level lvl, ContentManager cm) : base()
        {
            X = _x;
            Y = _y;
            currentLevel = lvl;
            sprite = cm.Load<Texture2D>("MysteryBlock");
            hitbox = new Rectangle(X, Y, 12, 16);
        }
    }
}
