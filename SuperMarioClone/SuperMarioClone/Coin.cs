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
        private int Value { get; set; }

        private bool Moveable { get; set; }

        private bool HasBeenPickedUp { get; set; }
        private bool IsMysteryCoin { get; set; }

        private SoundEffect _coinPickUpSound;

        private Animator animator;

        private int _spriteImageIndex;

        public Coin(int _x, int _y, bool isMystereyCoin, Level lvl, ContentManager cm) : base()
        {
            Position = new Vector2(_x, _y);
            CurrentLevel = lvl;
            VelocityY = -2f;
            IsMysteryCoin = isMystereyCoin;
            HasBeenPickedUp = false;
            _spriteImageIndex = 0;
            animator = new Animator(cm.Load<Texture2D>("CoinSheet"), 180);
            animator.GetTextures(0, 0, 16, 16, 4, 1);
            Sprite = animator.GetCurrentTexture();
            _coinPickUpSound = cm.Load<SoundEffect>("Pling");
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, 12, 16); // fix the magic numbers
        }

        public void AddCoin(Mario mario)
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
                mario.addCoin();
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
                VelocityY += gravity;
            }
            Sprite = animator.GetCurrentTexture();
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture: Sprite, position: Position);
        }
    } 
}

