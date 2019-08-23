using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using Matze.Algorithms;

namespace Matze.Utils
{
    public class Import
    {
        private static string ReadFile(string file)
        {
            string export = "";
            using (StreamReader stream = new StreamReader(file))
            {
                string line;
                while ((line = stream.ReadLine()) != null)
                {
                    export += line+"\n";
                }
            }
            return export;
        }
        private static byte[][] ReadBinaryFile(string file)
        {
            if (File.Exists(file))
            {
                List<byte[]> result;
                using (BinaryReader reader = new BinaryReader(File.Open(file, FileMode.Open)))
                {
                    var row = reader.ReadInt32();
                    var col = reader.ReadInt32();
                    result = new List<byte[]>(col);
                    for (var y = 0; y < col; y++)
                    {
                        result.Add(reader.ReadBytes(row * sizeof(int)));
                    }
                }
                return result.ToArray();
            }
            else
            {
                throw new Exception("Error: Binary File could not be found");
            }
        }
        private static bool ValidateValues(IEnumerable<int> grid)
        {
            foreach (var bit in grid)
            {
                return ValidateValue(bit);
            }
            return true;
        }
        private static bool ValidateValue(int bit)
        {
            if (!((bit & (int)Directions.E) != 0 || (bit & (int)Directions.N) != 0 || (bit & (int)Directions.W) != 0 || (bit & (int)Directions.S) != 0))
            {
                return false;
            }
            return true;
        }
        public static BitGrid ParseAnyFile(string name)
        {
            Regex txtRegex = new Regex(@"(\.txt|\.grid|\.csv)");
            Regex binRegex = new Regex(@"(\.bin)");
            Regex jsonRegex = new Regex(@"(\.json)");
            Regex yamlRegex = new Regex(@"(\.yaml)");
            Match match = txtRegex.Match(name);
            if (match.Success)
            {
                var fileContent = ReadFile(name);
                return ParseString(fileContent);
            }
            match = binRegex.Match(name);
            if (match.Success)
            {
                return ParseBytes(ReadBinaryFile(name));
            }
            match = jsonRegex.Match(name);
            if (match.Success)
            {
                var fileContent = ReadFile(name);
                return ParseJSON(fileContent);
            }
            match = yamlRegex.Match(name);
            if (match.Success)
            {
                var fileContent = ReadFile(name);
                return ParseYAML(fileContent);
            }
            throw new Exception("Error: ParseFile() Does not support this file format!");
        }
        public static BitGrid ParseJSON(string json)
        {
            List<List<int>> result = JsonConvert.DeserializeObject<List<List<int>>>(json);
            foreach (var row in result)
            {
                if (!ValidateValues(row)) throw new Exception("Error: The given value could not have been generated with the MazeGenerator");
            }
            return new BitGrid(result);
        }
        public static BitGrid ParseBytes(byte[][] bytes)
        {
            var grid = new List<List<int>>();
            foreach (var row in bytes)
            {
                grid.Add(Enumerable.Range(0, row.Length / 4)
                        .Select(i => BitConverter.ToInt32(row, i * 4))
                        .ToList());
                if (!ValidateValues(grid.Last())) throw new Exception("Error: The given value could not have been generated with the MazeGenerator");

            }
            return new BitGrid(grid);
        }
        public static BitGrid ParseString(string source)
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
                    catch(Exception e)
                    {
                        throw new Exception("Error: String cannot e converted to cell because its not a numeric value\n Message:"+e.Message);
                    }
                }
            }
            return new BitGrid(grid);
        }
        public static BitGrid ParseYAML(string yaml)
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
                return new BitGrid(grid);
            }
            else
            {
                throw new Exception("Error: Yaml file is not correct.");
            }
        }

        public static BitGrid ParseJSONFile(string file)
        {
            var fileContent = ReadFile(file);
            return ParseJSON(fileContent);
        }
        public static BitGrid ParseCSVFile(string file)
        {
            var fileContent = ReadFile(file);
            return ParseString(fileContent);
        }
        public static BitGrid ParseBytesFile(string file) {
            var fileContent = ReadBinaryFile(file);
            return ParseBytes(fileContent);
        }
        public static BitGrid ParseYamlFile(string file) {
            var fileContent = ReadFile(file);
            return ParseYAML(fileContent);
        }

    }
}