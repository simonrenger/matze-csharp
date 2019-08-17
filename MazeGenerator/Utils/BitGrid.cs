using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matze.Algorithms;

namespace Matze.Utils
{
    class BitGrid
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
    }
}
