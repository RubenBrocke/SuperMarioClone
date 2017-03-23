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
    class TransFloor : Tangible, ISolid
    {
        protected int Width { get; set; }
        protected int Height { get; set; }

        public TransFloor(int x, int y, int w, int h, Level level, ContentManager contentManager) : base()
        {
            Position = new Vector2(x * Global.Instance.GridSize, y * Global.Instance.GridSize);
            Width = w * Global.Instance.GridSize;
            Height = h * Global.Instance.GridSize;
            CurrentLevel = level;
            Hitbox = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            Sprite = contentManager.Load<Texture2D>("GroundSheet");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRect = new Rectangle(16, 16, 16, 16);
            for (int x = (int)Position.X; x < Width + (int)Position.X; x += 16)
            {
                for (int y = (int)Position.Y; y < Height + (int)Position.Y; y += 16)
                {
                    if (y == (int)Position.Y)
                    {
                        if (x == (int)Position.X)
                        {
                            sourceRect = new Rectangle(0, 48, 16, 16);
                        }
                        else if (x == (int)Position.X + Width - 16)
                        {
                            sourceRect = new Rectangle(32, 48, 16, 16);
                        }
                        else
                        {
                            sourceRect = new Rectangle(16, 48, 16, 16);
                        }
                    }
                    else if (y == (int)Position.Y + Height - 16)
                    {
                        if (x == (int)Position.X)
                        {
                            sourceRect = new Rectangle(0, 80, 16, 16);
                        }
                        else if (x == (int)Position.X + Width - 16)
                        {
                            sourceRect = new Rectangle(32, 80, 16, 16);
                        }
                        else
                        {
                            sourceRect = new Rectangle(16, 80, 16, 16);
                        }
                    }
                    else if (x == (int)Position.X)
                    {
                        sourceRect = new Rectangle(0, 64, 16, 16);
                    }
                    else if (x == (int)Position.X + Width - 16)
                    {
                        sourceRect = new Rectangle(32, 64, 16, 16);
                    }
                    else
                    {
                        sourceRect = new Rectangle(16, 64, 16, 16);
                    }
                    spriteBatch.Draw(texture: Sprite, position: new Vector2(x, y), sourceRectangle: sourceRect);
                }
            }
        }
    }
}
