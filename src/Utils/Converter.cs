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
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

using Newtonsoft.Json;
using Matze.Algorithms;
using Matze.Grids;

namespace Matze.Utils
{
    public class Converter
    {
        ////////////////////////////////////////////////////////////////////////////////////
        ////////// BitGrid To Converters                                          //////////
        ////////////////////////////////////////////////////////////////////////////////////
        static public byte[][] ToBytes(BitGrid bitgrid)
        {
            var grid = bitgrid.Grid;
            List<byte[]> bytes = new List<byte[]>();
            foreach (var row in grid)
            {
                bytes.Add(row.SelectMany(i => BitConverter.GetBytes(i)).ToArray());
            }
            return bytes.ToArray();
        }

        public static string ToString(BitGrid bitgrid, bool newLine = true)
        {
            var grid = bitgrid.Grid;
            string str = "";
            foreach (var row in grid)
            {
                str += String.Join(';', row);
                str += (newLine) ? "\n" : "|";
            }
            return str;
        }

        public static string ToCSV(BitGrid bitgrid)
        {
            return ToString(bitgrid);
        }

        public static string ToJSON(BitGrid grid)
        {
            return JsonConvert.SerializeObject(grid.Grid);
        }

        public static string ToYAML(BitGrid grid)
        {
            string export = "---\n";
            foreach (var row in grid.Grid)
            {
                export += "- ";
                var newRow = true;
                foreach (var cell in row)
                {
                    export += ((newRow) ? "" : "\t") + "- " + cell + "\n";
                    newRow = false;
                }
            }
            return export;
        }

        ////////////////////////////////////////////////////////////////////////////////////
        ////////// CellGrid To Converters                                         //////////
        ////////////////////////////////////////////////////////////////////////////////////

        public static string ToString(CellGrid grid, bool newline = true)
        {
            return ToString(GridConverter.ToBitGrid(grid), newline);
        }
        public static string ToCSV(CellGrid grid)
        {
            return ToCSV(GridConverter.ToBitGrid(grid));
        }
        public static string ToJSON(CellGrid grid)
        {
            return ToJSON(GridConverter.ToBitGrid(grid));
        }
        public static string ToYAML(CellGrid grid)
        {
            return ToYAML(GridConverter.ToBitGrid(grid));
        }
        public static byte[][] ToBytes(CellGrid grid)
        {
            return ToBytes(GridConverter.ToBitGrid(grid));
        }
        ////////////////////////////////////////////////////////////////////////////////////
        ////////// None Parsebale Converters                                      //////////
        ////////////////////////////////////////////////////////////////////////////////////

        public static string ToASCII(BitGrid grid)
        {

            string output = "\n";
            var width = grid.Width;
            var height = grid.Height;

            for (int y = 0; y < width; y++)
            {
                if (y == 0)
                {
                    output += CreateLine(y, width) + "\n";
                }
                string line = "#";
                for (int x = 0; x < width; x++)
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
                    if (x < width)
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
                output += line + "\n";
                if (y == height)
                {
                    output += CreateLine(y, height) + "\n";
                }
            }
            return output;
        }

        public static string ToBits(BitGrid grid)
        {
            int width = grid.Width;
            int height = grid.Height;
            string output = "\n";
            for (int y = 0; y < height; y++)
            {
                string line = "| ";
                for (int x = 0; x < width; x++)
                {
                    line += ((grid[y][x] < 10) ? " " : "") + grid[y][x] + " | ";
                }
                output += line + "\n";
            }
            output += '\n';
            return output;
        }

        public static string ToASCII(CellGrid grid)
        {
            return ToASCII(GridConverter.ToBitGrid(grid));
        }
        public static string ToBits(CellGrid grid)
        {
            return ToBits(GridConverter.ToBitGrid(grid));
        }
        ////////////////////////////////////////////////////////////////////////////////////
        ////////// generic Parsers Converters                                     //////////
        ////////////////////////////////////////////////////////////////////////////////////

        public static GridType ParseJSON<GridType>(string json) where GridType : IGrid
        {
            List<List<int>> result = JsonConvert.DeserializeObject<List<List<int>>>(json);
            foreach (var row in result)
            {
                if (!ValidateValues(row)) throw new Exception("Error: The given value could not have been generated with the MazeGenerator");
            }
            return (GridType)Activator.CreateInstance(typeof(GridType), new object[] { result });
        }
        public static GridType ParseBytes<GridType>(byte[][] bytes) where GridType : IGrid
        {
            var grid = new List<List<int>>();
            foreach (var row in bytes)
            {
                grid.Add(Enumerable.Range(0, row.Length / 4)
                        .Select(i => BitConverter.ToInt32(row, i * 4))
                        .ToList());
                if (!ValidateValues(grid.Last())) throw new Exception("Error: The given value could not have been generated with the MazeGenerator");

            }
            return (GridType)Activator.CreateInstance(typeof(GridType), new object[] { grid });
        }
        public static GridType ParseString<GridType>(string source) where GridType : IGrid
        {
            var grid = new List<List<int>>();
            var rows = source.Split('\n');

            if (rows.Length == 0)
            {
                rows = source.Split('|');
            }

            foreach (var row in rows)
            {
                if (row == "") continue;
                var cells = row.Split(';');
                grid.Add(new List<int>());
                foreach (var cell in cells)
                {
                    try
                    {
                        var bit = Int32.Parse(cell);
                        if (ValidateValue(bit))
                        {
                            grid.Last().Add(bit);
                        }
                        else
                        {
                            throw new Exception("Error: String cannot e converted to cell because its not a correct numeric value the System could have generated!");
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Error: String cannot e converted to cell because its not a numeric value\n Message:" + e.Message);
                    }
                }
            }
            return (GridType)Activator.CreateInstance(typeof(GridType), new object[] { grid });
        }
        public static GridType ParseYAML<GridType>(string yaml) where GridType : IGrid
        {
            yaml = yaml.Replace(" ", "");
            yaml = yaml.Replace("\t", "");
            var chars = yaml.ToCharArray();
            if (chars[0] == '-' && chars[1] == '-' && chars[2] == '-')
            {
                string line = "";
                List<List<int>> grid = new List<List<int>>();
                for (var idx = 3; idx < chars.Length; idx++)
                {
                    if (chars[idx] == '-' && chars[idx + 1] == '-')
                    {
                        //new row
                        grid.Add(new List<int>());
                        line = "";
                        line += chars[idx + 2];
                        idx += 2;
                    }
                    else if (chars[idx] == '-')
                    {
                        line += chars[idx + 1];
                        idx += 1;
                    }
                    else
                    {
                        line += chars[idx];
                        if (idx + 1 < chars.Length && chars[idx + 1] == '-')
                        {
                            var value = Int32.Parse(line);
                            if (!ValidateValue(value)) throw new Exception("Error: The given value could not have been generated with the MazeGenerator");
                            grid.Last().Add(value);
                            line = "";
                        }
                        else if (idx + 1 == chars.Length)
                        {
                            var value = Int32.Parse(line);
                            if (!ValidateValue(value)) throw new Exception("Error: The given value could not have been generated with the MazeGenerator");
                            grid.Last().Add(value);
                        }
                    }
                }
                return (GridType)Activator.CreateInstance(typeof(GridType), new object[] { grid });
            }
            else
            {
                throw new Exception("Error: Yaml file is not correct.");
            }
        }

        public static GridType ParseJSONFile<GridType>(string file) where GridType : IGrid
        {
            var fileContent = Import.ReadFile(file);
            return ParseJSON<GridType>(fileContent);
        }
        public static GridType ParseCSVFile<GridType>(string file) where GridType : IGrid
        {
            var fileContent = Import.ReadFile(file);
            return ParseString<GridType>(fileContent);
        }
        public static GridType ParseBytesFile<GridType>(string file) where GridType : IGrid
        {
            var fileContent = Import.ReadBinaryFile(file);
            return ParseBytes<GridType>(fileContent);
        }
        public static GridType ParseYamlFile<GridType>(string file) where GridType : IGrid
        {
            var fileContent = Import.ReadFile(file);
            return ParseYAML<GridType>(fileContent);
        }

        ///////////////////////////////////////////////////////////////////////////////////
        ////////// Internal helpers
        ///////////////////////////////////////////////////////////////////////////////////


        private static bool ValidateValues(IEnumerable<int> grid)
        {
            foreach (var bit in grid)
            {
                return ValidateValue(bit);
            }
            return true;
        }
        private static bool ValidateValue(int bit)
        {
            if (!((bit & (int)Directions.E) != 0 || (bit & (int)Directions.N) != 0 || (bit & (int)Directions.W) != 0 || (bit & (int)Directions.S) != 0))
            {
                return false;
            }
            return true;
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
    }
}