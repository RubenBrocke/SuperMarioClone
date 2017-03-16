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
    class Goomba : Tangible, IMovable
    {
        public float VelocityX { get; protected set; }
        public float VelocityY { get; private set; }
        public float JumpVelocity { get; private set; }
        public float Gravity { get; private set; }

        private string _walkDirection;
        private float _speed = 2f;

        public Goomba(int x, int y, Level level, ContentManager contentManager)
        {
            Gravity = 0.3f;
            Position = new Vector2(x, y);
            CurrentLevel = level;
            VelocityX = 2f;
            _walkDirection = "right";
            Sprite = contentManager.Load<Texture2D>("Goomba");
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height);
            _horizontalPadding = 1;
            _verticalPadding = 0;
        }

        public override void Update()
        {
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Hitbox.Width, Hitbox.Height);

            VelocityY += Gravity;
            float vX;
            float vY;
            
            if (CheckCollision(this, out vX, out vY))
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

            VelocityX = vX;
            VelocityY = vY;

            if (_walkDirection.Equals("left"))
            {
                VelocityX = -_speed;
            }
            if (_walkDirection.Equals("right"))
            {
                VelocityX = _speed;
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
