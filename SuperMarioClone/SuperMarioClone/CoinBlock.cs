using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SuperMarioClone
{
    class CoinBlock : Solid
    {
        private int ContainAmount { get; set; }
        private ContentManager _cm;
        private bool _hasBeenUsed = false;
        private int _spriteImageIndex = 0;

        public CoinBlock(int _x, int _y, Level lvl, ContentManager cm, int containAmount) : base()
        {
            ContainAmount = containAmount;
            Position = new Vector2(_x, _y);
            CurrentLevel = lvl;
            _cm = cm;
            Sprite = _cm.Load<Texture2D>("CoinBlockSheet");
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, 16, 16); // TODO: numbers represent pixels, change magic number
        }

        public void Eject(Mario mario, float vY, float Y)
        {
            if (vY < 0 && !_hasBeenUsed && Y > Hitbox.Bottom)
            {
                Coin c = (Coin)Activator.CreateInstance(typeof(Coin), (int)Position.X, (int)Position.Y - Hitbox.Height, true, CurrentLevel, _cm);
                c.AddCoin(mario);
                CurrentLevel.ToAddGameObject(c);
                if (ContainAmount-- == 0)
                {
                    _hasBeenUsed = true;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture: Sprite, position: Position, sourceRectangle: new Rectangle(16 * _spriteImageIndex, 0, 16, 16));
        }

        private void ChangeSpriteIndex(object state)
        {
            if (_hasBeenUsed)
            {
                _spriteImageIndex = 2;
            }
        }
    }
}
