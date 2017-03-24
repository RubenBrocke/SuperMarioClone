using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.MockClasses
{
    class TestTexture2D : Texture2D
    {
        public TestTexture2D(GraphicsDevice gd, int widht, int height) : base (gd, widht, height)
        {

        }

    }
}
