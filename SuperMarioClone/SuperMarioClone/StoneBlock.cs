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
    public class StoneBlock : Tangible
    {
        /// <summary>
        ///  Constructor for StoneBlock, sets the position of the StoneBlock using the GridSize and sets its Sprite
        /// </summary>
        /// <param name="x">X position of the StoneBlock</param>
        /// <param name="y">Y position of the StoneBlock</param>
        /// <param name="level">Level the StoneBlock should be in</param>
        /// <param name="contentManager">ContentManager used to load Sprite</param>
        public StoneBlock(int x, int y, Level level, ContentManager contentManager) : base()
        {
            Position = new Vector2(x * Global.Instance.GridSize, y * Global.Instance.GridSize);
            CurrentLevel = level;
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Global.Instance.GridSize, Global.Instance.GridSize);
            IsSolid = true;
            Sprite = contentManager.Load<Texture2D>("StoneBlock");
        }
    }
}
