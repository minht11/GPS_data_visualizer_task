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
            var data = paths.SelectMany(GpsReader.Read).ToList();

            var satelitesList = data.Select((item) => item.Satellites).ToList();
            var speedList = data.Select((item) => item.Speed).ToList();

            Console.Clear();
            Histograms.DrawVertical(satelitesList, 10, "hits");

            Console.WriteLine();
            Histograms.DrawHorizontalRanges(
                data: speedList,
                intervalSize: 10,
                width: 20,
                title: "Speed histogram",
                valuesLabel: "hits");

            Console.WriteLine();
            displayShortestRoadStripInfo(data, 100);
        }

        static private HashSet<string> getInputedPaths()
        {
            const string BEGIN_PARSE_COMMAND = "parse";
            Console.WriteLine("Enter absolute file path and press 'Enter'");
            Console.WriteLine($"Once finished type '{BEGIN_PARSE_COMMAND}' to begin parsing");

            HashSet<string> filePaths = new();

            while (true)
            {
                string value = Console.ReadLine();
                if (BEGIN_PARSE_COMMAND.Equals(value))
                {
                    break;
                }

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

            return filePaths;
        }

        static private void displayShortestRoadStripInfo(List<GpsRecord> data, double roadDistance)
        {
            TimeSpan shortestTime = TimeSpan.MaxValue;
            Tuple<double, GpsRecord, GpsRecord> fastestData = null;

            for (int i = 0; i < data.Count; i += 1)
            {
                GpsRecord startingCoord = data[i];
                GpsRecord coord = startingCoord;

                double distance = 0;

                for (int j = i + 1; j < data.Count; j += 1)
                {
                    GpsRecord nextCoord = data[j];
                    distance += GeoCalculator.GetDistance(
                        coord.Latitude,
                        coord.Longitude,
                        nextCoord.Latitude,
                        nextCoord.Longitude,
                        1,
                        DistanceUnit.Kilometers);
                    coord = nextCoord;
                    TimeSpan time = coord.GpsTime - startingCoord.GpsTime;
                    bool isMoreThanShortestTime = time > shortestTime;
                    if (distance >= roadDistance || isMoreThanShortestTime)
                    {
                        if (!isMoreThanShortestTime)
                        {
                            shortestTime = time;
                            fastestData = Tuple.Create(distance, startingCoord, coord);
                        }
                        break;
                    }
                }
            }

            if (fastestData != null)
            {
                Console.WriteLine($"Fastest road section of at least 100km was driven over {shortestTime.TotalSeconds:f3}s and was {fastestData.Item1:f3}km long.");

                const string startWord = "Start";
                Console.WriteLine($"{startWord} position {fastestData.Item2.Latitude}; {fastestData.Item2.Longitude}");
                Console.WriteLine($"{startWord} gps time {fastestData.Item2.GpsTime}");

                string endWord = "End".PadRight(startWord.Length, ' ');
                Console.WriteLine($"{endWord} position {fastestData.Item3.Latitude}; {fastestData.Item3.Longitude}");
                Console.WriteLine($"{endWord} gps time {fastestData.Item3.GpsTime}");

                Console.Write($"Average speed: {fastestData.Item1 / shortestTime.TotalHours:f1}km/h");
            }
        }
    }
}
