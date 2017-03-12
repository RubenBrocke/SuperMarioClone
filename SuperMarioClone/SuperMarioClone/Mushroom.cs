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

        public Mushroom(int _x, int _y, Level lvl, ContentManager cm) : base()
        {
            position = new Vector2(_x, _y);
            currentLevel = lvl;
            velocityY = 1f;
            _hasBeenPickedUp = false;
            _walkDirection = "right";
            sprite = cm.Load<Texture2D>("Mushroom");
            _coinPickUpSound = cm.Load<SoundEffect>("Pling");
            hitbox = new Rectangle((int)position.X, (int)position.Y, 12, 16); // fix the magic numbers
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
    }
}
