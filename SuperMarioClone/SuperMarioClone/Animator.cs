using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioClone
{
    public class Animator
    {
        private Texture2D InputTexture { get; set; }
        private Texture2D[] TextureArray { get; set; }
        private int AnimationSpeed { get; set; }
        private bool Paused { get; set; }

        private int _imageCounter;
        private Timer _imageTimer;

        /// <summary>
        /// Sets input texture and defaults the animationspeed to 0 
        /// </summary>
        /// <param name="inputTexture">Spritesheet used to get the animation from</param>
        public Animator(Texture2D inputTexture)
        {
            //Properties and private fields are set
            InputTexture = inputTexture;
            Paused = true;
            _imageCounter = 0;
            _imageTimer = new Timer(IncreaseImageCounter, null, 0, AnimationSpeed);
        }

        /// <summary>
        /// Sets the input texture and the speed of the animation
        /// </summary>
        /// <param name="inputTexture">Spritesheet used to get the animation from</param>
        /// <param name="speed">Amount of milliseconds each frame of the animation should be</param>
        public Animator(Texture2D inputTexture, int speed)
        {
            //Properties and private fields are set
            InputTexture = inputTexture;
            Paused = false;
            _imageCounter = 0;
            AnimationSpeed = speed;
            _imageTimer = new Timer(IncreaseImageCounter, null, 0, AnimationSpeed);
        }

        /// <summary>
        /// Function to manually set the input texture
        /// </summary>
        /// <param name="inputTexture">>The texture to get the animation from, usualy a spritesheet</param>
        public void SetInputTexture(Texture2D inputTexture)
        {
            InputTexture = inputTexture;
        }

        /// <summary>
        /// Gets the animation from the input texture and stores it in a texture array to be retreived by GetCurrentTexture()
        /// </summary>
        /// <param name="x">X coordinate of where the animation should start</param>
        /// <param name="y">Y coordinate of where the animation should start</param>
        /// <param name="width">Width of the sprite</param>
        /// <param name="height">Height of the sprite</param>
        /// <param name="collumnAmount">Amount of collumns the animation has</param>
        /// <param name="rowAmount">Amount of rows the animation has</param>
        public void GetTextures(int x, int y, int width, int height, int collumnAmount, int rowAmount)
        {
            Texture2D[] returnArray = new Texture2D[collumnAmount * rowAmount];
            Texture2D part = new Texture2D(InputTexture.GraphicsDevice, width, height);            
            Rectangle sourceRect = Rectangle.Empty;
            Color[] data = new Color[width * height];
            for (int i = 0; i < rowAmount; i++)
            {
                for (int j = 0; j < collumnAmount; j++)
                {
                    part = new Texture2D(InputTexture.GraphicsDevice, width, height);
                    sourceRect = new Rectangle(j * width + x, i * height + y, width, height);
                    InputTexture.GetData(0, sourceRect, data, 0, data.Length);
                    part.SetData(data);
                    part.Tag = i + j;
                    returnArray[i + j] = part;
                }
            }
            TextureArray = returnArray;
            if (collumnAmount * rowAmount < _imageCounter)
            {
                _imageCounter = 0;
            }
        }

        /// <summary>
        /// Returns the current texture in the animation sequence
        /// </summary>
        /// <returns>Current texture in the animation sequence</returns>
        public Texture2D GetCurrentTexture()
        {
            return TextureArray[_imageCounter];
        }

        private void IncreaseImageCounter(object state)
        {
            if (!Paused)
            {
                if (TextureArray != null)
                {
                    if (_imageCounter < TextureArray.Length - 1)
                    {
                        _imageCounter++;
                    }
                    else
                    {
                        _imageCounter = 0;
                    }
                } 
            }
        }
        /// <summary>
        /// Sets the speed of the animation
        /// </summary>
        /// <param name="speed">Amount of milliseconds each frame of the animation should be</param>
        public void SetAnimationSpeed(int speed)
        {
            if (speed == 0)
            {
                Paused = true;
                AnimationSpeed = speed;
                _imageTimer.Change(Timeout.Infinite, Timeout.Infinite);
                _imageCounter = 0;
            }
            else if(AnimationSpeed != speed)
            {
                Paused = false;
                AnimationSpeed = speed;
                _imageTimer.Change(AnimationSpeed, AnimationSpeed);
                _imageCounter = 0;
            }
        }

        /// <summary>
        /// Pauses animation to current frame
        /// </summary>
        public void PauseAnimation()
        {
            Paused = true;
        }
        /// <summary>
        /// Unpauses the animation
        /// </summary>
        public void UnpauseAnimation()
        {
            Paused = false;
        }
    }
}
