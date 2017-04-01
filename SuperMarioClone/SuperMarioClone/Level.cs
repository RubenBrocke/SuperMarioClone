using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;

namespace SuperMarioClone
{
    public class Level
    {
        public List<GameObject> GameObjects { get; private set; }
        public Texture2D Background { get; private set; }
        public int Time { get; private set; }

        private List<GameObject> _toRemove = new List<GameObject>();
        private List<GameObject> _toAdd = new List<GameObject>();
        private Timer _timerTimer;

        public Level()
        {
            Time = 280;
            GameObjects = new List<GameObject>();
            _timerTimer = new Timer(DecreaseTime);
            _timerTimer.Change(0, 1000);
        }

        private void DecreaseTime(object state)
        {
            Time -= 1;
            if (Time == 0)
            {
                //TODO: End game and remove life from Mario
            }
        }

        private void AddGameObjects()
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

        private void RemoveGameObjects()
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
                    if (t.Hitbox.Intersects(Global.Instance.MainGame.camera.GetBounds()))
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
            AddGameObjects();
            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Update();
            }
            AddGameObjects();
            RemoveGameObjects();
        }
    }
}
