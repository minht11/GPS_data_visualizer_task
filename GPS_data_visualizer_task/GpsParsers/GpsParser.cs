using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GPS_data_visualizer_task.GpsParsers
{
    class GpsParser
    {
        private static HashSet<IParser> Parsers = new()
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
            List<GpsData> data = new();
            string ext = Path.GetExtension(filepath);
            foreach (var parser in Parsers)
            {
                if (parser.Supports(ext))
                {
                    data = parser.Parse(filepath);
                }
            }
            return data;
        }
    }
}
