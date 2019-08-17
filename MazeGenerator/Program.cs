using System;
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
            mazeGenerator.Add<Algorithms.Eller>();
            //var grid = mazeGenerator.Run<Algorithms.RecursiveBacktracking>(10,10);
            //var grid = mazeGenerator.Run<Algorithms.KruskalRandomized>(10,10);
            var grid = mazeGenerator.Run<Algorithms.Eller>(10, 10);
            Writer.ToConsole(grid);
            Writer.ToDisk(grid);
            //Console.ReadKey();
        }
    }
}
