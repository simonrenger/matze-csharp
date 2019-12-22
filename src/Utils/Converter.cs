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
    public partial class Converter
    {
  
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