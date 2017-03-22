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
        public Vector2 Position { get; protected set; }
        protected Texture2D Sprite { get; set; }
        protected SpriteEffects Direction { get; set; }
        protected Level CurrentLevel { get; set; }

        public GameObject()
        {
            Direction = SpriteEffects.None;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture: Sprite, position: Position, effects: Direction);
        }

        public virtual void Update()
        {

        }
        
    } 
}

