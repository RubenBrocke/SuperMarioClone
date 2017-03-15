using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace SuperMarioClone
{
    class Goomba : Tangible
    {

        private string _walkDirection;

        public Goomba(int x, int y, Level lvl, ContentManager cm)
        {
            Position = new Vector2(x, y);
            CurrentLevel = lvl;
            VelocityX = 2f;
            _walkDirection = "right";
            Sprite = cm.Load<Texture2D>("Goomba");
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height);
            _horizontalPadding = 1;
            _verticalPadding = 0;
        }

        public override void Update()
        {
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Hitbox.Width, Hitbox.Height);

            VelocityY += gravity;

            if (CheckCollision())
            {
                if (_walkDirection.Equals("right"))
                {
                    Direction = SpriteEffects.None;
                    _walkDirection = "left";
                }
                else
                {
                    Direction = SpriteEffects.FlipHorizontally;
                    _walkDirection = "right";
                }
            }

            if (_walkDirection.Equals("left"))
            {
                VelocityX = -2;
            }
            if (_walkDirection.Equals("right"))
            {
                VelocityX = 2;
            }           

            Position = new Vector2(Position.X + VelocityX, Position.Y + VelocityY);
        }  
        
        public void CheckDeath(Mario mario, float vY)
        {
            if (vY > 0)
            {
                CurrentLevel.ToRemoveGameObject(this);
            }else
            {
                mario.LoseLife();
            }
        }
    }
}
