﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matze.Algorithms;
using Matze.Utils;


namespace Matze
{
    class Program
    {
        static void Main(string[] args)
        {
            MazeGenerator mazeGenerator = new MazeGenerator(42);
            mazeGenerator.Add<Algorithms.RecursiveBacktracking>();
            mazeGenerator.Add<Algorithms.KruskalRandomized>();
            //var grid = mazeGenerator.Run<Algorithms.RecursiveBacktracking,BitGrid>(100,100);
            BitGrid gird = mazeGenerator.Run<Algorithms.KruskalRandomized,BitGrid>(10, 10);
            gird.Print();
            Console.ReadKey();
        }
    }
}