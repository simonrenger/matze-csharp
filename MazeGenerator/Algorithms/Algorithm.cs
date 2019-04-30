using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matze.Algorithms
{
    enum Directions
    {
        N = 1,
        S = 2,
        E = 4,
        W = 8
    };

    abstract class Algorithm
    {
        protected static Dictionary<Directions, int[]> directions = new Dictionary<Directions, int[]>
        {
            {Algorithms.Directions.E, new int[] { 1,0 }},
            {Algorithms.Directions.W, new int[] { -1,0}},
            {Algorithms.Directions.N, new int[] { 0,-1}},
            {Algorithms.Directions.S, new int[] { 0,1}}
        };

        protected static Dictionary<Directions, Directions> oppositeDirections = new Dictionary<Directions, Directions>
        {
            {Algorithms.Directions.E, Algorithms.Directions.W},
            {Algorithms.Directions.W, Algorithms.Directions.E},
            {Algorithms.Directions.N, Algorithms.Directions.S},
            {Algorithms.Directions.S, Algorithms.Directions.N}
        };
    }
}
