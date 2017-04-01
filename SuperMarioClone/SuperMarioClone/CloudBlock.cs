using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperMarioClone
{
    public class CloudBlock : Tangible, ISolid
    {
        public CloudBlock(int x, int y, Level level, ContentManager contentManager) : base()
        {
            Position = new Vector2(x * Global.Instance.GridSize, y * Global.Instance.GridSize);
            CurrentLevel = level;
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Global.Instance.GridSize, Global.Instance.GridSize);
            Sprite = contentManager.Load<Texture2D>("CloudBlock");
        }
    }
}
