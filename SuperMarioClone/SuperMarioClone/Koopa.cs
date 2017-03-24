using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace SuperMarioClone
{
    class Koopa : Tangible, IMovable, ISolid
    {
        public float VelocityX { get; protected set; }
        public float VelocityY { get; private set; }
        public float JumpVelocity { get; private set; }
        public float Gravity { get; private set; }

        private float _speed;
        private bool _isHit;
        private ContentManager _contentManager;
        private Texture2D _spriteSheet;
        private Animator _animator;

        public Koopa(float x, float y, Level level, ContentManager contentManager)
        {
            //Properties and private fields are set
            Position = new Vector2(x * Global.Instance.GridSize, y * Global.Instance.GridSize);
            CurrentLevel = level;

            _speed = 0.5f;
            VelocityX = _speed;
            Gravity = 0.3f;

            _horizontalPadding = 1; //FIXME Waarom heeft Koopa een padding?
            _verticalPadding = 0;
            _isHit = false;
            _contentManager = contentManager;

            //Sprite, animation and hitbox are set
            _spriteSheet = contentManager.Load<Texture2D>("GreenKoopaSheet");
            _animator = new Animator(_spriteSheet, 200);
            _animator.GetTextures(0, 0, Global.Instance.GridSize, _spriteSheet.Height, 2, 1);
            Sprite = _animator.GetCurrentTexture();
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height);
        }

        public override void Update()
        {
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Hitbox.Width, Hitbox.Height);

            VelocityY += Gravity;
            float vX;
            float vY;
            Console.WriteLine(VelocityX);

            if (CheckCollision(this, out vX, out vY))
            {
                if (VelocityX > 0)
                {
                    Direction = SpriteEffects.FlipHorizontally;
                    VelocityX = -_speed;
                }
                else
                {
                    Direction = SpriteEffects.None;
                    VelocityX = _speed;
                }
            }
            VelocityY = vY;

            Position = new Vector2(Position.X + VelocityX, Position.Y + VelocityY);
            Sprite = _animator.GetCurrentTexture();
        }

        public void CheckDeath(Mario mario, float vY)
        {
            if (!_isHit)
            {
                if (vY > 0.5)
                {
                    mario.Jump();
                    CurrentLevel.ToRemoveGameObject(this);
                    CurrentLevel.ToAddGameObject(new Shell(Position.X, Position.Y + Hitbox.Height - Global.Instance.GridSize, CurrentLevel, _contentManager));
                    _isHit = true;
                }
                else
                {
                    mario.LoseLife();
                }
            }
        }
    }
}

