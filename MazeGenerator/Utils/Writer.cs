using System;
using System.IO;
using Matze.Algorithms;

namespace Matze.Utils
{
    class Writer
    {
        public static void BitsToConsole(BitGrid grid)
        {
            Console.Write("\n");
            for (int y = 0; y <= grid.Size(); y++)
            {
                string line = "| ";
                for (int x = 0; x <= grid.Size(); x++)
                {
                    line += ((grid[y][x] < 10) ? " " : "") + grid[y][x] + " | ";
                }
                Console.WriteLine(line);
            }
            Console.Write("\n");
        }
        public static void ToConsole(BitGrid grid)
        {
            Console.Write("\n");
            for (int y = 0; y <= grid.Size(); y++)
            {
                if (y == 0)
                {
                    DrawLine(y, grid.Size());
                }
                string line = "#";
                for (int x = 0; x <= grid.Size(); x++)
                {
                    var cell = "|_|";
                    var isEmpty = grid[y][x] == 0;
                    var hasNoWallNorth = (grid[y][x] & (int)Directions.N) != 0;
                    var hasNoWallEast = (grid[y][x] & (int)Directions.E) != 0;
                    var hasNoWallSouth = (grid[y][x] & (int)Directions.S) != 0;
                    var hasNoWallWest = (grid[y][x] & (int)Directions.W) != 0;
                    var privX = x - 1;
                    var privHasNoWallEast = ((privX >= 0) ? (grid[y][privX] & (int)Directions.E) != 0 : false);
                    //cell: |_|
                    cell = CarveWay(hasNoWallWest, 0, '.', cell);
                    if (x < grid.Size())
                    {
                        cell = CarveWay(hasNoWallEast, 2, '.', cell);
                    }
                    if (hasNoWallEast)
                    {
                        cell = CarveWay(privHasNoWallEast, 0, '.', cell);
                    }
                    cell = CarveWay(hasNoWallSouth, 1, ' ', cell);
                    line += cell;
                }
                line += "#";
                Console.WriteLine(line);
                if (y == grid.Size())
                {
                    DrawLine(y, grid.Size());
                }
            }
        }
        private static void DrawLine(int y, int max)
        {
            var line = "";
            for (int x = 0; x <= max; x++)
            {
                line += "###";
            }
            line += "##";
            Console.WriteLine(line);
        }
        private static string CarveWay(bool condition, int index, char element, string cell)
        {
            if (condition)
            {
                cell = cell.ReplaceAt(index, element);
            }
            return cell;
        }
        public static void ToDisk(BitGrid grid, string file = "grid.txt")
        {
            using (var writer = new StreamWriter(file))
            {
                writer.WriteLine("\n");
                for (int y = 0; y <= grid.Size(); y++)
                {
                    string line = "| ";
                    for (int x = 0; x <= grid.Size(); x++)
                    {
                        line += ((grid[y][x] < 10) ? " " : "") + grid[y][x] + " | ";
                    }
                    writer.WriteLine(line);
                }
                writer.WriteLine("\n");
            }
        }
    }
}