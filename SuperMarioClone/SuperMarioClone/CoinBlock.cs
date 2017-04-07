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
    public class CoinBlock : Tangible
    {
        public  int ContainAmount { get; private set; }

        private ContentManager _contentManager;
        private Animator _animator;
        private bool _hasBeenUsed = false;
        private int _hitBoxWidth;
        private int _hitBoxHeight;

        /// <summary>
        /// Constructor for CoinBlock, sets the position of the CoinBlock using the GridSize and sets its SpriteSheet
        /// </summary>
        /// <param name="x">X position of the CoinBlock</param>
        /// <param name="y">Y position of the CoinBlock</param>
        /// <param name="containAmount">Amount of coins that should be in the CoinBlock</param>
        /// <param name="level">Level the CoinBlock should be in</param>
        /// <param name="contentManager">ContentManager used to load SpriteSheet</param>
        public CoinBlock(int x, int y, int containAmount, Level level, ContentManager contentManager) : base()
        {
            _hitBoxWidth = Global.Instance.GridSize;
            _hitBoxHeight = Global.Instance.GridSize;
            ContainAmount = containAmount;
            Position = new Vector2(x * Global.Instance.GridSize, y * Global.Instance.GridSize);
            CurrentLevel = level;
            _contentManager = contentManager;
            _animator = new Animator(_contentManager.Load<Texture2D>("CoinBlockSheet"), 0);
            _animator.GetTextures(0, 0, 16, 16, 1, 1);
            Sprite = _animator.GetCurrentTexture();
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, _hitBoxWidth, _hitBoxHeight);
            IsSolid = true;
        }

        /// <summary>
        /// Ejects a coin
        /// </summary>
        /// <param name="mario">Used to check if Mario is below the CoinBlock and used to give a coin to Mario</param>
        public void Eject(Mario mario)
        {
            if (mario.VelocityY < 0 && !_hasBeenUsed && mario.Hitbox.Y >= Hitbox.Bottom)
            {
                Coin c = (Coin)Activator.CreateInstance(typeof(Coin), (int)Position.X / Global.Instance.GridSize, ((int)Position.Y - Hitbox.Height) / Global.Instance.GridSize, CurrentLevel, _contentManager, true);
                c.AddCoin(mario);
                CurrentLevel.ToAddGameObject(c);
                if (ContainAmount-- <= 0)
                {
                    _animator.GetTextures(16, 0, 16, 16, 1, 1);
                    Sprite = _animator.GetCurrentTexture();
                    _hasBeenUsed = true;
                }
            }
        }
    }
}
