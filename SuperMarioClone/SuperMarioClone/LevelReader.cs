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
        private ContentManager _contentManager;
        private GameObjectFactory _gameObjectFactory;

        public LevelReader(ContentManager contentManager)
        {
            _contentManager = contentManager;
            _gameObjectFactory = new GameObjectFactory();
        }

        public Level ReadLevel(int levelNumber)
        {            
            StreamReader lvlReader = new StreamReader(@"..\..\..\..\Level" + levelNumber + ".txt");
            string line;
            Level level = new Level();
            int x;
            int y;
            int width;
            int height;

            while ((line = lvlReader.ReadLine()) != null)
            { 
                //Check for type
                //Set width, height and position of items
                if (!string.IsNullOrWhiteSpace(line) && line.Contains(":") && line.Contains(","))
                {
                    line = line.Replace(" ", "");
                    level.ToAddGameObject(_gameObjectFactory.Fabricate(line.Split(':'), level, _contentManager));
                }
            }
            level.ToAddGameObject(new Mario(10, 10, level, _contentManager));
            level.AddGameObjects();
            lvlReader.Close();
            return level;
        }

    } 
}

