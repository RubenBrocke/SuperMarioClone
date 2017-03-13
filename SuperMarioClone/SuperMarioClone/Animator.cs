using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioClone
{
    class Animator
    {
        private Texture2D _inputTexture;

        public Animator(Texture2D inputTexture)
        {
            _inputTexture = inputTexture;
        }

        public void SetTexture(Texture2D inputTexture)
        {
            _inputTexture = inputTexture;
        }

        public Texture2D[] GetTextures(int x, int y, int width, int height, int collumnAmount, int rowAmount)
        {
            Texture2D[] returnArray = new Texture2D[collumnAmount * rowAmount];
            Rectangle sourceRect = Rectangle.Empty;
            Color[] data = new Color[width * height];
            for (int i = 0; i < rowAmount; i++)
            {
                for (int j = 0; j < collumnAmount; j++)
                {
                    sourceRect = new Rectangle(x * j, y * i, width, height);
                    _inputTexture.GetData(0, sourceRect, data, 0, data.Length);
                    returnArray[i * j].SetData(data);
                }
            }
            return returnArray;
        }
    }
}
