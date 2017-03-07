using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace SuperMarioClone
{
    class Background
    {
        public Texture2D backgroundTexture { get; set; }

        public Background(Texture2D texture)
        {
            backgroundTexture = texture;
        }
    }
}
