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
    class MysteryBlock : Tangible, ISolid
    {
        private Type MysteryObject { get; set; }
        private ContentManager _contentManager;
        private Animator _animator;
        private bool _hasBeenUsed = false;

        public MysteryBlock(int x, int y, Type MysteryObject, Level level, ContentManager contentManager) : base()
        {
            this.MysteryObject = MysteryObject;
            Position = new Vector2(x * Global.Instance.GridSize, y * Global.Instance.GridSize);
            CurrentLevel = level;
            _contentManager = contentManager;
            _animator = new Animator(_contentManager.Load<Texture2D>("MysteryBlockSheet"), 120);
            _animator.GetTextures(0, 0, 16, 16, 4, 1);
            Sprite = _animator.GetCurrentTexture();
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, 16, 16); // TODO: numbers represent pixels, change magic number
        }

        public void Eject(Mario mario, float vY, float Y)
        {
            if (vY < 0 && !_hasBeenUsed && mario.Hitbox.Y > Hitbox.Bottom)
            {
                if (MysteryObject == typeof(Coin))
                {
                    Coin c = (Coin)Activator.CreateInstance(MysteryObject, (int)Position.X, (int)Position.Y - Hitbox.Height, CurrentLevel, _contentManager);
                    c.IsMysteryCoin = true;                 
                    c.AddCoin(mario);
                    CurrentLevel.ToAddGameObject(c);
                }
                if (MysteryObject == typeof(Mushroom))
                {
                    Mushroom m = (Mushroom)Activator.CreateInstance(MysteryObject, (int)Position.X, (int)Position.Y - Hitbox.Height, CurrentLevel, _contentManager);
                    CurrentLevel.ToAddGameObject(m);
                }
                if (MysteryObject == typeof(LevelReader))
                {
                    LevelReader _lr = (LevelReader)Activator.CreateInstance(MysteryObject, _contentManager);
                    MainGame.currentLevel = _lr.ReadLevel(1);
                }
                _hasBeenUsed = true;
                _animator.GetTextures(64, 0, 16, 16, 1, 1);
                _animator.SetAnimationSpeed(0);
            } 
        }

        public override void Update()
        {
            Sprite = _animator.GetCurrentTexture();
        }
    }
}
