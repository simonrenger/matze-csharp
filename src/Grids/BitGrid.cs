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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matze.Algorithms;
using Matze.Utils;

namespace Matze.Grids
{
    public class BitGrid
    {
        private List<List<int>> grid;
        public BitGrid(int width, int height)
        {
            grid = new List<List<int>>();
            for (int i = 0; i < height; i++)
            {
                grid.Add(new List<int>());
                for (int j = 0; j < width; j++)
                {
                    grid[i].Add(0);
                }
            }
        }

        internal BitGrid(List<List<int>> grid)
        {
            this.grid = grid;
        }
        public List<int> this[int index]
        {
            get { return grid[index]; }
        }

        public int Width
        {
            get
            {
                return grid[0].Count;
            }
        }

        public int Height
        {
            get { return grid.Count; }
        }

        public int Size()
        {
            return grid.Size();
        }

        internal List<List<int>> Grid => grid;

 
    }
}