using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GPS_data_visualizer_task.Gps;
using GPS_data_visualizer_task.Histograms;

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

            var paths = GetFilePathsFromUserInput();
            var recordsList = paths.Select(GpsReader.Read).ToList();
            var allRecords = recordsList.SelectMany((d) => d).ToList();

            Console.Clear();
            DisplaySatellitesHistogram(allRecords);
            DisplaySpeedHistogram(allRecords);
            DisplayFastest100kmRoadSection(recordsList);
        }

        static private HashSet<string> GetFilePathsFromUserInput()
        {
            const string BEGIN_PARSE_COMMAND = "parse";
            Console.WriteLine("Enter absolute file path and press 'Enter'");
            Console.WriteLine($"Once finished type '{BEGIN_PARSE_COMMAND}' to begin parsing");

            HashSet<string> filePaths = new();

            while (true)
            {
                string? value = Console.ReadLine();
                if (BEGIN_PARSE_COMMAND.Equals(value))
                {
                    break;
                }

                if (value == null)
                {
                    continue;
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

        static private void DisplaySatellitesHistogram(List<GpsRecord> records)
        {
            Histogram.Vertical.Display(new()
            {
                Data = records.Select((item) => item.Satellites).ToList(),
                Height = 10,
                YAxisLabel = "hits",
            });
        }

        static private void DisplaySpeedHistogram(List<GpsRecord> records)
        {
            Histogram.RangesHorizontal.Display(new()
            {
                Data = records.Select((item) => item.Speed).ToList(),
                IntervalSize = 10,
                Width = 20,
                Title = "Speed histogram",
                ValuesLabel = "hits",
            });
        }

        static private void DisplayFastest100kmRoadSection(List<List<GpsRecord>> recordsList)
        {
            const int minimumDistance = 100;
            var section = recordsList
               .Select((r) => RoadSectionCalculator.FindFastest(r, minimumDistance))
               .Aggregate((curr, s) => curr == null || s?.Duration < curr.Duration ? s : curr);

            if (section != null)
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
            }
            else
            {
                Console.Write($"No road section was {minimumDistance}km long");
            }
        }
    }
}
