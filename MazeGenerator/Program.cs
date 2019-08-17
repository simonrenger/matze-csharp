using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Matze.Algorithms;
using Matze.Utils;


namespace Matze
{
    class Program
    {
        private const int ERROR_SUCCESS = 0;
        private const int ERROR_BAD_ARGUMENTS = 0xA0;
        private const int ERROR_INVALID_COMMAND_LINE = 0x667;
        static void PrintHelp(){
                        Console.WriteLine("#############################");
                        Console.WriteLine("Matze CL tool");
                        Console.WriteLine("Commands:");
                        Console.WriteLine("[Algorithm] ([WIDTH=10] [HEIGHT=10])\n");
                        Console.WriteLine("RB: Recursive Backtracking");
                        Console.WriteLine("KR: Kruskal Randomized");
                        Console.WriteLine("E: Eller");
                        Console.WriteLine("P: Prim");
                        Console.WriteLine("#############################");
        }
        static int Main()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length == 1)
            {
                PrintHelp();
                return ERROR_INVALID_COMMAND_LINE;
            }
            else
            {
                var width = 10;
                var height = 10;
                if(args.Length == 4){
                    width = Int32.Parse(args[2]);
                    height = Int32.Parse(args[3]);
                }else if(args.Length > 2){
                    Console.WriteLine("Error: Bad command line argument!\n");
                    PrintHelp();
                    return ERROR_BAD_ARGUMENTS;
                }
                var generator = args[1];
                MazeGenerator mazeGenerator = new MazeGenerator();
                mazeGenerator.Add<Algorithms.RecursiveBacktracking>();
                mazeGenerator.Add<Algorithms.KruskalRandomized>();
                mazeGenerator.Add<Algorithms.Eller>();
                
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                BitGrid grid;
                switch (generator)
                {
                    case "RB":
                        grid = mazeGenerator.Run<Algorithms.RecursiveBacktracking>(width,height);
                        break;
                    case "KR":
                        grid = mazeGenerator.Run<Algorithms.KruskalRandomized>(width,height);
                        break;
                    case "E":
                        grid = mazeGenerator.Run<Algorithms.Eller>(width,height);
                        break;
                    default:
                        PrintHelp();
                        return ERROR_BAD_ARGUMENTS;
                }
                stopwatch.Stop();
                Random rand = new Random();
                var file = "grids/grid_" + (rand.Next(0, 100000).ToString() + ".txt");
                Writer.ToDisk(grid, file);
                using(StreamWriter writer = new StreamWriter(file,true)){
                    writer.WriteLine("\nInformation:\n");
                    writer.WriteLine("Algorithm: "+generator);
                    writer.WriteLine("Width: "+width);
                    writer.WriteLine("Height: "+height);
                    writer.WriteLine("Time: "+stopwatch.Elapsed);
                }
                Console.WriteLine("Saved Maze in: "+file);
                return ERROR_SUCCESS;
            }
        }
    }
}
