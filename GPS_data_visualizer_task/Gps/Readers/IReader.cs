using System.Collections.Generic;

namespace GPS_data_visualizer_task.Gps.Readers
{
    interface IReader
    {
        public bool Supports(string ext);

        public List<GpsRecord>? Parse(string filepath);
    }
}
