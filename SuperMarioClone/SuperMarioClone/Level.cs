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

        private List<GameObject> _toRemove = new List<GameObject>();
        private List<GameObject> _toAdd = new List<GameObject>();

        public Level()
        {
            _gameObjects = new List<GameObject>();
        }

        public void AddGameObjects()
        {
            foreach (GameObject g in _toAdd)
            {
                _gameObjects.Add(g);
            }
            _toAdd = new List<GameObject>();
        }

        public void ToAddGameObject(GameObject g)
        {
            _toAdd.Add(g);
        }

        public void ToRemoveGameObject(GameObject g)
        {
            _toRemove.Add(g);
        }

        public void RemoveGameObjects()
        {
            foreach(GameObject g in _toRemove)
            {
                if (_gameObjects.Contains(g))
                {
                    _gameObjects.Remove(g);
                }
                _toRemove = new List<GameObject>();
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
            AddGameObjects();
            RemoveGameObjects();
        }
    }
}
