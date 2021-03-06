﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace SuperMarioClone
{
    public class MysteryBlock : Tangible
    {
        //Properties
        public Type MysteryObject { get; private set; }
        public bool HasBeenUsed { get; private set; }

        //Private fields
        private ContentManager _contentManager;
        private Animator _animator;
        private Texture2D _spriteSheet;
        private SoundEffect _courseClearSound;
        private int _levelNumber = 0;

        /// <summary>
        /// Constructor for MysteryBlock, sets the position of the MysteryBlock using the GridSize and sets its SpriteSheet and Animation
        /// </summary>
        /// <param name="x">X position of the MysteryBlock</param>
        /// <param name="y">Y position of the MysteryBlock</param>
        /// <param name="mysteryObject">Object the MysteryBlock should Eject when hit from below</param>
        /// <param name="level">Level the MysteryBlock should be in</param>
        /// <param name="contentManager">ContentManager used to load SpriteSheet and create new Objects</param>
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
            _courseClearSound = _contentManager.Load<SoundEffect>("CourseClear");
            Sprite = _animator.GetCurrentTexture();
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Global.Instance.GridSize, Global.Instance.GridSize);
            IsSolid = true;
        }

        /// <summary>
        /// Constructor for MysteryBlock, sets the position of the MysteryBlock using the GridSize and sets its SpriteSheet and Animation
        /// </summary>
        /// <param name="x">X position of the MysteryBlock</param>
        /// <param name="y">Y position of the MysteryBlock</param>
        /// <param name="mysteryObject">Object the MysteryBlock should Eject when hit from below</param>
        /// <param name="levelNumber">Number of the Level that should be loaded from the levelReader</param>
        /// <param name="level">Level the MysteryBlock should be in</param>
        /// <param name="contentManager">ContentManager used to load SpriteSheet and create new Objects</param>
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
            if (_levelNumber <= 2 && _levelNumber >= 0)
            {
                _spriteSheet = _contentManager.Load<Texture2D>("MysteryBlockSheet" + _levelNumber);
            }
            else
            {
                _spriteSheet = _contentManager.Load<Texture2D>("MysteryBlockSheet");
            }
            _animator = new Animator(_spriteSheet, 120);
            _animator.GetTextures(0, 0, 16, 16, 4, 1);
            _courseClearSound = _contentManager.Load<SoundEffect>("CourseClear");
            IsSolid = true;
            Sprite = _animator.GetCurrentTexture();
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Global.Instance.GridSize, Global.Instance.GridSize);
        }

        /// <summary>
        /// Ejects the content of the MysterBlock
        /// </summary>
        /// <param name="mario">Used to check if the mysteryBlock is being hit from below</param>
        public void Eject(Mario mario)
        {
            if (mario.VelocityY < 0 && !HasBeenUsed && mario.Hitbox.Y >= Hitbox.Bottom)
            {
                if (MysteryObject == typeof(Coin))
                {
                    Coin c = (Coin)Activator.CreateInstance(MysteryObject, (int)Position.X / Global.Instance.GridSize, ((int)Position.Y - Hitbox.Height) / Global.Instance.GridSize, CurrentLevel, _contentManager, true);            
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
                    if (_levelNumber == 0)
                    {
                        if (_courseClearSound != null)
                        {
                            MediaPlayer.Pause();
                            _courseClearSound.Play();
                            new Timer(Global.Instance.MainGame.sound.ResumeMusic).Change(8000, Timeout.Infinite);
                        }
                    }
                    LevelReader _lr = (LevelReader)Activator.CreateInstance(MysteryObject, _contentManager);
                    Global.Instance.MainGame.ChangeCurrentLevel(_lr.ReadLevel(_levelNumber));
                }
                HasBeenUsed = true;
                _animator.GetTextures(64, 0, 16, 16, 1, 1);
                _animator.SetAnimationSpeed(0);
            } 
        }
        /// <summary>
        /// Updates the MysterBlock
        /// </summary>
        public override void Update()
        {
            //Update sprite
            Sprite = _animator.GetCurrentTexture();
        }
    }
}
