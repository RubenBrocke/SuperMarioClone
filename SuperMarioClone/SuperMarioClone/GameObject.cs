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
        protected int X { get; set; }

        protected int Y { get;  set; }

        protected Texture2D sprite { get; set; }

        protected SpriteEffects direction { get; set; }

        protected Level currentLevel { get; set; }

        public GameObject()
        {
            direction = SpriteEffects.None;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture: sprite, position: new Vector2(X, Y), effects: direction);
        }

        public virtual void Update()
        {

        }
        
    } 
}

