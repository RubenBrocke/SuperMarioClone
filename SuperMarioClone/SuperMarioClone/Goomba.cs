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
        }

        public override void Update()
        {
            hitbox = new Rectangle((int)position.X, (int)position.Y, hitbox.Width, hitbox.Height);

            velocityY += gravity;

            if (_walkDirection.Equals("left"))
            {
                velocityX = -2;
            }
            if (_walkDirection.Equals("right"))
            {
                velocityX = 2;
            }

            if (IsColliding(currentLevel, (int)Math.Ceiling(velocityX), 0, out outRect) || IsColliding(currentLevel, (int)Math.Floor(velocityX), 0, out outRect))
            {
                if (velocityX < 0)
                {
                    position = new Vector2(outRect.Right, position.Y);
                    _walkDirection = "right";
                    direction = SpriteEffects.FlipHorizontally;
                }
                else if (velocityX > 0)
                {
                    position = new Vector2(outRect.Left - hitbox.Width, position.Y);
                    _walkDirection = "left";
                    direction = SpriteEffects.None;
                }
                velocityX = 0;
            }

            if (IsColliding(currentLevel, 0, (int)Math.Ceiling(velocityY), out outRect) || IsColliding(currentLevel, 0, (int)Math.Floor(velocityY), out outRect))
            {
                if (velocityY > 0)
                {
                    position = new Vector2(position.X, outRect.Top - hitbox.Height);
                }
                else if (velocityY < 0)
                {
                    position = new Vector2(position.X, outRect.Bottom);
                }
                velocityY = 0;
            }
            position = new Vector2(position.X + velocityX, position.Y + velocityY);
        }  
        
        public void CheckCollision(float vX, float vY)
        {
            if (vY > 0)
            {
                currentLevel.ToRemoveGameObject(this);
            }else
            {
                //Do damage to mario
            }
        }      
    }
}
