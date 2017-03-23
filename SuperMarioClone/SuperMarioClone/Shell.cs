using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace SuperMarioClone
{
    class Shell : Tangible, IMovable, ISolid
    {
        private bool HasBeenPickedUp { get; set; }

        public float VelocityX { get; protected set; }
        public float VelocityY { get; private set; }
        public float JumpVelocity { get; private set; }
        public float Gravity { get; private set; }

        private float _speed = 3f;
        private Animator _animator;

        public Shell(float x, float y, Level level, ContentManager contentManager) : base()
        {
            Gravity = 0.3f;
            CurrentLevel = level;
            VelocityX = _speed;
            Position = new Vector2(x, y);
            _animator = new Animator(contentManager.Load<Texture2D>("koopasheet"), 110);
            _animator.GetTextures(64, 0, 16, 16, 4, 1);
            Sprite = _animator.GetCurrentTexture();
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height);
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
                    Direction = SpriteEffects.FlipHorizontally;
                    VelocityX = -_speed;
                }
                else
                {
                    Direction = SpriteEffects.None;
                    VelocityX = _speed;
                }
            }
            VelocityY = vY;

            Position = new Vector2(Position.X + VelocityX, Position.Y + VelocityY);
            Sprite = _animator.GetCurrentTexture();
        }

        public void CheckHit(Mario m)
        {
            Console.WriteLine(m.VelocityY);
            if (m.VelocityY > 0.5)
            {
                VelocityX = 0;
            }
            else
            {
                if (VelocityX == 0 && m.Hitbox.Y > Hitbox.Y - m.Hitbox.Height)
                 {
                    if (m.VelocityX > 0)
                    {
                        VelocityX = _speed;
                    }
                    else if (m.VelocityX < 0)
                    {
                        VelocityX = -_speed;
                    }
                }
                else
                {
                    if (m.Hitbox.Y >= Hitbox.Y - Hitbox.Height)
                    {
                        m.LoseLife();
                    }
                }
            }
        }
    }
}
