using System;
using System.IO;
using System.Text;
using Matze.Algorithms;

namespace Matze.Utils
{
    class Writer
    {
        public static void BitsToConsole(BitGrid grid)
        {
            WriteBitsTo(grid, System.Console.OpenStandardOutput());
        }
        public static void ToConsole(BitGrid grid)
        {
            WriteGridTo(grid, System.Console.OpenStandardOutput());
        }
        public static void WriteGridTo(BitGrid grid, Stream stream)
        {
            Write(stream, "\n");
            for (int y = 0; y <= grid.Size(); y++)
            {
                if (y == 0)
                {
                    Write(stream, CreateLine(y, grid.Size()) + "\n");
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
                Write(stream, line + "\n");
                if (y == grid.Size())
                {
                    Write(stream, CreateLine(y, grid.Size()) + "\n");
                }
            }
        }
        public static void WriteBitsTo(BitGrid grid, Stream stream)
        {
            Write(stream, "\n");
            for (int y = 0; y <= grid.Size(); y++)
            {
                string line = "| ";
                for (int x = 0; x <= grid.Size(); x++)
                {
                    line += ((grid[y][x] < 10) ? " " : "") + grid[y][x] + " | ";
                }
                Write(stream, line + "\n");
            }
            Write(stream, "\n");
        }
        private static void Write(Stream stream, string str)
        {
            var bytes = Encoding.ASCII.GetBytes(str);
            stream.Write(bytes, 0, bytes.Length);
        }
        private static void WriteLine(Stream stream, string str)
        {
            var bytes = Encoding.ASCII.GetBytes(str + "\n");
            stream.Write(bytes, 0, bytes.Length);
        }
        private static string CreateLine(int y, int max)
        {
            var line = "";
            for (int x = 0; x <= max; x++)
            {
                line += "###";
            }
            line += "##";
            return line;
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

            if (File.Exists(file))
            {
                File.Delete(file);
            }

            using (FileStream stream = File.Create(file))
            {
                WriteBitsTo(grid, stream);
                WriteGridTo(grid, stream);
            }
        }
    }
}