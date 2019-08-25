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
using Matze.Algorithms;

namespace Matze.Grids
{
    public class Cell
    {
        bool north;
        bool south;
        bool east;
        bool west;
        public Cell(bool north, bool east, bool south, bool west)
        {
            this.north = north;
            this.east = east;
            this.south = south;
            this.west = west;
        }
        public Cell()
        {
            north = true;
            east = true;
            west = true;
            south = true;
        }

        public bool North
        {
            set { north = value; }
            get { return north; }
        }
        public bool East
        {
            set { east = value; }
            get { return east; }
        }
        public bool South
        {
            set { south = value; }
            get { return south; }
        }
        public bool West
        {
            set { west = value; }
            get { return west; }
        }

        public void UpdateViaDirection(Directions directions)
        {
            switch (directions)
            {
                case Directions.E:
                    East = true;
                    break;
                case Directions.N:
                    North = true;
                    break;
                case Directions.S:
                    South = true;
                    break;
                case Directions.W:
                    West = true;
                    break;
            }
        }
        public void UpdateViaBit(int directions)
        {
            North = (((int)Directions.N & directions) != 0) ? false : true;
            East = (((int)Directions.E & directions) != 0) ? false : true;
            South = (((int)Directions.S & directions) != 0) ? false : true;
            West = (((int)Directions.W & directions) != 0) ? false : true;
        }

        static public Cell operator|(Cell org,Directions directions)
        {
            org.North = (Directions.N == directions) ? false : true;
            org.East = (Directions.E == directions) ? false : true;
            org.South = (Directions.S == directions) ? false : true;
            org.West = (Directions.W == directions) ? false : true;
            return org;
        }
        static public Cell operator|(Cell org,int direction)
        {
            org.North = (((int)Directions.N & direction) != 0) ? false : true;
            org.East = (((int)Directions.E & direction) != 0) ? false : true;
            org.South = (((int)Directions.S & direction) != 0) ? false : true;
            org.West = (((int)Directions.W & direction) != 0) ? false : true;
            return org;
        }
    }
}