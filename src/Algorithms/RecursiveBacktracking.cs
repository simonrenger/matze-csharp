using System;
using System.Collections.Generic;
using Matze.Utils;

namespace Matze.Algorithms
{

    class RecursiveBacktracking : Algorithm
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