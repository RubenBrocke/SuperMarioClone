using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SuperMarioClone
{
    class Muncher : Tangible, ISolid
    {
        private Texture2D _spriteSheet;
        private Animator _animator;

        public Muncher(int x, int y, Level level, ContentManager contentManager)
        {
            //Properties and private fields are set
            Position = new Vector2(x * Global.Instance.GridSize, y * Global.Instance.GridSize);
            CurrentLevel = level;

            //Sprite, animation and hitbox are set
            _spriteSheet = contentManager.Load<Texture2D>("MuncherSheet");
            _animator = new Animator(_spriteSheet, 110);
            _animator.GetTextures(0, 0, Global.Instance.GridSize, Global.Instance.GridSize, 2, 1);
            Sprite = _animator.GetCurrentTexture();
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height);
        }

        public override void Update()
        {
            //Update sprite
            UpdateSprite();
        }

        private void UpdateSprite()
        {
            Sprite = _animator.GetCurrentTexture();
        }
    }
}