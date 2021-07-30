using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPS_data_visualizer_task
{
    class GpsData
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string GpsTime { get; set; }
        public int Speed { get; set; }
        public int Angle { get; set; }
        public int Altitude { get; set; }
        public int Satellites { get; set; }
    }
}
