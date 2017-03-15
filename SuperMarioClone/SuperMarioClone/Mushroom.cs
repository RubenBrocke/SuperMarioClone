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
    class Mushroom : Tangible
    {
        private bool HasBeenPickedUp { get; set; }

        private SoundEffect _coinPickUpSound;

        private string _walkDirection;
        private float _speed = 1.5f;

        public Mushroom(int x, int y, Level lvl, ContentManager cm) : base()
        {
            Position = new Vector2(x, y);
            CurrentLevel = lvl;
            VelocityY = 1f;
            HasBeenPickedUp = false;
            _walkDirection = "right";
            Sprite = cm.Load<Texture2D>("Mushroom");
            _coinPickUpSound = cm.Load<SoundEffect>("Pling");
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
