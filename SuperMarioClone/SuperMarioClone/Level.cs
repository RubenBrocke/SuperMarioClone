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
            lock (_toAdd)
            {
                foreach (GameObject g in _toAdd.ToList())
                {
                    _gameObjects.Add(g);
                }
            }
            _toAdd.Clear();
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
            lock (_toRemove)
            {
                foreach (GameObject g in _toRemove)
                {
                    if (_gameObjects.Contains(g))
                    {
                        _gameObjects.Remove(g);
                    }
                }
            }
            _toRemove.Clear();
        }
        
        public void SetBackground(Texture2D background)
        {
            _background = background;
        }

        public void DrawLevel(SpriteBatch spriteBatch, Viewport viewPort)
        {
            foreach (GameObject o in _gameObjects)
            {
                if (o is Tangible)
                {
                    Tangible t = (Tangible)o;
                    if (t.hitbox.Intersects(viewPort.Bounds))
                    {
                        o.Draw(spriteBatch);
                    }
                    
                }
                else
                {
                    o.Draw(spriteBatch);
                }
               
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
