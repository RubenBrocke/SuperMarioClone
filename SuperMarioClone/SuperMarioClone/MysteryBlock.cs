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
        public Type MysteryObject { get; private set; }
        public bool HasBeenUsed { get; private set; }

        private ContentManager _contentManager;
        private Animator _animator;
        private Texture2D _spriteSheet;
        private int _levelNumber = 0;

        

        public MysteryBlock(int x, int y, Type mysteryObject, Level level, ContentManager contentManager) : base()
        {
            //Properties and private fields are set
            Position = new Vector2(x * Global.Instance.GridSize, y * Global.Instance.GridSize);
            CurrentLevel = level;
            
            MysteryObject = mysteryObject;
            HasBeenUsed = false;    
            _contentManager = contentManager;

            //Sprite, animation and hitbox are set
            _spriteSheet = _contentManager.Load<Texture2D>("MysteryBlockSheet");
            _animator = new Animator(_spriteSheet, 120);
            _animator.GetTextures(0, 0, 16, 16, 4, 1);
            Sprite = _animator.GetCurrentTexture();
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Global.Instance.GridSize, Global.Instance.GridSize);
        }

        public MysteryBlock(int x, int y, Type mysteryObject, int levelNumber, Level level, ContentManager contentManager) : base()
        {
            //Properties and private fields are set
            Position = new Vector2(x * Global.Instance.GridSize, y * Global.Instance.GridSize);
            CurrentLevel = level;

            MysteryObject = mysteryObject;
            HasBeenUsed = false;
            _contentManager = contentManager;
            _levelNumber = levelNumber;

            //Sprite, animation and hitbox are set
            if (_levelNumber <= 3 && _levelNumber > 0)
            {
                _spriteSheet = _contentManager.Load<Texture2D>("MysteryBlockSheet" + _levelNumber);
            }
            else
            {
                _spriteSheet = _contentManager.Load<Texture2D>("MysteryBlockSheet");
            }
            _animator = new Animator(_spriteSheet, 120);
            _animator.GetTextures(0, 0, 16, 16, 4, 1);
            Sprite = _animator.GetCurrentTexture();
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Global.Instance.GridSize, Global.Instance.GridSize);
        }

        public void Eject(Mario mario)
        {
            if (mario.VelocityY < 0 && !HasBeenUsed && mario.Hitbox.Y > Hitbox.Bottom)
            {
                if (MysteryObject == typeof(Coin))
                {
                    Coin c = (Coin)Activator.CreateInstance(MysteryObject, (int)Position.X / Global.Instance.GridSize, ((int)Position.Y - Hitbox.Height) / Global.Instance.GridSize, CurrentLevel, _contentManager);
                    c.IsMysteryCoin = true;                 
                    c.AddCoin(mario);
                    CurrentLevel.ToAddGameObject(c);
                }
                if (MysteryObject == typeof(Mushroom))
                {
                    Mushroom m = (Mushroom)Activator.CreateInstance(MysteryObject, (int)Position.X / Global.Instance.GridSize, ((int)Position.Y - Hitbox.Height) / Global.Instance.GridSize, CurrentLevel, _contentManager);
                    CurrentLevel.ToAddGameObject(m);
                }
                if (MysteryObject == typeof(LevelReader))
                {
                    LevelReader _lr = (LevelReader)Activator.CreateInstance(MysteryObject, _contentManager);
                    Global.Instance.MainGame.ChangeCurrentLevel(_lr.ReadLevel(_levelNumber));
                }
                HasBeenUsed = true;
                _animator.GetTextures(64, 0, 16, 16, 1, 1);
                _animator.SetAnimationSpeed(0);
            } 
        }

        public override void Update()
        {
            //Update sprite
            Sprite = _animator.GetCurrentTexture();
        }
    }
}
