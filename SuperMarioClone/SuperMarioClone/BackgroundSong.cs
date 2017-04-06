using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarioClone
{
    class BackgroundSong
    {
        public BackgroundSong(ContentManager contentManager)
        {
            MediaPlayer.Play(contentManager.Load<Song>("SMWSong"));
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.1f;
        }

        public void CheckInput()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Down))
            {
                MediaPlayer.Volume -= 0.01f;
            }
            if (state.IsKeyDown(Keys.Up))
            {
                MediaPlayer.Volume += 0.01f;
            }
        }
    }
}
