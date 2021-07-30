using System;
using System.Collections.Generic;
using System.IO;

namespace GPS_data_visualizer_task
{
    class Program
    {
        static void Main(string[] args)
        {
            const string BEGIN_PARSE_COMMAND = "parse";
            Console.WriteLine("Enter absolute file locations");
            Console.WriteLine($"Type '{BEGIN_PARSE_COMMAND}' once you done to begin parsing");

            //string path = @"D:\Development\C#\GPS_data_visualizer_task\gps-data\test.json";
            string path = @"D:\Development\C#\GPS_data_visualizer_task\gps-data\test.csv";

            var data = GpsParsers.GpsParser.Parse(path);
            Console.WriteLine($"{data[0].Latitude} {data[0].Speed} {data[0].Satellites}");

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
