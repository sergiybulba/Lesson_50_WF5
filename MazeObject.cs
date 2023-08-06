﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson_50_WF_5_v1
{
    class MazeObject
    {
        public enum MazeObjectType { HALL, WALL, MEDAL, ENEMY, CHAR, HEALTH };

        public static Bitmap[] images = { new Bitmap(Properties.Resources.hall),
            new Bitmap(Properties.Resources.wall), // wall
            new Bitmap(Properties.Resources.medal), // medal
            new Bitmap(Properties.Resources.enemy), // enemy
            new Bitmap(Properties.Resources.player), // player
            new Bitmap(Properties.Resources.health)}; // health

        public MazeObjectType type;

        public int width;
        public int height;
        public Image texture;

        public MazeObject(MazeObjectType type)
        {
            this.type = type;
            width = 16;
            height = 16;
            texture = images[(int)type];
        }
    }
}
