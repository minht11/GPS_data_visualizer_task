using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GPS_data_visualizer_task.GpsParsers
{
    class GpsParser
    {
        private static List<IParser> Parsers = new()
        {
            new JsonParser(),
            new CsvParser(),
            new BinParser(),
        };
        static public bool IsSupportedFile(string filepath)
        {
            string ext = Path.GetExtension(filepath);
            return Parsers.Any(p => p.Supports(ext));
        }

        static public List<GpsData> Parse(string filepath)
        {
            string ext = Path.GetExtension(filepath);
            foreach (var parser in Parsers)
            {
                if (parser.Supports(ext))
                {
                    return parser.Parse(filepath);
                }
            }

            throw new ArgumentException("No parsers support this file type.");
        }
    }
}
