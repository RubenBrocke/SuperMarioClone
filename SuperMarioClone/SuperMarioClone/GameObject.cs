using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SuperMarioClone
{
    public abstract class GameObject
    {
        public int X { get; protected set; }

        public int Y { get; protected set; }

        public Texture2D sprite { get; protected set; }

        protected Level level { get; set; }

        public GameObject()
        {

        }

        public void Draw(SpriteBatch spriteBatch, int _X, int _Y)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(sprite, new Vector2(_X, _Y));
            spriteBatch.End();
        }
    } 
}

