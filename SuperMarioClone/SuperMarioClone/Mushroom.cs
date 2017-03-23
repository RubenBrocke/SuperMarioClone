using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace SuperMarioClone
{
    class Mushroom : Tangible, IMovable
    {
        private bool HasBeenPickedUp { get; set; }

        public float VelocityX { get; protected set; }
        public float VelocityY { get; private set; }
        public float JumpVelocity { get; private set; }
        public float Gravity { get; private set; }

        private SoundEffect _coinPickUpSound;

        private string _walkDirection;
        private float _speed = 1.5f;

        public Mushroom(int x, int y, Level level, ContentManager contentManager) : base()
        {
            Gravity = 0.3f;
            Position = new Vector2(x * Global.Instance.GridSize, y * Global.Instance.GridSize);
            CurrentLevel = level;
            VelocityY = 1f;
            HasBeenPickedUp = false;
            _walkDirection = "right";
            Sprite = contentManager.Load<Texture2D>("Mushroom");
            _coinPickUpSound = contentManager.Load<SoundEffect>("Pling");
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height); // fix the magic numbers
        }

        public void CollectMushroom(Mario mario)
        {
            if (!HasBeenPickedUp)
            {
                mario.BecomeBig();
                HasBeenPickedUp = true;
                CurrentLevel.ToRemoveGameObject(this);
                _coinPickUpSound.Play();
            }
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
    }
}
