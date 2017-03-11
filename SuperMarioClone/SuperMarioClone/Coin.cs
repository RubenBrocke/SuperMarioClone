using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using System.Threading;

namespace SuperMarioClone
{
    public class Coin : Tangible
    {
        private int _value { get; set; }

        private bool _moveable { get; set; }

        private bool _hasBeenPickedUp { get; set; }

        private SoundEffect _coinPickUpSound;

        private Timer _timer;

        private int _spriteImageIndex;

        public Coin(int _x, int _y, Level lvl, ContentManager cm) : base()
        {
            position = new Vector2(_x, _y);
            currentLevel = lvl;
            _hasBeenPickedUp = false;
            _spriteImageIndex = 0;
            _timer = new Timer(ChangeSpriteIndex);
            _timer.Change(0, 190);
            sprite = cm.Load<Texture2D>("CoinSheet");
            _coinPickUpSound = cm.Load<SoundEffect>("Pling");
            hitbox = new Rectangle((int)position.X, (int)position.Y, 12, 16); // fix the magic numbers
        }

        public void AddCoin(Mario mario)
        {
            if (!_hasBeenPickedUp)
            {
                mario.addCoin();
                currentLevel.ToRemoveGameObject(this);
                _hasBeenPickedUp = true;
                _coinPickUpSound.Play();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture: sprite, position: position, sourceRectangle: new Rectangle(16 * _spriteImageIndex, 0, 16, 16));
        }

        private void ChangeSpriteIndex(object state)
        {
            if ( _spriteImageIndex < 3)
            {
                _spriteImageIndex++;
            }
            else
            {
                _spriteImageIndex = 0;
            }
        }
    } 
}

