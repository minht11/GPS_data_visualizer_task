using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GPS_data_visualizer_task.Gps.Readers;

namespace GPS_data_visualizer_task.Gps
{
    class GpsReader
    {
        private static List<IReader> Parsers = new()
        {
            new JsonReader(),
            new CsvReader(),
            new BinReader(),
        };

        static public bool IsSupportedFile(string filepath)
        {
            string ext = Path.GetExtension(filepath);
            return Parsers.Any(p => p.Supports(ext));
        }

        static public List<GpsRecord> Read(string filepath)
        {
            string ext = Path.GetExtension(filepath);
            foreach (var parser in Parsers)
            {
                if (parser.Supports(ext))
                {
                    return parser.Parse(filepath);
                }
            }

            throw new ArgumentException("File type is not supported");
        }
    }
}
