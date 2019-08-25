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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matze.Algorithms
{
    public enum Directions
    {
        N = 1,
        S = 2,
        E = 4,
        W = 8
    };

    public abstract class Algorithm
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

        protected static int E => (int)Directions.E;
        protected static int S => (int)Directions.S;
        protected static int N => (int)Directions.N;
        protected static int W => (int)Directions.W;

    }
}
