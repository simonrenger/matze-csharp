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
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;
using Matze.Algorithms;
using Matze.Grids;

namespace Matze.Utils
{
    public class Import
    {
        internal static string ReadFile(string file)
        {
            string export = "";
            using (StreamReader stream = new StreamReader(file))
            {
                string line;
                while ((line = stream.ReadLine()) != null)
                {
                    export += line + "\n";
                }
            }
            return export;
        }
        internal static byte[][] ReadBinaryFile(string file)
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

        public static GridType ParseAnyFile<GridType>(string name) where GridType : IGrid
        {
            Regex txtRegex = new Regex(@"(\.txt|\.grid|\.csv)");
            Regex binRegex = new Regex(@"(\.bin)");
            Regex jsonRegex = new Regex(@"(\.json)");
            Regex yamlRegex = new Regex(@"(\.yaml)");
            Match match = txtRegex.Match(name);
            if (match.Success)
            {
                var fileContent = ReadFile(name);
                return Converter.ParseString<GridType>(fileContent);
            }
            match = binRegex.Match(name);
            if (match.Success)
            {
                return Converter.ParseBytes<GridType>(ReadBinaryFile(name));
            }
            match = jsonRegex.Match(name);
            if (match.Success)
            {
                var fileContent = ReadFile(name);
                return Converter.ParseJSON<GridType>(fileContent);
            }
            match = yamlRegex.Match(name);
            if (match.Success)
            {
                var fileContent = ReadFile(name);
                return Converter.ParseYAML<GridType>(fileContent);
            }
            throw new Exception("Error: ParseFile() Does not support this file format!");
        }
    }
}