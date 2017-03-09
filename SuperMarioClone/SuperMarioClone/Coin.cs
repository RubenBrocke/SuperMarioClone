using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Threading;

namespace SuperMarioClone
{
    public class Coin : Tangible
    {
        private int _value { get; set; }

        private bool _moveable { get; set; }

        private bool _hasBeenPickedUp { get; set; }

        private Timer _timer;

        private int _spriteImageIndex;

        public Coin(int _x, int _y, Level lvl, ContentManager cm) : base()
        {
            X = _x;
            Y = _y;
            currentLevel = lvl;
            _hasBeenPickedUp = false;
            _spriteImageIndex = 0;
            _timer = new Timer(ChangeSpriteIndex);
            _timer.Change(0, 190);
            sprite = cm.Load<Texture2D>("CoinSheet");
            hitbox = new Rectangle(X, Y, 12, 16); // fix the magic numbers
        }

        public void AddCoin(Mario mario)
        {
            if (!_hasBeenPickedUp)
            {
                mario.addCoin();
                currentLevel.ToRemoveGameObject(this);
                _hasBeenPickedUp = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (_spriteImageIndex)
            {
                case 0:
                    spriteBatch.Draw(sprite, new Vector2(X, Y), sourceRectangle: new Rectangle(0, 0, 16, 16));
                    break;
                case 1:
                    spriteBatch.Draw(sprite, new Vector2(X, Y), sourceRectangle: new Rectangle(16, 0, 16, 16));
                    break;
                case 2:
                    spriteBatch.Draw(sprite, new Vector2(X, Y), sourceRectangle: new Rectangle(32, 0, 16, 16));
                    break;
                case 3:
                    spriteBatch.Draw(sprite, new Vector2(X, Y), sourceRectangle: new Rectangle(48, 0, 16, 16));
                    break;
            }
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

