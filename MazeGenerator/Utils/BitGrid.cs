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

        public void PrintBits()
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
        public struct Cell
        {
            public bool east;
            public bool west;
            public bool south;
            public bool north;
            public Cell(bool north, bool east, bool south, bool west)
            {
                this.east = east;
                this.west = west;
                this.north = north;
                this.south = south;
            }
        }
        public void Print()
        {

            Action<int,int> DrawLine = (int y, int max) =>
            {
                var line = "";
                for (int x = 0; x <= max; x++)
                {
                    line += "###";
                }
                line += "##";
                Console.WriteLine(line);
            };

            Console.Write("\n");
            for (int y = 0; y <= grid.Size(); y++)
            {
                if(y == 0){
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
                    //cell: |_|
                    if (hasNoWallWest)
                    {
                        cell = cell.ReplaceAt(0, '.');

                    }
                    if (hasNoWallEast)
                    {
                        cell = cell.ReplaceAt(2, '.');
                    }
                    if (hasNoWallSouth)
                    {
                        cell = cell.ReplaceAt(1, ' ');
                    }
                    line += cell;
                }
                line += "#";
                Console.WriteLine(line);
                if(y == grid.Size()){
                    DrawLine(y, grid.Size());
                }
            }
        }
    }
}
