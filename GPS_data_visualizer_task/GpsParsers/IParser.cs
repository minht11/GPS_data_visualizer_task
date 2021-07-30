using System.Collections.Generic;

namespace GPS_data_visualizer_task.GpsParsers
{
    interface IParser
    {
        public bool Supports(string ext);

        public List<GpsData> Parse(string filepath);
    }
}
