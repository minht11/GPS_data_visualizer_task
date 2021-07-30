using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace GPS_data_visualizer_task.GpsParsers
{
    class JsonParser : IParser
    {
        public bool Supports(string ext)
        {
            return ext.Equals(".json");
        }
        public List<GpsData> Parse(string filepath)
        {
            string jsonString = File.ReadAllText(filepath);
            return JsonSerializer.Deserialize<List<GpsData>>(jsonString);
        }
    }
}
