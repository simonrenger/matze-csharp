using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Matze.Utils;

namespace Matze.Algorithms
{
    class KruskalRandomized : Algorithm
    {
         public static BitGrid Generate(Random rand, int width, int height)
        {
            BitGrid bitGrid = new BitGrid(width, height);
            // fill sets:
            List<List<Tree>> sets = new List<List<Tree>>();
            List<int[]> edges = new List<int[]>();
            for (int y = 0;y< height; y++) { 
                sets.Add(new List<Tree>());
                for (int x = 0; x < width; x++)
                {
                    if(y > 0) edges.Add(new []{x,y,(int) Directions.N });
                    if (x > 0) edges.Add(new[] { x, y, (int)Directions.W });
                    sets[y].Add(new Tree());
                }
            }
            // create edges
            Generator(ref bitGrid,ref sets,ref edges,ref rand);

            return bitGrid;
        }
        private static void Generator(ref BitGrid bitGrid,ref List<List<Tree>> sets, ref List<int[]> edges,ref Random rand)
        {
            edges.Shuffle(ref rand);
            while (edges.Count != 0)
            {
                var edge = edges.Pop();
                var x = edge[0];
                var y = edge[1];
                var direction = (Directions) edge[2];
                var nx = x + directions[direction][0];
                var ny = y + directions[direction][1];
                var set1 = sets[y][x];
                var set2 = sets[ny][nx];
                if (!set1.IsConnected(set2))
                {
                    set1.Connect(set2);
                    bitGrid[y][x] |= (int) direction;
                    bitGrid[ny][nx] |= (int) oppositeDirections[direction];
                }
            }
        }
    }
}
