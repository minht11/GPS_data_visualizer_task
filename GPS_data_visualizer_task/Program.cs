using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Geolocation;
using GPS_data_visualizer_task.Gps;

namespace GPS_data_visualizer_task
{
    class Program
    {
        static void Main(string[] args)
        {
            //string basePath = @"D:\Development\C#\GPS_data_visualizer_task\gps-data\";
            //List<string> paths = new()
            //{
            //    $@"{basePath}2019-07.json",
            //    $@"{basePath}2019-08.csv",
            //    $@"{basePath}2019-09.bin",
            //};

            var paths = getInputedPaths();
            var recordsList = paths.Select(GpsReader.Read).ToList();
            var allRecords = recordsList.SelectMany((d) => d).ToList();

            // 3.   Draw histogram of sattelites data
            var satelitesList = allRecords.Select((item) => item.Satellites).ToList();
            Console.Clear();
            Histograms.DrawVertical(satelitesList, 10, "hits");

            // 4.	Draw histogram of speed data
            var speedList = allRecords.Select((item) => item.Speed).ToList();
            Console.WriteLine();
            Histograms.DrawHorizontalRanges(
                data: speedList,
                intervalSize: 10,
                width: 20,
                title: "Speed histogram",
                valuesLabel: "hits");

            Console.WriteLine();

            // 5.	Find the road section, along all records loaded from all files,
            //      of at least 100 km long which was driven in the shortest time
            const int minimumDistance = 100;

            // The requirements were ambiguous if all records should be combined or treated separetly.
            // Search them independently, because different files might contain unrelated data.
            RoadSection? section = null;
            foreach (var record in recordsList)
            {
                var newSection = findFastestRoadSection(record, minimumDistance);
                if (section is null || newSection?.Duration < section.Duration)
                {
                    section = newSection;
                }
            }

            if (section is not null)
            {
                Console.WriteLine($"Fastest road section of at least {minimumDistance}km was driven over {section.Duration.TotalSeconds:f3}s and was {section.Distance:f3}km long.");

                GpsRecord startRecord = section.StartRecord;
                GpsRecord endRecord = section.EndRecord;

                const string startWord = "Start";
                Console.WriteLine($"{startWord} position {startRecord.Latitude}; {startRecord.Longitude}");
                Console.WriteLine($"{startWord} gps time {startRecord.GpsTime.ToString("yyyy-MM-dd hh:mm:ss")}");

                string endWord = "End".PadRight(startWord.Length, ' ');
                Console.WriteLine($"{endWord} position {endRecord.Latitude}; {endRecord.Longitude}");
                Console.WriteLine($"{endWord} gps time {endRecord.GpsTime.ToString("yyyy-MM-dd hh:mm:ss")}");

                Console.Write($"Average speed: {section.Distance / section.Duration.TotalHours:f1}km/h");
            } else
            {
                Console.Write($"No road section was {minimumDistance}km long");
            }
        }

        static private HashSet<string> getInputedPaths()
        {
            const string BEGIN_PARSE_COMMAND = "parse";
            Console.WriteLine("Enter absolute file paths, between each path press 'Enter'");
            Console.WriteLine($"Once finished type '{BEGIN_PARSE_COMMAND}' to begin parsing");

            HashSet<string> filePaths = new();

            while (true)
            {
                string? value = Console.ReadLine();
                if (BEGIN_PARSE_COMMAND.Equals(value))
                {
                    break;
                }

                if (value is not null)
                {
                    if (!File.Exists(value))
                    {
                        Console.WriteLine("No file found at inputed location");
                    }
                    else if (!GpsReader.IsSupportedFile(value))
                    {
                        Console.WriteLine("File type is not supported");
                    }
                    else
                    {
                        filePaths.Add(value);
                        Console.WriteLine("File added");
                    }
                }
            }

            return filePaths;
        }

        private class RoadSection
        {
            public TimeSpan Duration { get; init; }
            public double Distance { get; init; }
            // TODO. Assigning properties in constructor would be a proper solution.
#pragma warning disable CS8618 // Non-nullable
            public GpsRecord StartRecord { get; init; }
            public GpsRecord EndRecord { get; init; }
#pragma warning restore CS8618 // Non-nullable
        }

        static private RoadSection? findFastestRoadSection(List<GpsRecord> data, double roadDistance)
        {
            List<double> precomputedDistances = new();
            for (int i = 0; i < data.Count - 1; i += 1)
            {
                var start = data[i];
                var end = data[i + 1];
                var distance = GeoCalculator.GetDistance(start.Latitude, start.Longitude, end.Latitude, end.Longitude, 1, DistanceUnit.Kilometers);
                precomputedDistances.Add(distance);
            }

            RoadSection? section = null;
            TimeSpan shortestTime = TimeSpan.MaxValue;
            for (int i = 0; i < precomputedDistances.Count; i += 1)
            {
                double distance = 0;
                for (int j = i; j < precomputedDistances.Count; j += 1)
                {
                    distance += precomputedDistances[j];

                    TimeSpan time = data[j].GpsTime - data[i].GpsTime;
                    bool isMoreThanShortestTime = time > shortestTime;
                    if (distance >= roadDistance || isMoreThanShortestTime)
                    {
                        if (!isMoreThanShortestTime)
                        {
                            shortestTime = time;
                            section = new()
                            {
                                Distance = distance,
                                Duration = time,
                                StartRecord = data[i],
                                EndRecord = data[j],
                            };
                        }
                        break;
                    }
                }
            }

            return section;
        }
    }
}
