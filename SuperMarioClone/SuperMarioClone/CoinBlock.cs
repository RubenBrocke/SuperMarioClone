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
    class CoinBlock : Tangible, ISolid
    {
        private int ContainAmount { get; set; }
        private ContentManager _contentManager;
        private bool _hasBeenUsed = false;
        private int _spriteImageIndex = 0;

        public CoinBlock(int x, int y, int containAmount, Level level, ContentManager contentManager) : base()
        {
            ContainAmount = containAmount;
            Position = new Vector2(x, y);
            CurrentLevel = level;
            _contentManager = contentManager;
            Sprite = _contentManager.Load<Texture2D>("CoinBlockSheet");
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, 16, 16); // TODO: numbers represent pixels, change magic number
        }

        public void Eject(Mario mario, float vY, float Y)
        {
            if (vY < 0 && !_hasBeenUsed && Y > Hitbox.Bottom)
            {
                Coin c = (Coin)Activator.CreateInstance(typeof(Coin), (int)Position.X, (int)Position.Y - Hitbox.Height, CurrentLevel, _contentManager);
                c.IsMysteryCoin = true;
                c.AddCoin(mario);
                CurrentLevel.ToAddGameObject(c);
                if (ContainAmount-- == 0)
                {
                    _spriteImageIndex = 1;
                    _hasBeenUsed = true;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture: Sprite, position: Position, sourceRectangle: new Rectangle(16 * _spriteImageIndex, 0, 16, 16));
        }
    }
}
