/**
MIT License

Copyright (c) 2019 Simon Renger

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
**/

using System;
using System.IO;
using System.Text;
using Matze.Algorithms;

namespace Matze.Utils
{
    public class Writer
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
        private static void WriteToFile(string filename, Action<Stream> action)
        {
            if (File.Exists(filename)) { File.Delete(filename); }
            using (FileStream stream = File.Create(filename))
            {
                action(stream);
            }
        }
        public static void ToDisk(BitGrid grid,string file = "grid"){
            ToDisk(grid,file);
        }
        public static void ToDisk(BitGrid grid,string file,Format format = Format.Info)
        {
            if ((format & Format.Info) != 0)
            {
                var filename = file + "_info.txt";
                WriteToFile(filename, (Stream stream) =>
                {
                    WriteBitsTo(grid, stream);
                    WriteGridTo(grid, stream);
                });
            }
            if ((format & Format.Json) != 0)
            {
                var filename = file + ".json";
                WriteToFile(filename, (Stream stream) =>
                {
                    Write(stream, Export.ToJSON(grid));
                });
            }

            if ((format & Format.Txt) != 0)
            {
                var filename = file + ".txt";
                WriteToFile(filename, (Stream stream) =>
                {
                    Write(stream, Export.ToString(grid,false));
                });
            }
            if ((format & Format.Csv) != 0)
            {
                var filename = file + ".csv";
                WriteToFile(filename, (Stream stream) =>
                {
                    Write(stream, Export.ToString(grid));
                });
            }
            if ((format & Format.Yaml) != 0)
            {
                var filename = file + ".yaml";
                WriteToFile(filename, (Stream stream) =>
                {
                    Write(stream, Export.ToYAML(grid));
                });
            }
            if ((format & Format.Binary) != 0)
            {
                var filename = file + ".bin";
                if (File.Exists(filename)) { File.Delete(filename); }
                using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
                {
                    var byteGrid = Export.ToBytes(grid);
                    writer.Write(grid.Width);
                    writer.Write(grid.Height);
                    foreach(var row in byteGrid){
                        writer.Write(row);
                    }
                }
            }
        }
    }
}