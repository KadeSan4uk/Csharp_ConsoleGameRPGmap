﻿using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Game2Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var game = new Game(@"D:\CsharpCourse\MyGame\bin\Debug\net8.0\map1.txt");
            game.Run();
            
        }
    }
}
