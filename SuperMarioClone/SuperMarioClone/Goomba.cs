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

        private Animator _animator;
        private float _speed = 2f;

        public Goomba(int x, int y, Level level, ContentManager contentManager)
        {
            _animator = new Animator(contentManager.Load<Texture2D>("GoombaSheet"), 110);
            _animator.GetTextures(0, 0, 16, 16, 2, 1);
            Sprite = _animator.GetCurrentTexture();
            Gravity = 0.3f;
            Position = new Vector2(x, y);
            CurrentLevel = level;
            VelocityX = 2f;
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
                if (VelocityX > 0)
                {
                    Direction = SpriteEffects.None;
                    VelocityX = -_speed;
                }
                else
                {
                    Direction = SpriteEffects.FlipHorizontally;
                    VelocityX = _speed;
                }
            }
            VelocityY = vY;

            Position = new Vector2(Position.X + VelocityX, Position.Y + VelocityY);
            Sprite = _animator.GetCurrentTexture();
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
