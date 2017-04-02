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
        public Texture2D Sprite { get; protected set; }
        public SpriteEffects Direction { get; protected set; }
        public Level CurrentLevel { get; protected set; }

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

