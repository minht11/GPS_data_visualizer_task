using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GPS_data_visualizer_task.Gps.Readers;

namespace GPS_data_visualizer_task.Gps
{
    class GpsReader
    {
        private static List<IReader> Readers = new()
        {
            new JsonReader(),
            new CsvReader(),
            new BinReader(),
        };

        static public bool IsSupportedFile(string filepath)
        {
            string ext = Path.GetExtension(filepath);
            return Readers.Any(p => p.Supports(ext));
        }

        static public List<GpsRecord> Read(string filepath)
        {
            string ext = Path.GetExtension(filepath);
            foreach (var reader in Readers)
            {
                if (reader.Supports(ext))
                {
                    var value = reader.Read(filepath);
                    if (value is not null)
                    {
                        return value;
                    }
                }
            }

            throw new ArgumentException("File type is not supported or file is empty");
        }
    }
}
