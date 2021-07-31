using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;

namespace GPS_data_visualizer_task.GpsParsers
{
    class BinParser : IParser
    {
        public bool Supports(string ext)
        {
            return ext.Equals(".bin");
        }
        public List<GpsData> Parse(string filepath)
        {
            List<GpsData> dataList = new();

            int offset = 0;
            byte[] bytes = File.ReadAllBytes(filepath);

            Func<int, byte[]> readBytes = (size) =>
            {

                var splicedBytes = bytes.Skip(offset).Take(size);
                offset += size;

                var formatedBytes = BitConverter.IsLittleEndian
                   ? splicedBytes.Reverse() : splicedBytes;
                return formatedBytes.ToArray();
            };

            while (offset < bytes.Length)
            {
                GpsData data = new();

                data.Latitude = BitConverter.ToInt32(readBytes(4)) / 10000000.0;
                data.Longitude = BitConverter.ToInt32(readBytes(4)) / 10000000.0;

                long ms = BitConverter.ToInt64(readBytes(8));
                data.GpsTime = DateTime.UnixEpoch.AddMilliseconds(ms);

                data.Speed = BitConverter.ToInt16(readBytes(2));
                data.Angle = BitConverter.ToInt16(readBytes(2));
                data.Altitude = BitConverter.ToInt16(readBytes(2));

                data.Satellites = bytes[offset];
                offset += 1;

                dataList.Add(data);
            }
            return dataList;
        }
    }
}
