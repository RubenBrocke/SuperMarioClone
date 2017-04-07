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
    public class Coin : Tangible, IMovable
    {
        //Implementation of IMovable
        public float VelocityX { get; }
        public float VelocityY { get; private set; }
        public float JumpVelocity { get; }
        public float Gravity { get; private set; }

        //Properties
        public bool HasBeenPickedUp { get; private set; }
        public bool IsMysteryCoin { get; private set; }
        public int Value { get; private set; }

        //Private fields
        private bool _shouldBeDeleted;
        private SoundEffect _coinPickUpSound;
        private SoundEffect _weirdCoinPickUpSound;
        private Animator _animator;
        private int _hitBoxWidth;
        private int _hitBoxHeight;

        /// <summary>
        /// Constructor for Coin, sets the position of the Coin using the GridSize and sets its SpriteSheet and Animation
        /// </summary>
        /// <param name="x">X position of the Coin</param>
        /// <param name="y">Y position of the Coin</param>
        /// <param name="level">Level the Coin should be in</param>
        /// <param name="contentManager">ContentManager used to load SpriteSheet</param>
        public Coin(int x, int y, Level level, ContentManager contentManager) : base()
        {
            _shouldBeDeleted = false;
            _hitBoxWidth = Global.Instance.GridSize / 4 * 3;
            _hitBoxHeight = Global.Instance.GridSize;
            Gravity = 0.3f;
            Position = new Vector2(x * Global.Instance.GridSize, y * Global.Instance.GridSize);
            CurrentLevel = level;
            VelocityY = -2f;
            Value = 1;
            IsMysteryCoin = false;
            HasBeenPickedUp = false;
            _animator = new Animator(contentManager.Load<Texture2D>("CoinSheet"), 180);
            _animator.GetTextures(0, 0, 16, 16, 4, 1);
            Sprite = _animator.GetCurrentTexture();
            _coinPickUpSound = contentManager.Load<SoundEffect>("Coin");
            _weirdCoinPickUpSound = contentManager.Load<SoundEffect>("Pling");
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, _hitBoxWidth, _hitBoxHeight); 
        }

        /// <summary>
        /// Constructor for Coin, sets the position of the Coin using the GridSize and sets its SpriteSheet and Animation
        /// </summary>
        /// <param name="x">X position of the Coin</param>
        /// <param name="y">Y position of the Coin</param>
        /// <param name="level">Level the Coin should be in</param>
        /// <param name="contentManager">ContentManager used to load SpriteSheet</param>
        /// <param name="isMysteryCoin">Bool used to check if the coin is a coin Ejected from a MysteryBlock</param>
        public Coin(int x, int y, Level level, ContentManager contentManager, bool isMysteryCoin) : this(x, y, level, contentManager)
        {
            IsMysteryCoin = isMysteryCoin;
        }

        /// <summary>
        /// Adds a coin to Mario
        /// </summary>
        /// <param name="mario">Specifies which Mario to add the coin to</param>
        public void AddCoin(Mario mario)
        {
            if (!HasBeenPickedUp)
            {
                Timer deathTimer = new Timer(DeleteCoin);
                if (IsMysteryCoin)
                {
                    deathTimer.Change(200, Timeout.Infinite);
                }
                else
                {
                    deathTimer.Change(0, Timeout.Infinite);
                }
                if (Global.Instance.WeirdSounds)
                {
                    if (_weirdCoinPickUpSound != null)
                    {
                        _weirdCoinPickUpSound.Play();
                    }
                }
                else
                {
                    if (_coinPickUpSound != null)
                    {
                        _coinPickUpSound.Play();
                    }
                }
                mario.AddCoin(Value);
                HasBeenPickedUp = true;                
            }
        }

        /// <summary>
        /// Sets the _shouldBeDeleted variable to true
        /// </summary>
        /// <param name="state"></param>
        public void DeleteCoin(object state)
        {
            _shouldBeDeleted = true;
        }

        /// <summary>
        /// Updates the Coin and deletes it if _shouldBeDeleted is true
        /// </summary>
        public override void Update()
        {
            if (IsMysteryCoin)
            {
                Position = new Vector2(Position.X, Position.Y + VelocityY);
                VelocityY += Gravity;
            }
            if (_shouldBeDeleted)
            {
                CurrentLevel.ToRemoveGameObject(this);
            }
            Sprite = _animator.GetCurrentTexture();
        }
    } 
}

