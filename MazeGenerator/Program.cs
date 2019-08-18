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
        static void PrintHelp()
        {
            Console.WriteLine("#############################");
            Console.WriteLine("Matze CL tool");
            Console.WriteLine("#############################\n");
            Console.WriteLine("Commands:");
            Console.WriteLine("[Algorithm] ([WIDTH=10] [HEIGHT=10] [-db|-dg])\n");
            Console.WriteLine("Algorithms:");
            Console.WriteLine("- RB: Recursive Backtracking");
            Console.WriteLine("- KR: Kruskal Randomized");
            Console.WriteLine("- E: Eller");
            Console.WriteLine("- P: Prim\n");
            Console.WriteLine("#############################\n");
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
                var displayOnScreen = false;
                var displayGrid = false;
                var displayBits = false;
                if (args.Length == 4)
                {
                    try{
                        width = Int32.Parse(args[2]);
                        height = Int32.Parse(args[3]);
                    }catch{
                        PrintHelp();
                        Console.WriteLine("Error: width or height was not an numeric value!\n");
                        return ERROR_BAD_ARGUMENTS;
                    }
                    if(width <= 0 && height <= 0){
                        PrintHelp();
                        Console.WriteLine("Error: width and height must be bigger then 10!\n");
                        return ERROR_BAD_ARGUMENTS;
                    }
                }
                else if (args.Length > 2)
                {
                    if (args.Last() != "-db" && args.Last() != "-dg")
                    {
                        PrintHelp();
                        Console.WriteLine("Error: Bad command line argument!\n");
                        return ERROR_BAD_ARGUMENTS;
                    }
                    else
                    {
                        displayOnScreen = true;
                        if(args.Last() == "-db"){
                            displayBits = true;
                        }else if(args.Last() == "-db"){
                            displayGrid = true;
                        }
                    }
                }
                var generator = args[1];
                MazeGenerator mazeGenerator = new MazeGenerator(42);
                mazeGenerator.Add<Algorithms.RecursiveBacktracking>();
                mazeGenerator.Add<Algorithms.KruskalRandomized>();
                mazeGenerator.Add<Algorithms.Eller>();
                mazeGenerator.Add<Algorithms.Prim>();

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                BitGrid grid;
                switch (generator)
                {
                    case "RB":
                        grid = mazeGenerator.Run<Algorithms.RecursiveBacktracking>(width, height);
                        break;
                    case "KR":
                        grid = mazeGenerator.Run<Algorithms.KruskalRandomized>(width, height);
                        break;
                    case "E":
                        grid = mazeGenerator.Run<Algorithms.Eller>(width, height);
                        break;
                    case "P":
                        grid = mazeGenerator.Run<Algorithms.Prim>(width, height);
                        break;
                    default:
                        PrintHelp();
                        Console.WriteLine("Error: Unknown Argument");
                        return ERROR_BAD_ARGUMENTS;
                }
                stopwatch.Stop();
                Random rand = new Random();
                var file = "grids/grid_" + (rand.Next(0, 100000).ToString());
                if(displayOnScreen && displayBits){
                    Writer.BitsToConsole(grid);
                }
                if(displayOnScreen && displayGrid){
                    Writer.ToConsole(grid);
                }
                Writer.ToDisk(grid,file);
                using (StreamWriter writer = new StreamWriter(file+"_info.txt", true))
                {
                    writer.WriteLine("\nInformation:\n");
                    writer.WriteLine("Algorithm: " + generator);
                    writer.WriteLine("Width: " + width);
                    writer.WriteLine("Height: " + height);
                    writer.WriteLine("Time: " + stopwatch.Elapsed);
                }
                Console.WriteLine("Saved Maze in: " + file);
                return ERROR_SUCCESS;
            }
        }
    }
}
