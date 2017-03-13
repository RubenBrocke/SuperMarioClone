using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioClone
{
    class TransFloor : Floor
    {
        public TransFloor(float _x, float _y, int _w, int _h, Level lvl, ContentManager cm) : base(_x, _y, _w, _h, lvl, cm)
        {

        }
    }
}
