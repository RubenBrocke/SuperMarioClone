using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SuperMarioClone
{
    public static class ContentLoader
    {
        private static ContentManager contentManager;

        public static Texture2D loadTexture(string fileName)
        {
            return contentManager.Load<Texture2D>(fileName);
        }

        public static void setContentManager(ContentManager Cm)
        {
            contentManager = Cm;
        }
    }
}
