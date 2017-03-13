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

        public Goomba(int _x, int _y, Level lvl, ContentManager cm)
        {
            Position = new Vector2(_x, _y);
            CurrentLevel = lvl;
            velocityX = 2f;
            _walkDirection = "right";
            Sprite = cm.Load<Texture2D>("Goomba");
            hitbox = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height);
            _horizontalPadding = 1;
            _verticalPadding = 0;
        }

        public override void Update()
        {
            hitbox = new Rectangle((int)Position.X, (int)Position.Y, hitbox.Width, hitbox.Height);

            velocityY += gravity;

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
                velocityX = -2;
            }
            if (_walkDirection.Equals("right"))
            {
                velocityX = 2;
            }           

            Position = new Vector2(Position.X + velocityX, Position.Y + velocityY);
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
