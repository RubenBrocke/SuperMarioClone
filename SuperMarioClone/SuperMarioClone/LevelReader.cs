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
        private ContentManager cm;

        public LevelReader(ContentManager _cm)
        {
            cm = _cm;
        }

        public Level ReadLevel(int levelNumber)
        {
            StreamReader lvlReader = new StreamReader(@"..\..\..\..\Level" + levelNumber + ".txt");
            string line;
            Level level = new Level();
            int x, y, width, height; //elk een eigen regel


            while((line = lvlReader.ReadLine()) != null)
            {
                //Split line
                string[] arguments = line.Split(',');
                arguments[0] = arguments[0].Split(':')[1];

                //Check for type
                //Set width, height and position of items
                if (line.Substring(0, 6).Equals("Floor:"))
                {
                    try
                    {
                        x = Int32.Parse(arguments[0]) * 16;
                        y = Int32.Parse(arguments[1]) * 16;
                        width = Int32.Parse(arguments[2]) * 16;
                        height = Int32.Parse(arguments[3]) * 16;
                        level.AddGameObject(new Floor(x, y, width, height, level, cm));
                    }
                    catch
                    {
                        new FormatException("Unable to parse number in: level" + levelNumber + " File");
                    }
                }
                if (line.Substring(0, 5).Equals("Coin:"))
                {
                    try
                    {
                        x = Int32.Parse(arguments[0]) * 16;
                        y = Int32.Parse(arguments[1]) * 16;
                        level.AddGameObject(new Coin(x, y, level, cm));
                    }
                    catch
                    {
                        new FormatException("Unable to parse number in: level" + levelNumber + " File");
                    }
                }
                if (line.Substring(0, 8).Equals("Mystery:"))
                {
                    Type containObject;
                    try
                    {
                        x = Int32.Parse(arguments[0]) * 16;
                        y = Int32.Parse(arguments[1]) * 16;
                        containObject = Type.GetType("SuperMarioClone." + arguments[2]);
                        level.AddGameObject(new MysteryBlock(x, y, level, cm, containObject));
                    }
                    catch
                    {
                        new FormatException("Unable to parse number in: level" + levelNumber + " File");
                    }
                }

                }
            level.AddGameObject(new Mario(10, 10, level, cm));
            lvlReader.Close();
            return level;
        }

    } 
}

