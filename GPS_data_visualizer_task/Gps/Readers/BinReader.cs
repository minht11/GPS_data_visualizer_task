using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;

namespace GPS_data_visualizer_task.Gps.Readers
{
    class BinReader : IReader
    {
        public bool Supports(string ext)
        {
            return ext.Equals(".bin");
        }
        public List<GpsRecord>? Read(string filepath)
        {
            List<GpsRecord> records = new();

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
                GpsRecord record = new();

                record.Latitude = BitConverter.ToInt32(readBytes(4)) / 10000000.0;
                record.Longitude = BitConverter.ToInt32(readBytes(4)) / 10000000.0;

                long ms = BitConverter.ToInt64(readBytes(8));
                record.GpsTime = DateTime.UnixEpoch.AddMilliseconds(ms);

                record.Speed = BitConverter.ToInt16(readBytes(2));
                record.Angle = BitConverter.ToInt16(readBytes(2));
                record.Altitude = BitConverter.ToInt16(readBytes(2));

                record.Satellites = bytes[offset];
                offset += 1;

                records.Add(record);
            }
            return records;
        }
    }
}
