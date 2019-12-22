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
using System.Reflection;

using Newtonsoft.Json;
using Matze.Algorithms;
using Matze.Grids;

namespace Matze.Utils
{
    public partial class Converter
    {
        ////////////////////////////////////////////////////////////////////////////////////
        ////////// generic Parsers Converters                                     //////////
        ////////////////////////////////////////////////////////////////////////////////////

        public static GridType ParseJSON<GridType>(string json) where GridType : IGrid
        {
            List<List<int>> result = JsonConvert.DeserializeObject<List<List<int>>>(json);
            foreach (var row in result)
            {
                if (!ValidateValues(row)) throw new Exception("Error: The given value could not have been generated with the MazeGenerator");
            }
            return (GridType)Activator.CreateInstance(typeof(GridType), new object[] { result });
        }
        public static GridType ParseBytes<GridType>(byte[][] bytes) where GridType : IGrid
        {
            var grid = new List<List<int>>();
            foreach (var row in bytes)
            {
                grid.Add(Enumerable.Range(0, row.Length / 4)
                        .Select(i => BitConverter.ToInt32(row, i * 4))
                        .ToList());
                if (!ValidateValues(grid.Last())) throw new Exception("Error: The given value could not have been generated with the MazeGenerator");

            }
            return (GridType)Activator.CreateInstance(typeof(GridType), new object[] { grid });
        }
        public static GridType ParseString<GridType>(string source) where GridType : IGrid
        {
            var grid = new List<List<int>>();
            var rows = source.Split('\n');

            if (rows.Length == 0)
            {
                rows = source.Split('|');
            }

            foreach (var row in rows)
            {
                if (row == "") continue;
                var cells = row.Split(';');
                grid.Add(new List<int>());
                foreach (var cell in cells)
                {
                    try
                    {
                        var bit = Int32.Parse(cell);
                        if (ValidateValue(bit))
                        {
                            grid.Last().Add(bit);
                        }
                        else
                        {
                            throw new Exception("Error: String cannot e converted to cell because its not a correct numeric value the System could have generated!");
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Error: String cannot e converted to cell because its not a numeric value\n Message:" + e.Message);
                    }
                }
            }
            return (GridType)Activator.CreateInstance(typeof(GridType), new object[] { grid });
        }
        public static GridType ParseYAML<GridType>(string yaml) where GridType : IGrid
        {
            yaml = yaml.Replace(" ", "");
            yaml = yaml.Replace("\t", "");
            var chars = yaml.ToCharArray();
            if (chars[0] == '-' && chars[1] == '-' && chars[2] == '-')
            {
                string line = "";
                List<List<int>> grid = new List<List<int>>();
                for (var idx = 3; idx < chars.Length; idx++)
                {
                    if (chars[idx] == '-' && chars[idx + 1] == '-')
                    {
                        //new row
                        grid.Add(new List<int>());
                        line = "";
                        line += chars[idx + 2];
                        idx += 2;
                    }
                    else if (chars[idx] == '-')
                    {
                        line += chars[idx + 1];
                        idx += 1;
                    }
                    else
                    {
                        line += chars[idx];
                        if (idx + 1 < chars.Length && chars[idx + 1] == '-')
                        {
                            var value = Int32.Parse(line);
                            if (!ValidateValue(value)) throw new Exception("Error: The given value could not have been generated with the MazeGenerator");
                            grid.Last().Add(value);
                            line = "";
                        }
                        else if (idx + 1 == chars.Length)
                        {
                            var value = Int32.Parse(line);
                            if (!ValidateValue(value)) throw new Exception("Error: The given value could not have been generated with the MazeGenerator");
                            grid.Last().Add(value);
                        }
                    }
                }
                return (GridType)Activator.CreateInstance(typeof(GridType), new object[] { grid });
            }
            else
            {
                throw new Exception("Error: Yaml file is not correct.");
            }
        }

        public static GridType ParseJSONFile<GridType>(string file) where GridType : IGrid
        {
            var fileContent = Import.ReadFile(file);
            return ParseJSON<GridType>(fileContent);
        }
        public static GridType ParseCSVFile<GridType>(string file) where GridType : IGrid
        {
            var fileContent = Import.ReadFile(file);
            return ParseString<GridType>(fileContent);
        }
        public static GridType ParseBytesFile<GridType>(string file) where GridType : IGrid
        {
            var fileContent = Import.ReadBinaryFile(file);
            return ParseBytes<GridType>(fileContent);
        }
        public static GridType ParseYamlFile<GridType>(string file) where GridType : IGrid
        {
            var fileContent = Import.ReadFile(file);
            return ParseYAML<GridType>(fileContent);
        }
    }
}