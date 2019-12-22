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
using Matze.Grids;

namespace Matze.Utils
{
    public partial class Converter
    {
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
    }
}