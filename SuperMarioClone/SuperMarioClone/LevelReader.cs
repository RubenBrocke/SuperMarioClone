﻿using System;
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
            int x;
            int y;
            int width;
            int height; 


            while((line = lvlReader.ReadLine()) != null)
            {
                //Split line
                line = line.Replace(" ", "");
                string[] arguments = line.Split(',');
                arguments[0] = arguments[0].Split(':')[1];

                //Check for type
                //Set width, height and position of items
                if (line.Contains("Floor:"))
                {
                    try
                    {
                        x = Int32.Parse(arguments[0]) * 16;
                        y = Int32.Parse(arguments[1]) * 16;
                        width = Int32.Parse(arguments[2]) * 16;
                        height = Int32.Parse(arguments[3]) * 16;
                        level.ToAddGameObject(new Floor(x, y, width, height, level, cm));
                    }
                    catch
                    {
                        throw new FormatException("Unable to parse number in: level" + levelNumber + " File");
                    }
                }
                if (line.Contains("TransFloor:"))
                {
                    try
                    {
                        x = Int32.Parse(arguments[0]) * 16;
                        y = Int32.Parse(arguments[1]) * 16;
                        width = Int32.Parse(arguments[2]) * 16;
                        height = Int32.Parse(arguments[3]) * 16;
                        level.ToAddGameObject(new TransFloor(x, y, width, height, level, cm));
                    }
                    catch
                    {
                        throw new FormatException("Unable to parse number in: level" + levelNumber + " File");
                    }
                }
                if (line.Contains("Coin:"))
                {
                    try
                    {
                        x = Int32.Parse(arguments[0]) * 16;
                        y = Int32.Parse(arguments[1]) * 16;
                        level.ToAddGameObject(new Coin(x, y, false, level, cm));
                    }
                    catch
                    {
                        throw new FormatException("Unable to parse number in: level" + levelNumber + " File");
                    }
                }
                if (line.Contains("Mystery:"))
                {
                    Type containObject;
                    try
                    {
                        x = Int32.Parse(arguments[0]) * 16;
                        y = Int32.Parse(arguments[1]) * 16;
                        containObject = Type.GetType("SuperMarioClone." + arguments[2]);
                        level.ToAddGameObject(new MysteryBlock(x, y, level, cm, containObject));
                    }
                    catch
                    {
                        throw new FormatException("Unable to parse number in: level" + levelNumber + " File");
                    }
                }
                if (line.Contains("Goomba:"))
                {
                    try
                    {
                        x = Int32.Parse(arguments[0]) * 16;
                        y = Int32.Parse(arguments[1]) * 16;
                        level.ToAddGameObject(new Goomba(x, y, level, cm));
                    }
                    catch
                    {
                        throw new FormatException("Unable to parse number in: level" + levelNumber + " File");
                    }
                }
            }
            level.ToAddGameObject(new Mario(10, 10, level, cm));
            level.AddGameObjects();
            lvlReader.Close();
            return level;
        }

    } 
}

