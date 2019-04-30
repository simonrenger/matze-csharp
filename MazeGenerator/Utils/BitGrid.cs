using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matze.Algorithms;

namespace Matze.Utils
{
    class BitGrid : IGrid
    {
        private List<List<int>> grid;
        public BitGrid(int width,int height)
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

        public List<int>this[int index]
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

        public void Print()
        {
            Console.Write(" ");
            for (int i = 0; i < (grid.Size() * 2); i++)
            {
                Console.Write("_");
            }

            Console.Write("\n");
            for (int y = 0; y <= grid.Size(); y++)
            {
                string line = "|";
                for (int x = 0; x <= grid.Size(); x++)
                {
                    line +=
                        ((grid[y][x] & (int)Directions.S) != 0) ? " " : "_"
                        ;
                    if ((grid[y][x] & (int)Directions.E) != 0)
                    {
                        line +=
                            (((grid[y][x] | grid[y][x + 1]) & (int)Directions.S) != 0) ? " " : "_"
                            ;
                    }
                    else
                    {
                        line += "|";
                    }

                }
                Console.WriteLine(line);
            }
        }
    }
}
