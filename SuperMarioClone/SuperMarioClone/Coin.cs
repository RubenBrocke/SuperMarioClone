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
        public int Value { get; set; }
        public bool Moveable { get; set; }
        public bool HasBeenPickedUp { get; set; }
        public bool IsMysteryCoin { get; set; }

        public float VelocityX { get; protected set; }
        public float VelocityY { get; private set; }
        public float JumpVelocity { get; private set; }
        public float Gravity { get; private set; }

        private SoundEffect _coinPickUpSound;
       
        private Animator _animator;
        private int rectX;
        private int rectY;

        public Coin(int x, int y, Level level, ContentManager contentManager) : base()
        {
            rectX = 16;
            rectY = 12;
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
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, rectY, rectX); 
        }

        public void AddCoin(Mario mario) // FIX THE GODDAMN DEATHTIMER PLEASE, IT CREATES WAY TO MUCH ERRORS, THANK YOU
        {
            if (!HasBeenPickedUp)
            {
                Timer deathTimer = new Timer(DeleteCoin);
                if (IsMysteryCoin)
                {
                    deathTimer.Change(200, 0);
                }
                else
                {
                    deathTimer.Change(0, 0);
                }
                mario.AddCoin();
                HasBeenPickedUp = true;                
            }
        }

        public void DeleteCoin(object state)
        {
            _coinPickUpSound.Play();
            CurrentLevel.ToRemoveGameObject(this);
        }

        public override void Update()
        {
            if (IsMysteryCoin)
            {
                Position = new Vector2(Position.X, Position.Y + VelocityY);
                VelocityY += Gravity;
            }
            Sprite = _animator.GetCurrentTexture();
        }
    } 
}

