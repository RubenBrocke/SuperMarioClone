using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace SuperMarioClone
{
    class CloudBlock : Solid
    {
        private ContentManager _cm;

        public CloudBlock(int x, int y, Level lvl, ContentManager cm) : base()
        {
            CurrentLevel = lvl;
            Position = new Vector2(x, y);
            _cm = cm;
            Sprite = _cm.Load<Texture2D>("CloudBlock");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture: Sprite, position: Position, sourceRectangle: new Rectangle(16, 0, 16, 16));
        }
    }
}
