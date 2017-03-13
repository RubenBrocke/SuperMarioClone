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
        private bool _hasBeenPickedUp { get; set; }

        private SoundEffect _coinPickUpSound;

        private string _walkDirection;
        private float _speed = 1.5f;

        public Mushroom(int _x, int _y, Level lvl, ContentManager cm) : base()
        {
            Position = new Vector2(_x, _y);
            CurrentLevel = lvl;
            velocityY = 1f;
            _hasBeenPickedUp = false;
            _walkDirection = "right";
            Sprite = cm.Load<Texture2D>("Mushroom");
            _coinPickUpSound = cm.Load<SoundEffect>("Pling");
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height); // fix the magic numbers
        }

        public void collectMushroom(Mario mario)
        {
            if (!_hasBeenPickedUp)
            {
                mario.becomeBig();
                _hasBeenPickedUp = true;
                CurrentLevel.ToRemoveGameObject(this);
                _coinPickUpSound.Play();
            }
        }

        public override void Update()
        {
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Hitbox.Width, Hitbox.Height);

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
                velocityX = -_speed;
            }
            if (_walkDirection.Equals("right"))
            {
                velocityX = _speed;
            }

            Position = new Vector2(Position.X + velocityX, Position.Y + velocityY);
        }   
    }
}
