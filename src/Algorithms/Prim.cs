using System;
using System.Collections.Generic;
using Matze.Utils;

namespace Matze.Algorithms
{
    class Prim : Algorithm
    {

        private const int Frontier = 32;
        private const int In = 16;

        private class Pos
        {
            public int X;
            public int Y;
            public Pos(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }
        public static BitGrid Generate(Random rand, int width, int height)
        {
            var grid = new BitGrid(width, height);
            var frontiers = new List<Pos>();
            var beginX = rand.Next(0, width - 1);
            var beginY = rand.Next(0, height - 1);
            //start algorithm
            MarkFrontiers(beginX, beginY, ref grid, ref frontiers);
            while (frontiers.Count != 0)
            {
                var frontier = frontiers[rand.Next(0, frontiers.Count - 1)];
                frontiers.Remove(frontier);
                var neighours = GetNeighbors(frontier.X, frontier.Y, grid);
                var randomNeighour = neighours[rand.Next(0, neighours.Count - 1)];
                var direction = GetDirection(frontier.X, frontier.Y, randomNeighour.X, randomNeighour.Y);
                grid[frontier.Y][frontier.X] |= (int)direction;
                grid[randomNeighour.Y][randomNeighour.X] |= (int)oppositeDirections[direction];
                MarkFrontiers(frontier.X, frontier.Y, ref grid, ref frontiers);
            }

            return grid;
        }
        private static void MarkFrontiers(int x, int y, ref BitGrid grid, ref List<Pos> frontiers)
        {
            grid[y][x] |= In;
            CreateFrontier(x - 1, y, ref grid, ref frontiers);
            CreateFrontier(x + 1, y, ref grid, ref frontiers);
            CreateFrontier(x, y - 1, ref grid, ref frontiers);
            CreateFrontier(x, y + 1, ref grid, ref frontiers);
        }
        private static void CreateFrontier(int posX, int posY, ref BitGrid grid, ref List<Pos> frontiers)
        {
            if (posX >= 0 && posY >= 0 && posY < grid.Height && posX < grid.Width && grid[posY][posX] == 0)
            {
                grid[posY][posX] |= Frontier;
                frontiers.Add(new Pos(posX, posY));
            }
        }
        private static List<Pos> GetNeighbors(int x, int y, BitGrid grid)
        {
            var neighours = new List<Pos>();
            if (x > 0 && (grid[y][x - 1] & In) != 0) neighours.Add(new Pos(x - 1, y));
            if (x + 1 < grid.Width && (grid[y][x + 1] & In) != 0) neighours.Add(new Pos(x + 1, y));
            if (y > 0 && (grid[y - 1][x] & In) != 0) neighours.Add(new Pos(x, y - 1));
            if (y + 1 < grid.Height && (grid[y + 1][x] & In) != 0) neighours.Add(new Pos(x, y + 1));
            return neighours;
        }
        private static Directions GetDirection(int x, int y, int nx, int ny)
        {
            if (x < nx) return Directions.E;
            if (x > nx) return Directions.W;
            if (y < ny) return Directions.S;
            if (y > ny) return Directions.N;
            throw new Exception("Error: Undefined behvaiour in GetDirections()");
        }
    }
}