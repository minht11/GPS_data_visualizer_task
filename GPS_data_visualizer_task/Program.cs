using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace GPS_data_visualizer_task
{
    class Program
    {
        static void Main(string[] args)
        {
            const string BEGIN_PARSE_COMMAND = "parse";
            Console.WriteLine("Enter absolute file paths");
            Console.WriteLine($"Type '{BEGIN_PARSE_COMMAND}' once you done to begin parsing");

            List<string> paths = new()
            {
                @"D:\Development\C#\GPS_data_visualizer_task\gps-data\2019-07.json",
                @"D:\Development\C#\GPS_data_visualizer_task\gps-data\2019-08.csv",
                @"D:\Development\C#\GPS_data_visualizer_task\gps-data\2019-09.bin",
            };

            var data = paths.SelectMany((p) => GpsParsers.GpsParser.Parse(p)).ToList();

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
                valuesTitle: "hits");

            //HashSet<string> filePaths = new();

            //while(true)
            //{
            //    string value = Console.ReadLine();
            //    if (BEGIN_PARSE_COMMAND.Equals(value))
            //    {
            //        Console.WriteLine("YOOOOO");
            //        break;
            //    }

            //    if (isSupportedFile(value) && File.Exists(value))
            //    {
            //        filePaths.Add(value);
            //    } else
            //    {
            //        Console.WriteLine("This is not a valid file");
            //    }
            //}

            //foreach (var path in filePaths)
            //{
            //    var a = Parser.parse(path);
            //    var b = a[0];
            //    Console.WriteLine(b);
            //}
        }
    }
}
