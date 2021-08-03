using System.Collections.Generic;
using System.Text.Json;
using System.IO;

namespace GPS_data_visualizer_task.Gps.Readers
{
    class JsonReader : IReader
    {
        public bool Supports(string ext)
        {
            return ext.Equals(".json");
        }
        public List<GpsRecord>? Read(string filepath)
        {
            string jsonString = File.ReadAllText(filepath);
            return JsonSerializer.Deserialize<List<GpsRecord>>(jsonString);
        }
    }
}
