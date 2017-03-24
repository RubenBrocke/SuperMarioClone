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
    class Goomba : Tangible, IMovable
    {
        public float VelocityX { get; protected set; }
        public float VelocityY { get; private set; }
        public float JumpVelocity { get; private set; }
        public float Gravity { get; private set; }

        private float _speed;
        private Texture2D _spriteSheet;
        private Animator _animator;

        public Goomba(int x, int y, Level level, ContentManager contentManager)
        {
            //Properties and private fields are set
            Position = new Vector2(x * Global.Instance.GridSize, y * Global.Instance.GridSize);
            CurrentLevel = level;

            _speed = 2f;
            VelocityX = _speed;
            Gravity = 0.3f;

            _horizontalPadding = 1; //FIXME Waarom heeft Goomba een padding?
            _verticalPadding = 0;

            //Sprite, animation and hitbox are set
            _spriteSheet = contentManager.Load<Texture2D>("GoombaSheet");
            _animator = new Animator(_spriteSheet, 110);
            _animator.GetTextures(0, 0, Global.Instance.GridSize, Global.Instance.GridSize, 2, 1);
            Sprite = _animator.GetCurrentTexture();          
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height);
        }

        public override void Update()
        {
            //Update hitbox to match current position
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Hitbox.Width, Hitbox.Height);

            //Add gravity to vertical velocity
            VelocityY += Gravity;

            //Check collision and change direction if needed
            float vX;
            float vY;
            if (CheckCollision(this, out vX, out vY))
            {
                if (VelocityX > 0)
                {
                    Direction = SpriteEffects.None;
                    VelocityX = -_speed;
                }
                else
                {
                    Direction = SpriteEffects.FlipHorizontally;
                    VelocityX = _speed;
                }
            }
            VelocityY = vY;

            //Update position
            Position = new Vector2(Position.X + VelocityX, Position.Y + VelocityY);

            //Update sprite
            Sprite = _animator.GetCurrentTexture();
        }  
        
        public void CheckDeath(Mario mario, float vY)
        {
            if (vY > 0)
            {
                CurrentLevel.ToRemoveGameObject(this);
            }else
            {
                mario.LoseLife();
            }
        }
    }
}
