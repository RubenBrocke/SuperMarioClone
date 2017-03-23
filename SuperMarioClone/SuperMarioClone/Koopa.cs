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

        private Animator _animator;
        private float _speed = 0.5f;
        private GameObjectFactory _factory;
        private ContentManager _contentManager;
        private bool _isHit;

        public Koopa(float x, float y, Level level, ContentManager contentManager)
        {
            _isHit = false;
            _factory = new GameObjectFactory();
            _contentManager = contentManager;
            _animator = new Animator(contentManager.Load<Texture2D>("koopasheet"), 200);
            _animator.GetTextures(0, 0, 16, 27, 2, 1);
            Sprite = _animator.GetCurrentTexture();
            Gravity = 0.3f;
            Position = new Vector2(x * Global.Instance.GridSize, y * Global.Instance.GridSize);
            CurrentLevel = level;
            VelocityX = 2f;
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height);
            _horizontalPadding = 1;
            _verticalPadding = 0;
        }

        public override void Update()
        {
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Hitbox.Width, Hitbox.Height);

            VelocityY += Gravity;
            float vX;
            float vY;

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
                if (vY > 0)
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

