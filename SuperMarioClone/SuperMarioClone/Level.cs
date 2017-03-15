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
        public List<GameObject> GameObjects { get; set; }

        private Texture2D Background { get; set; }

        private List<GameObject> _toRemove = new List<GameObject>();
        private List<GameObject> _toAdd = new List<GameObject>();

        public Level()
        {
            GameObjects = new List<GameObject>();
        }

        public void AddGameObjects()
        {
            lock (_toAdd)
            {
                foreach (GameObject g in _toAdd.ToList())
                {
                    GameObjects.Add(g);
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
            foreach (GameObject g in _toRemove)
            {
                if (GameObjects.Contains(g))
                {
                    GameObjects.Remove(g);
                }
            }
            _toRemove.Clear();
        }
        
        public void SetBackground(Texture2D background)
        {
            Background = background;
        }

        public void DrawLevel(SpriteBatch spriteBatch, Viewport viewPort)
        {
            foreach (GameObject o in GameObjects)
            {
                if (o is Tangible)
                {
                    Tangible t = (Tangible)o;
                    if (t.Hitbox.Intersects(viewPort.Bounds))
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
            foreach (GameObject Object in GameObjects)
            {
                Object.Update();
            }
            AddGameObjects();
            RemoveGameObjects();
        }
    }
}
