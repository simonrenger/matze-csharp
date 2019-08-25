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
using System.Text;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;
using Matze.Algorithms;
using Matze.Grids;

namespace Matze.Utils
{
    public class Export
    {
        static public byte[][] ToBytes(BitGrid bitgrid)
        {
            var grid = bitgrid.Grid;
            List<byte[]> bytes = new List<byte[]>();
            foreach (var row in grid)
            {
                bytes.Add(row.SelectMany(i => BitConverter.GetBytes(i)).ToArray());
            }
            return bytes.ToArray();
        }

        public static string ToString(BitGrid bitgrid,bool newLine = true)
        {
            var grid = bitgrid.Grid;
            string str = "";
            foreach (var row in grid)
            {
                str += String.Join(';', row);
                str += (newLine) ? "\n" : "|";
            }
            return str;
        }

        public static string ToCSV(BitGrid bitgrid){
         return ToString(bitgrid);
        }

        public static string ToJSON(BitGrid grid){
            return JsonConvert.SerializeObject(grid.Grid);
        }

        public static string ToYAML(BitGrid grid){
            string export = "---\n";
            foreach(var row in grid.Grid){
                export += "- ";
                var newRow = true;
                foreach(var cell in row){
                    export += ((newRow)? "":"\t")+"- "+cell+"\n";
                    newRow = false;
                }
            }
            return export;
        }
    }
}