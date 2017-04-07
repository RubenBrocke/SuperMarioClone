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
    public class Sound
    {
        //Private fields
        private KeyboardState _lastKeyboardState;
    
        /// <summary>
        /// Constructor for Sound, starts playing the song
        /// </summary>
        /// <param name="contentManager">Used to load the song</param>
        public Sound(ContentManager contentManager)
        {
            MediaPlayer.Play(contentManager.Load<Song>("SMWSong"));
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.1f;
        }

        /// <summary>
        /// Checks for input to adjust the volume and switch between normal and weird sound
        /// </summary>
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
            if (state.IsKeyDown(Keys.Home) && !_lastKeyboardState.IsKeyDown(Keys.Home))
            {
                if (Global.Instance.WeirdSounds)
                {
                    Global.Instance.WeirdSounds = false;
                }
                else
                {
                    Global.Instance.WeirdSounds = true;
                }
            }
            _lastKeyboardState = state;
        }

        /// <summary>
        /// Resumes the music
        /// </summary>
        /// <param name="state"></param>
        public void ResumeMusic(object state)
        {
            MediaPlayer.Resume();
        }
    }
}
