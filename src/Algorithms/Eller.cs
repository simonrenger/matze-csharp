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
using System.Threading;
using System.Linq;

using Matze.Utils;
using Matze.Grids;

namespace Matze.Algorithms
{
    public class Eller : Algorithm
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