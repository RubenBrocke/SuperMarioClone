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
        private Animator _animator;
        private bool _hasBeenUsed = false;

        public CoinBlock(int x, int y, int containAmount, Level level, ContentManager contentManager) : base()
        {
            ContainAmount = containAmount;
            Position = new Vector2(x * Global.Instance.GridSize, y * Global.Instance.GridSize);
            CurrentLevel = level;
            _contentManager = contentManager;
            _animator = new Animator(_contentManager.Load<Texture2D>("CoinBlockSheet"), 0);
            _animator.GetTextures(0, 0, 16, 16, 1, 1);
            Sprite = _animator.GetCurrentTexture();
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, 16, 16); // TODO: numbers represent pixels, change magic number
        }

        public void Eject(Mario mario, float vY, float Y)
        {
            if (vY < 0 && !_hasBeenUsed && mario.Hitbox.Y > Hitbox.Bottom)
            {
                Coin c = (Coin)Activator.CreateInstance(typeof(Coin), (int)Position.X, (int)Position.Y - Hitbox.Height, CurrentLevel, _contentManager);
                c.IsMysteryCoin = true;
                c.AddCoin(mario);
                CurrentLevel.ToAddGameObject(c);
                if (ContainAmount-- == 0)
                {
                    _animator.GetTextures(16, 0, 16, 16, 1, 1);
                    Sprite = _animator.GetCurrentTexture();
                    _hasBeenUsed = true;
                }
            }
        }
    }
}
