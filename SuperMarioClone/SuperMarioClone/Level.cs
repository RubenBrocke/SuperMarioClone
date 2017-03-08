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

        private List<GameObject> toRemove = new List<GameObject>();

        public Level()
        {
            _gameObjects = new List<GameObject>();
        }

        public void AddGameObject(GameObject g)
        {
            _gameObjects.Add(g);
        }

        public void ToRemoveGameObject(GameObject g)
        {
            toRemove.Add(g);
        }

        public void RemoveGameObjects()
        {
            foreach(GameObject o in toRemove)
            {
                if (_gameObjects.Contains(o))
                {
                    _gameObjects.Remove(o);
                }
                toRemove = new List<GameObject>();
            }
        }
        
        public void SetBackground(Texture2D background)
        {
            _background = background;
        }

        public void DrawLevel(SpriteBatch spriteBatch)
        {
            foreach (GameObject Object in _gameObjects)
            {
                Object.Draw(spriteBatch);
            }
        }

        public void UpdateLevel()
        {
            foreach (GameObject Object in _gameObjects)
            {
                Object.Update();
            }
            RemoveGameObjects();
        }
    }
}
