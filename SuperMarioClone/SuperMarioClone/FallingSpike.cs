using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarioClone
{
    class FallingSpike : GameObject, IMovable
    {
        public float VelocityX { get; protected set; }
        public float VelocityY { get; private set; }
        public float JumpVelocity { get; private set; }
        public float Gravity { get; private set; }
        public Rectangle Hitbox { get; set; }
        public Rectangle FallHitbox { get; set; }
        private bool _goFall;


        public FallingSpike(int x, int y, Level level, ContentManager contentManager) : base()
        {
            Position = new Vector2(x * Global.Instance.GridSize, y * Global.Instance.GridSize);
            CurrentLevel = level;
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Global.Instance.GridSize, Global.Instance.GridSize);
            FallHitbox = new Rectangle((int)Position.X, (int)Position.Y, Global.Instance.GridSize, level.Height);
            Sprite = contentManager.Load<Texture2D>("FallingSpike");
            VelocityX = 0;
            Gravity = 0.3f;
            _goFall = false;
        }

        public override void Update()
        {
            Mario m = Global.Instance.MainGame.mario;
            //Checks if mario is beneath the spike
            CheckFall(m);

            if (_goFall)
            {
                //Update hitbox to match current position
                UpdateHitbox();

                //Add gravity to vertical velocity
                AddGravity();

                //Update position
                UpdatePosition();

                //Check if mario gets hit
                CheckHit(m);
            }
        }

        /// <summary>
        /// Updates Shell's Hitbox to match the current Position
        /// </summary>
        private void UpdateHitbox()
        {
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Hitbox.Width, Hitbox.Height);
        }

        /// <summary>
        /// Adds Gravity to Shell's vertical velocity
        /// </summary>
        private void AddGravity()
        {
            VelocityY += Gravity;
        }


        /// <summary>
        /// Updates Shell's current Position using its velocity
        /// </summary>
        private void UpdatePosition()
        {
            Position = new Vector2(Position.X + VelocityX, Position.Y + VelocityY);
        }

        /// <summary>
        /// Checks if Shell should move or hit Mario instead
        /// </summary>
        /// <param name="mario">Used to check if Mario jumped on top of the Shell or hit the side</param>
        public void CheckHit(Mario mario)
        {
            if (mario.Hitbox.Intersects(Hitbox))
            {
                mario.GetHit();
            }
        }

        public void CheckFall(Mario mario)
        {
            if (mario.Hitbox.Intersects(FallHitbox))
            {
                _goFall = true;
            }
        }
    }
}
