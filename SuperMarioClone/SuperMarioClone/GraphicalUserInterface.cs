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
        private Animator _coinAnimator;
        private Texture2D _coinSheet;

        public GraphicalUserInterface(Mario mario, ContentManager contentManager)
        {
            _mario = mario;
            _font = contentManager.Load<SpriteFont>("MarioFont");
            _coinSheet = contentManager.Load<Texture2D>("CoinSheet");
            _coinAnimator = new Animator(_coinSheet, 180);
            _coinAnimator.GetTextures(0, 0, 16, 16, 4, 1);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(texture: _coinAnimator.GetCurrentTexture(), position: new Vector2(720, 8));
            DrawBorderedText(_font, spriteBatch, String.Format("x {0, 4}", _mario.Coins), Color.Black, Color.White, new Vector2(736, 8));
            DrawBorderedText(_font, spriteBatch, String.Format("Time \n {0,3}", Global.Instance.MainGame.currentLevel.Time), Color.Black, Color.Yellow, new Vector2(375, 8));
            DrawBorderedText(_font, spriteBatch, String.Format("Lives \n x {0,4}", _mario.Lives), Color.Black, Color.White, new Vector2(16, 8));
            spriteBatch.End();
        }

        /// <summary>
        /// Draws a given string with a border around it
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch needed to draw the string</param>
        /// <param name="text">The string that needs to be drawn</param>
        /// <param name="borderColor">Color of the border</param>
        /// <param name="textColor">Color of the text itself</param>
        /// <param name="position">Position of the text on the screen</param>
        /// <param name="scale">Scale of the text, this defaults to 1</param>
        /// <param name="rotation">Rotation of the text, this defaults to 0</param>
        private void DrawBorderedText(SpriteFont font, SpriteBatch spriteBatch, string text, Color borderColor, Color textColor, Vector2 position, float borderWidth = 1, float scale = 1f, float rotation = 0f)
        {
            Vector2 origin = Vector2.Zero;
            spriteBatch.DrawString(font, text, new Vector2(position.X + borderWidth * scale, position.Y + borderWidth * scale), borderColor, rotation, origin, scale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, text, new Vector2(position.X + -borderWidth * scale, position.Y + -borderWidth * scale), borderColor, rotation, origin, scale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, text, new Vector2(position.X + -borderWidth * scale, position.Y + borderWidth * scale), borderColor, rotation, origin, scale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, text, new Vector2(position.X + borderWidth * scale, position.Y + -borderWidth * scale), borderColor, rotation, origin, scale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, text, position, textColor, rotation, origin, scale, SpriteEffects.None, 1f);
        }
    }
}