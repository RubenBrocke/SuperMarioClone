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
        private Rectangle outRect;

        public Goomba(int _x, int _y, Level lvl, ContentManager cm)
        {
            position = new Vector2(_x, _y);
            currentLevel = lvl;
            velocityX = 2f;
            _walkDirection = "right";
            sprite = cm.Load<Texture2D>("Goomba");
            hitbox = new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height);
            _horizontalPadding = 1;
            _verticalPadding = 0;
        }

        public override void Update()
        {
            hitbox = new Rectangle((int)position.X, (int)position.Y, hitbox.Width, hitbox.Height);

            velocityY += gravity;

            if (CheckCollision())
            {
                if (_walkDirection.Equals("right"))
                {
                    direction = SpriteEffects.None;
                    _walkDirection = "left";
                }
                else
                {
                    direction = SpriteEffects.FlipHorizontally;
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

            position = new Vector2(position.X + velocityX, position.Y + velocityY);
        }  
        
        public void CheckDeath(Mario mario, float vY)
        {
            if (vY > 0)
            {
                currentLevel.ToRemoveGameObject(this);
            }else
            {
                mario.LoseLife();
            }
        }
    }
}
