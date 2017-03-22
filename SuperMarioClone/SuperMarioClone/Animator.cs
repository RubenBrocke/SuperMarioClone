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
    class Animator
    {

        private Texture2D _inputTexture { get; set; }

        private Texture2D[] TextureArray { get; set; }

        private int AnimationSpeed { get; set; }

        private int _imageCounter;

        private Timer _imageTimer;

        /// <summary>
        /// Constructor
        /// sets the input texture and the timer
        /// </summary>
        /// <param name="inputTexture">>The texture to get the animation from, usualy a spritesheet</param>
        public Animator(Texture2D inputTexture)
        {
            _inputTexture = inputTexture;
            _imageCounter = 0;
            _imageTimer = new Timer(IncreaseImageCounter, null, 0, AnimationSpeed);
        }

        /// <summary>
        /// Another constructor
        /// Sets the input texture, the timer and the speed of the animation
        /// </summary>
        /// <param name="inputTexture">The texture to get the animation from, usualy a spritesheet</param>
        /// <param name="speed"></param>
        public Animator(Texture2D inputTexture, int speed)
        {
            _inputTexture = inputTexture;
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
            _inputTexture = inputTexture;
        }

        /// <summary>
        /// GetTextures uses an offset width / height and collumns / rows to determine how to split the inputtexture
        /// It then splits the image and stores it in a texture array
        /// </summary>
        /// <param name="x">The x coordinate</param>
        /// <param name="y">The y coordinate</param>
        /// <param name="width">The width of the sprite</param>
        /// <param name="height">The height of the sprite</param>
        /// <param name="collumnAmount">The amount of sprites an animation has lengthwise</param>
        /// <param name="rowAmount">The amount of sprites an animation has heightwise</param>
        public void GetTextures(int x, int y, int width, int height, int collumnAmount, int rowAmount)
        {
            Texture2D[] returnArray = new Texture2D[collumnAmount * rowAmount];
            Texture2D part = new Texture2D(_inputTexture.GraphicsDevice, width, height);            
            Rectangle sourceRect = Rectangle.Empty;
            Color[] data = new Color[width * height];
            for (int i = 0; i < rowAmount; i++)
            {
                for (int j = 0; j < collumnAmount; j++)
                {
                    part = new Texture2D(_inputTexture.GraphicsDevice, width, height);
                    sourceRect = new Rectangle(j * width + x, i * height + y, width, height);
                    _inputTexture.GetData(0, sourceRect, data, 0, data.Length);
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
        /// GetCurrentTexture returs the texture that is currently used
        /// </summary>
        /// <returns></returns>
        public Texture2D GetCurrentTexture()
        {
            return TextureArray[_imageCounter];
        }

        /// <summary>
        /// Increases the imagecounter, if the counter exceeds the number of images it resets, thus cycling through the images
        /// </summary>
        /// <param name="state"></param>
        private void IncreaseImageCounter(object state)
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
        /// <summary>
        /// Sets the speed of the animation
        /// </summary>
        /// <param name="speed"></param>
        public void SetAnimationSpeed(int speed)
        {
            if (speed == 0)
            {
                AnimationSpeed = speed;
                _imageTimer.Change(Timeout.Infinite, Timeout.Infinite);
                _imageCounter = 0;
            }
            else if(AnimationSpeed != speed)
            {
                AnimationSpeed = speed;
                _imageTimer.Change(AnimationSpeed, AnimationSpeed);
                _imageCounter = 0;
            }
        }
    }
}
