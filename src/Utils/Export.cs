using System;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using Matze.Algorithms;

namespace Matze.Utils
{
    class Export
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