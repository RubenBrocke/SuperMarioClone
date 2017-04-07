using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SuperMarioClone
{
    public class LevelReader
    {
        //Private fields
        private ContentManager _contentManager;
        private GameObjectFactory _gameObjectFactory;

        /// <summary>
        /// Constructor of LevelReader, creates a new GameObjectFactory
        /// </summary>
        /// <param name="contentManager">Used to create objects</param>
        public LevelReader(ContentManager contentManager)
        {
            //Private fields are set
            _contentManager = contentManager;
            _gameObjectFactory = new GameObjectFactory();
        }

        /// <summary>
        /// Reads the Level specified by the levelNumber
        /// </summary>
        /// <param name="levelNumber">Specifies which Level to read</param>
        /// <returns></returns>
        public Level ReadLevel(int levelNumber)
        {            
            StreamReader lvlReader = new StreamReader(@"Level" + levelNumber + ".txt");
            string line;
            Level level = null;

            line = lvlReader.ReadLine();
            if (line.Contains("Args:") && !line.Contains("//"))
            {
                string[] args;
                line = line.Replace(" ", "");
                args = line.Split(':')[1].Split(',');
                int width = int.Parse(args[0]) * Global.Instance.GridSize; 
                int height = int.Parse(args[1]) * Global.Instance.GridSize;
                int timerLength = int.Parse(args[2]);
                level = new Level(width, height, timerLength);
            }
            while ((line = lvlReader.ReadLine()) != null)
            {
                if (!string.IsNullOrWhiteSpace(line) && line.Contains(":") && line.Contains(",") && !line.Contains("//"))
                {
                    line = line.Replace(" ", "");
                    try
                    {
                        level.ToAddGameObject(_gameObjectFactory.Fabricate(line.Split(':'), level, _contentManager));
                    }
                    catch (Exception)
                    {
                        throw new FileLoadException("No size detected");
                    }
                }
                
            }
            lvlReader.Close();
            return level;
        }

    } 
}

