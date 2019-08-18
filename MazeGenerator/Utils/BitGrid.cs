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

        public BitGrid(List<List<int>> grid)
        {
            this.grid = grid;
        }
        public BitGrid(byte[][] bytes)
        {
            grid = new List<List<int>>();
            foreach (var row in bytes)
            {
                grid.Add(Enumerable.Range(0, row.Length / 4)
                        .Select(i => BitConverter.ToInt32(row, i * 4))
                        .ToList());
                foreach (var bit in grid.Last())
                {
                    if ((bit & (int)Directions.E) != 0 || (bit & (int)Directions.N) != 0 || (bit & (int)Directions.W) != 0 || (bit & (int)Directions.S) != 0) { }
                    else
                    {
                        throw new Exception("Error: provieded bytes do not match any of the possible generted numeric values!");
                    }
                }
            }
        }

        public BitGrid(string source)
        {
            grid = new List<List<int>>();
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
                        if ((bit & (int)Directions.E) != 0 || (bit & (int)Directions.N) != 0 || (bit & (int)Directions.W) != 0 || (bit & (int)Directions.S) != 0)
                        {
                            grid.Last().Add(bit);
                        }
                        else
                        {
                            throw new Exception("Error: String cannot e converted to cell because its not a correct numeric value the System could have generated!");
                        }
                    }
                    catch
                    {
                        throw new Exception("Error: String cannot e converted to cell because its not a numeric value");
                    }
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

        public byte[][] ToBytes()
        {
            List<byte[]> bytes = new List<byte[]>();
            foreach (var row in grid)
            {
                bytes.Add(row.SelectMany(i => BitConverter.GetBytes(i)).ToArray());
            }
            return bytes.ToArray();
        }

        public string ToString(bool newLine = true)
        {
            string str = "";
            foreach (var row in grid)
            {
                str += String.Join(';', row);
                str += (newLine) ? "\n" : "|";
            }
            return str;
        }
    }
}
