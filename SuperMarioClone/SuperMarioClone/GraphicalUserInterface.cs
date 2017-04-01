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
    public class GraphicalUserInterface
    {
        private Mario _mario;
        private SpriteFont _font;

        public GraphicalUserInterface(Mario mario, ContentManager _contentManager)
        {
            _mario = mario;
            _font = _contentManager.Load<SpriteFont>("MarioFont");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(_font, String.Format("Coins: {0,4}", _mario.Coins), new Vector2(704, 0), Color.Yellow);
            spriteBatch.DrawString(_font, String.Format("Lives: {0,4}", _mario.Lives), new Vector2(558, 0), Color.Yellow);
            spriteBatch.DrawString(_font, String.Format("Time \n {0,3}", Global.Instance.MainGame.currentLevel.Time), new Vector2(400, 0), Color.Yellow);
            spriteBatch.DrawString(_font, String.Format("Lives: {0,4}", _mario.Lives), new Vector2(558, 0), Color.Yellow);
            spriteBatch.End();
        }


    }
}
