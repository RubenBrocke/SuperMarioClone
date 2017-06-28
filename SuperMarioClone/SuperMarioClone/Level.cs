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
        //Properties
        public List<GameObject> GameObjects { get; private set; }
        public int Time { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        //Private fields
        private List<GameObject> _toRemove = new List<GameObject>();
        private List<GameObject> _toAdd = new List<GameObject>();
        private Timer _timerTimer;

        /// <summary>
        /// Constructor for Level, Timer is set to a default of 300 seconds and Width and Height are set to a default of 999 tiles
        /// </summary>
        public Level()
        {
            //Properties and private fields are set
            GameObjects = new List<GameObject>();
            Time = 300;
            Width = 999;
            Height = 999;

            _timerTimer = new Timer(DecreaseTime);
            _timerTimer.Change(0, 1000);
        }

        /// <summary>
        /// Constructor for Level, used to set the Timer length and Level Width and Height
        /// </summary>
        /// <param name="width">Amount of tiles the Level is in width</param>
        /// <param name="height">Amount of tiles the Level is in height</param>
        /// <param name="timerLength">Length of the Level timer in seconds</param>
        public Level(int width, int height, int timerLength)
        {
            //Properties and private fields are set
            GameObjects = new List<GameObject>();
            Time = timerLength;
            Width = width - 200;
            Height = height;

            _timerTimer = new Timer(DecreaseTime);
            if (timerLength == 1)
            {
                _timerTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }
            else
            {
                _timerTimer.Change(0, 1000);
            }


        }

        /// <summary>
        /// Disables the level timer
        /// </summary>
        public void DisableTimer()
        {
            _timerTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Decreases the timer by one
        /// </summary>
        /// <param name="state"></param>
        private void DecreaseTime(object state)
        {
            if (Time > 0)
            {
                Time -= 1;
            }
            else
            {
                Global.Instance.MainGame.mario.Die();
                DisableTimer();
            }
        }

        /// <summary>
        /// Adds GameObjects to the GameObjects List that are stored in the _toAdd List
        /// </summary>
        private void AddGameObjects()
        {
            lock (_toAdd)
            {
                foreach (GameObject gameObject in _toAdd.ToList())
                {
                    GameObjects.Add(gameObject);
                }
            }
            _toAdd.Clear();
        }

        /// <summary>
        /// Sets a GameObject to be added after the update cycle has ended
        /// </summary>
        /// <param name="gameObject">GameObject that has to be added to the GameObjects list</param>
        public void ToAddGameObject(GameObject gameObject)
        {
            _toAdd.Add(gameObject);
        }

        /// <summary>
        /// Sets a GameObject to be removed after the update cycle has ended
        /// </summary>
        /// <param name="gameObject">GameObject that has to be removed from the GameObjects list</param>
        public void ToRemoveGameObject(GameObject gameObject)
        {
            _toRemove.Add(gameObject);
        }

        /// <summary>
        /// Removes GameObjects from the GameObjects List that are stored in the _toRemove List
        /// </summary>
        private void RemoveGameObjects()
        {
            foreach (GameObject gameObject in _toRemove)
            {
                if (GameObjects.Contains(gameObject))
                {
                    GameObjects.Remove(gameObject);
                }
            }
            _toRemove.Clear();
        }

        /// <summary>
        /// Calls the Draw function of each of the GameObjects in the Level
        /// </summary>
        /// <param name="spriteBatch">Used to Draw all the sprites</param>
        public void DrawLevel(SpriteBatch spriteBatch)
        {
            foreach (GameObject gameObject in GameObjects)
            {
                if (gameObject is Tangible) //Makes sure only objects that are (partly) on-screen are drawed to improve framerate
                {
                    Tangible tangible = (Tangible)gameObject;
                    if (tangible.Hitbox.Intersects(Global.Instance.MainGame.camera.GetBounds()))
                    {
                        gameObject.Draw(spriteBatch);
                    }
                }
                else
                {
                    gameObject.Draw(spriteBatch);

                    FallingSpike fallingSpike = (FallingSpike)gameObject;
                    if (gameObject is FallingSpike)
                    {
                        Rectangle rect = new Rectangle((int)fallingSpike.StartPosition.X, (int)fallingSpike.StartPosition.Y, 32, 32);
                        if (!rect.Intersects(Global.Instance.MainGame.camera.GetBounds()))
                        {
                            fallingSpike.ResetSpike();
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Calls the Update function of each of the GameObjects in the Level
        /// </summary>
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
