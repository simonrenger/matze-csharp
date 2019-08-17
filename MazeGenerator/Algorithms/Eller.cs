using System;
using Matze.Utils;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace Matze.Algorithms
{
    class Eller : Algorithm
    {
        public static BitGrid Generate(Random rand, int width, int height)
        {
            BitGrid bitGrid = new BitGrid(width, height);

            //var row = 0;
            for (var row = 0; row < height; row++)
            {
                var sets = CreateNewSets(width);
                if (row == bitGrid.Height - 1 || row == 0)
                {
                    for (var idx = 0; idx < width; idx++)
                    {
                        bitGrid[row][idx] |= (idx < width-1) ? E : W;
                    }
                }
                else
                {
                    //join them:
                    for (var idx = 0; idx < width; idx++)
                    {
                        if (rand.Next(0, 2) > 0 && (idx + 1) < sets.Count)
                        {
                            if (!sets[idx].Overlaps(sets[idx + 1]))
                            {
                                sets[idx].UnionWith(sets[idx + 1]);
                                sets.RemoveAt(idx + 1);
                            }
                        }
                    }

                    foreach (var set in sets)
                    {
                        if (set.Count != 1)
                        {
                            foreach (var index in set)
                            {
                                bitGrid[row][index] |= (index != set.Last()) ? E : W;
                            }
                            bitGrid = CreateVerticalConnection(bitGrid, row, set.ElementAt(rand.Next(0, set.Count - 1)));
                        }
                        else
                        {
                            bitGrid = CreateVerticalConnection(bitGrid, row, set.Last());
                        }
                    }
                }
            }
            return bitGrid;
        }
        private static BitGrid CreateVerticalConnection(BitGrid bitGrid, int row, int index)
        {
            bitGrid[row][index] |= S;
            if (row + 1 < bitGrid.Height)
            {
                bitGrid[row + 1][index] |= N;
            }
            return bitGrid;
        }
        private static List<HashSet<int>> CreateNewSets(int length)
        {
            var sets = new List<HashSet<int>>();
            for (var idx = 0; idx < length; ++idx)
            {
                sets.Add(new HashSet<int>() { idx });
            }
            return sets;
        }
    }
}