using System.Collections.Generic;
using System.IO;
using System;

namespace GPS_data_visualizer_task.Gps.Readers
{
    class CsvReader : IReader
    {
        public bool Supports(string ext)
        {
            return ext.Equals(".csv");
        }
        public List<GpsRecord>? Read(string filepath)
        {
            List<GpsRecord> records = new();

            foreach (var line in File.ReadLines(filepath))
            {
                var splitLine = line.Split(',');

                int i = 0;
                records.Add(new()
                {
                    Latitude = Convert.ToDouble(splitLine[i++]),
                    Longitude = Convert.ToDouble(splitLine[i++]),
                    GpsTime = Convert.ToDateTime(splitLine[i++]),
                    Speed = Convert.ToInt32(splitLine[i++]),
                    Angle = Convert.ToInt32(splitLine[i++]),
                    Altitude = Convert.ToInt32(splitLine[i++]),
                    Satellites = Convert.ToInt32(splitLine[i++]),
                });
            }

            return records;
        }
    }
}
