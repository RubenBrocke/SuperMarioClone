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

        public Animator(Texture2D inputTexture)
        {
            _inputTexture = inputTexture;
            _imageCounter = 0;
            _imageTimer = new Timer(IncreaseImageCounter, null, 0, AnimationSpeed);
        }

        public Animator(Texture2D inputTexture, int speed)
        {
            _inputTexture = inputTexture;
            _imageCounter = 0;
            AnimationSpeed = speed;
            _imageTimer = new Timer(IncreaseImageCounter, null, 0, AnimationSpeed);
        }

        public void SetInputTexture(Texture2D inputTexture)
        {
            _inputTexture = inputTexture;
        }

        public void GetTextures(int x, int y, int width, int height, int collumnAmount, int rowAmount)
        {
            Texture2D[] returnArray = new Texture2D[collumnAmount * rowAmount];
            Texture2D Part = new Texture2D(_inputTexture.GraphicsDevice, width, height);            
            Rectangle sourceRect = Rectangle.Empty;
            Color[] data = new Color[width * height];
            for (int i = 0; i < rowAmount; i++)
            {
                for (int j = 0; j < collumnAmount; j++)
                {
                    Part = new Texture2D(_inputTexture.GraphicsDevice, width, height);
                    sourceRect = new Rectangle(j * width + x, i * height + y, width, height);
                    _inputTexture.GetData(0, sourceRect, data, 0, data.Length);
                    Part.SetData(data);
                    Part.Tag = i + j;
                    returnArray[i + j] = Part;
                }
            }
            TextureArray = returnArray;
        }

        public Texture2D GetCurrentTexture()
        {
            return TextureArray[_imageCounter];
        }

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

        public void setAnimationSpeed(int speed)
        {
            AnimationSpeed = speed;
            _imageTimer.Change(0, AnimationSpeed);
        }
    }
}
