﻿using System;
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
        public bool HasBeenPickedUp { get; set; }
        public bool IsMysteryCoin { get; set; }

        /// <summary>
        /// Implementations from IMovable
        /// </summary>
        public float VelocityX { get; }
        public float VelocityY { get; private set; }
        public float JumpVelocity { get; }
        public float Gravity { get; private set; }

        private bool _shouldBeDeleted;

        private SoundEffect _coinPickUpSound;
        private Animator _animator;
        private int _hitBoxWidth;
        private int _hitBoxHeight;

        public Coin(int x, int y, Level level, ContentManager contentManager) : base()
        {
            _shouldBeDeleted = false;
            _hitBoxWidth = Global.Instance.GridSize / 4 * 3;
            _hitBoxHeight = Global.Instance.GridSize;
            Gravity = 0.3f;
            Position = new Vector2(x * Global.Instance.GridSize, y * Global.Instance.GridSize);
            CurrentLevel = level;
            VelocityY = -2f;
            IsMysteryCoin = false;
            HasBeenPickedUp = false;
            _animator = new Animator(contentManager.Load<Texture2D>("CoinSheet"), 180);
            _animator.GetTextures(0, 0, 16, 16, 4, 1);
            Sprite = _animator.GetCurrentTexture();
            _coinPickUpSound = contentManager.Load<SoundEffect>("Pling");
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, _hitBoxWidth, _hitBoxHeight); 
        }

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
                if (_coinPickUpSound != null)
                {
                    _coinPickUpSound.Play();
                }
                mario.AddCoin();
                HasBeenPickedUp = true;                
            }
        }

        public void DeleteCoin(object state)
        {
            _shouldBeDeleted = true;
        }

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

