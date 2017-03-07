using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioClone
{
    public class Level
    {
        public List<GameObject> _gameObjects { get; set; }

        private Texture2D _background { get; set; }

        public Level()
        {
            _gameObjects = new List<GameObject>();
        }

        public virtual void AddGameObject(GameObject g)
        {
            _gameObjects.Add(g);

        }

        public virtual void SetBackground(Texture2D background)
        {
            _background = background;
        }

        public void DrawLevel(SpriteBatch spriteBatch)
        {
            foreach (GameObject Object in _gameObjects)
            {
                /*if (Object.GetType() == typeof(Floor))
                {
                    Floor toDraw = (Floor)Object;
                    toDraw.Draw(spriteBatch);
                }
                else
                {
                    Object.Draw(spriteBatch);
                }*/
                Object.Draw(spriteBatch);
            }
        }

        public void UpdateLevel()
        {
            foreach (GameObject Object in _gameObjects)
            {
                Object.Update();
            }
        }
    }
}
