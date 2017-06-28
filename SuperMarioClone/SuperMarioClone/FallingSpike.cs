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
    public class FallingSpike : GameObject, IMovable
    {
        //Implemention of IMovable
        public float VelocityX { get; protected set; }
        public float VelocityY { get; private set; }
        public float JumpVelocity { get; private set; }
        public float Gravity { get; private set; }
        
        //Other properties and variables
        public Rectangle Hitbox { get; set; }
        public Rectangle FallHitbox { get; set; }

        public Vector2 StartPosition;
        public bool _goFall;

        /// <summary>
        /// Constructor for FallingSpike, sets the position of the FallingSpike using the GridSize and sets its Sprite
        /// </summary>
        /// <param name="x">X position of the FallingSpike</param>
        /// <param name="y">Y position of the FallingSpike</param>
        /// <param name="level">Level the FallingSpike should be in</param>
        /// <param name="contentManager">ContentManager used to load Sprite</param>
        public FallingSpike(int x, int y, Level level, ContentManager contentManager) : base()
        {
            StartPosition = new Vector2(x * Global.Instance.GridSize, y * Global.Instance.GridSize);
            Position = StartPosition;
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
            //Checks if Mario is beneath the spike
            CheckFall(m);

            if (_goFall)
            {
                //Update hitbox to match current position
                UpdateHitbox();

                //Add gravity to vertical velocity
                AddGravity();

                //Update position
                UpdatePosition();

                //Check if Mario gets hit
                CheckHit(m);
            }
        }

        /// <summary>
        /// Updates FallingSpike's Hitbox to match the current Position
        /// </summary>
        private void UpdateHitbox()
        {
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Hitbox.Width, Hitbox.Height);
        }

        /// <summary>
        /// Adds Gravity to FallingSpike's vertical velocity
        /// </summary>
        private void AddGravity()
        {
            VelocityY += Gravity;
        }


        /// <summary>
        /// Updates FallingSpike's current Position using its velocity
        /// </summary>
        private void UpdatePosition()
        {
            Position = new Vector2(Position.X + VelocityX, Position.Y + VelocityY);
        }

        /// <summary>
        /// Checks if FallingSpike hits Mario
        /// </summary>
        /// <param name="mario">Used to check if Mario hits the FallingSpike</param>
        private void CheckHit(Mario mario)
        {
            if (mario.Hitbox.Intersects(Hitbox))
            {
                mario.GetHit();
            }
        }

        /// <summary>
        /// Check if the FallingSpike should fall
        /// </summary>
        /// <param name="mario">Used to check if Mario stands under the FallingSpike</param>
        private void CheckFall(Mario mario)
        {
            if (mario.Hitbox.Intersects(FallHitbox))
            {
                _goFall = true;
            }
        }

        /// <summary>
        /// Resets the position of the FallingSpike
        /// </summary>
        public void ResetSpike()
        {
            Position = StartPosition;
            _goFall = false;
            VelocityY = 0;
        }
    }
}
