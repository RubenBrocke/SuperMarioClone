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
            position = new Vector2(_x, _y);
            currentLevel = lvl;
            velocityY = 1f;
            _hasBeenPickedUp = false;
            _walkDirection = "right";
            sprite = cm.Load<Texture2D>("Mushroom");
            _coinPickUpSound = cm.Load<SoundEffect>("Pling");
            hitbox = new Rectangle((int)position.X, (int)position.Y, sprite.Width, sprite.Height); // fix the magic numbers
        }

        public void collectMushroom(Mario mario)
        {
            if (!_hasBeenPickedUp)
            {
                mario.becomeBig();
                _hasBeenPickedUp = true;
                currentLevel.ToRemoveGameObject(this);
                _coinPickUpSound.Play();
            }
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
                velocityX = -_speed;
            }
            if (_walkDirection.Equals("right"))
            {
                velocityX = _speed;
            }

            position = new Vector2(position.X + velocityX, position.Y + velocityY);
        }   
    }
}
