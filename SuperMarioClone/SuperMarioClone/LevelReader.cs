﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioClone
{
    public class LevelReader
    {
        public Level ReadLevel(int levelNumber)
        {
            //StreamReader lvlReader = new StreamReader("Level" + levelNumber);
            string line;
            Level level = new Level();
            int x, y, width, height;


            /*while((line = lvlReader.ReadLine()) != null)
            {
                //Split line
                string[] arguments = line.Split(',');

                //Check for type
                if (line.Substring(0, 6).Equals("Floor:"))
                {
                    try
                    {
                        x = Int32.Parse(arguments[1]);
                        y = Int32.Parse(arguments[2]);
                        width = Int32.Parse(arguments[3]);
                        height = Int32.Parse(arguments[4]);
                        level.AddGameObject(new Floor(x, y, width, height));
                    }
                    catch
                    {
                        new FormatException("Unable to parse number in: level" + levelNumber + " File");
                    }                
                }
            }*/

            level.AddGameObject(new Mario(10, 10));

            return level;
        }

    } 
}

