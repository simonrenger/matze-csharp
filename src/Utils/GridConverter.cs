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

using Matze.Grids;
using Matze.Algorithms;

namespace Matze.Utils{
    public class GridConverter{
        public static BitGrid Convert(CellGrid grid){
            return ToBitGrid(grid);
        }
        public static CellGrid Convert(BitGrid grid){
            return ToCellGrid(grid);
        }
        public static BitGrid ToBitGrid(CellGrid cellGrid){
            BitGrid grid = new BitGrid(cellGrid.Width,cellGrid.Height);
            for(var row = 0;row < grid.Height;row++){
                for(var cell = 0;cell < grid.Width;cell++){
                    var gridCell = cellGrid[row][cell];
                    grid[row][cell] |= (!gridCell.North)? (int)Directions.N:0;
                    grid[row][cell] |= (!gridCell.East)? (int)Directions.E:0;
                    grid[row][cell] |= (!gridCell.South)? (int)Directions.S:0;
                    grid[row][cell] |= (!gridCell.West)? (int)Directions.W:0;
                }
            }
            return grid;
        }
        public static CellGrid ToCellGrid(BitGrid bitGrid){
            CellGrid grid = new CellGrid(bitGrid.Width,bitGrid.Height);
            for(var row = 0;row < grid.Height;row++){
                for(var cell = 0;cell < grid.Width;cell++){
                    var gridCell = bitGrid[row][cell];
                    grid[row][cell] |= gridCell;
                }
            }
            return grid;
        }
    }
}