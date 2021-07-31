using System.Collections.Generic;
using System.IO;
using System;

namespace GPS_data_visualizer_task.GpsParsers
{
    class CsvParser : IParser
    {
        public bool Supports(string ext)
        {
            return ext.Equals(".csv");
        }
        public List<GpsData> Parse(string filepath)
        {
            List<GpsData> dataList = new();

            foreach (var line in File.ReadLines(filepath))
            {
                var splitLine = line.Split(',');
                GpsData data = new();

                int i = 0;
                data.Latitude = Convert.ToDouble(splitLine[i++]);
                data.Longitude = Convert.ToDouble(splitLine[i++]);
                data.GpsTime = Convert.ToDateTime(splitLine[i++]);
                data.Speed = Convert.ToInt32(splitLine[i++]);
                data.Angle = Convert.ToInt32(splitLine[i++]);
                data.Altitude = Convert.ToInt32(splitLine[i++]);
                data.Satellites = Convert.ToInt32(splitLine[i++]);

                dataList.Add(data);
            }

            return dataList;
        }
    }
}
