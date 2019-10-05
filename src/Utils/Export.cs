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
using System.IO;

using Newtonsoft.Json;
using Matze.Algorithms;
using Matze.Grids;

namespace Matze.Utils
{
    public class Export
    {
        public enum Format
        {
            Info = 1,
            Yaml = 4,
            Json = 6,
            Txt = 8,
            Csv = 16,
            Binary = 32
        }

        public enum DisplayFormat
        {
            ASCII = 4,
            Bits = 8
        };
        public static void WriteToFile(string file, CellGrid grid, Format format, bool appendFlag = false)
        {
            WriteToFile(file, GridConverter.Convert(grid), format, appendFlag);
        }

        public static void WriteToConsole(CellGrid grid, DisplayFormat format)
        {
            WriteToConsole(GridConverter.Convert(grid), format);
        }
        public static void WriteToFile(string file, BitGrid grid, Format format, bool appendFlag = false)
        {
            if ((format & Format.Info) != 0)
            {
                //to bites and ascii
            }
            if ((format & Format.Json) != 0)
            {
                var filename = file + ".json";
                WriteToFile(filename, appendFlag, (Stream stream) =>
                 {
                     Write(stream, Converter.ToJSON(grid));
                 });
            }

            if ((format & Format.Txt) != 0)
            {
                var filename = file + ".txt";
                WriteToFile(filename, appendFlag, (Stream stream) =>
                 {
                     Write(stream, Converter.ToString(grid, false));
                 });
            }
            if ((format & Format.Csv) != 0)
            {
                var filename = file + ".csv";
                WriteToFile(filename, appendFlag, (Stream stream) =>
                 {
                     Write(stream, Converter.ToString(grid));
                 });
            }
            if ((format & Format.Yaml) != 0)
            {
                var filename = file + ".yaml";
                WriteToFile(filename, appendFlag, (Stream stream) =>
                 {
                     Write(stream, Converter.ToYAML(grid));
                 });
            }
            if ((format & Format.Binary) != 0)
            {
                var filename = file + ".bin";
                if (File.Exists(filename)) { File.Delete(filename); }
                using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
                {
                    var byteGrid = Converter.ToBytes(grid);
                    writer.Write(grid.Width);
                    writer.Write(grid.Height);
                    foreach (var row in byteGrid)
                    {
                        writer.Write(row);
                    }
                }
            }


        }
        public static void WriteToConsole(BitGrid grid, DisplayFormat format)
        {
            if ((format & DisplayFormat.ASCII) != 0)
            {
                WriteLine(Console.OpenStandardOutput(), Converter.ToASCII(grid));
            }

            if ((format & DisplayFormat.Bits) != 0)
            {
                WriteLine(Console.OpenStandardOutput(), Converter.ToBits(grid));
            }

        }


        ///////////////////////////////////////////////////////////////////////////////////
        ////////// Internal helpers
        ///////////////////////////////////////////////////////////////////////////////////

        private static void WriteToFile(string filename, bool appendFlag, Action<Stream> action)
        {
            if (File.Exists(filename) && !appendFlag) { File.Delete(filename); }
            using (FileStream stream = File.Create(filename))
            {
                action(stream);
            }
        }
        private static void Write(Stream stream, string str)
        {
            var bytes = Encoding.ASCII.GetBytes(str);
            stream.Write(bytes, 0, bytes.Length);
        }
        private static void WriteLine(Stream stream, string str)
        {
            var bytes = Encoding.ASCII.GetBytes(str + "\n");
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}