using GPS_data_visualizer_task.Gps;
using System;
using System.Collections.Generic;
using Geolocation;

namespace GPS_data_visualizer_task
{
    class RoadSectionCalculator
    {
        public class Section
        {
            public TimeSpan Duration { get; }
            public double Distance { get; }
            public GpsRecord StartRecord { get; }
            public GpsRecord EndRecord { get; }

            public Section(TimeSpan duration, double distance, GpsRecord startRecord, GpsRecord endRecord)
            {
                (Duration, Distance, StartRecord, EndRecord) = (duration, distance, startRecord, endRecord);
            }
        }

        static private List<double> GetDistancesBetweenNearByRecords(List<GpsRecord> records)
        {
            List<double> distances = new();
            for (int i = 0; i < records.Count - 1; i += 1)
            {
                GpsRecord start = records[i];
                GpsRecord end = records[i + 1];
                double distance = GeoCalculator.GetDistance(start.Latitude, start.Longitude, end.Latitude, end.Longitude, 1, DistanceUnit.Kilometers);
                distances.Add(distance);
            }
            return distances;
        }

        static public Section? FindFastest(List<GpsRecord> records, double roadDistance)
        {
            List<double> distances = GetDistancesBetweenNearByRecords(records);

            Section? section = null;
            TimeSpan shortestTime = TimeSpan.MaxValue;

            for (int i = 0; i < distances.Count; i += 1)
            {
                double distance = 0;
                GpsRecord startRecord = records[i];

                for (int j = i; j < distances.Count; j += 1)
                {
                    distance += distances[j];

                    GpsRecord endRecord = records[j];
                    TimeSpan time = endRecord.GpsTime - startRecord.GpsTime;

                    bool isMoreThanShortestTime = time > shortestTime;
                    if (distance >= roadDistance || isMoreThanShortestTime)
                    {
                        if (!isMoreThanShortestTime)
                        {
                            shortestTime = time;
                            section = new Section(time, distance, startRecord, endRecord);
                        }
                        break;
                    }
                }
            }

            return section;
        }
    }
}
