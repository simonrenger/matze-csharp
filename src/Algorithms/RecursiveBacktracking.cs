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

﻿using System;
using System.Collections.Generic;
using Matze.Utils;

namespace Matze.Algorithms
{
    public class RecursiveBacktracking : Algorithm
    {
        public static BitGrid Generate(Random rand, int width, int height)
        {
            BitGrid bitGrid = new BitGrid(width, height);
            return Generator(0, 0, ref bitGrid,ref rand);
        }
        private static ref BitGrid Generator(int x, int y, ref BitGrid bitGrid,ref Random rand)
        {
            Directions[] directions_ = new Directions[] { Directions.E, Directions.N, Directions.S, Directions.W };
            directions_.Shuffle(ref rand);
            foreach (var direction in directions_)
            {
                var nx = x + directions[direction][0];
                var ny = y + directions[direction][1];
                if (
                    ny >= 0 && ny <= bitGrid.Height - 1 &&
                    nx >= 0 && nx <= bitGrid[ny].Size() &&
                    bitGrid[ny][nx] == 0
                )
                {
                    bitGrid[y][x] |= (int)direction;
                    bitGrid[ny][nx] |= (int)oppositeDirections[direction];
                    Generator(nx, ny, ref bitGrid,ref rand);
                }
            }

            return ref bitGrid;
        }
    }

}