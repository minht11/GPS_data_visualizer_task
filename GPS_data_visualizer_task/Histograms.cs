﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace GPS_data_visualizer_task
{
    class Histograms
    {
        static private Dictionary<int, int> getDictionaryFromList(List<int> list, Func<int, int> groupFn)
        {
            return list.GroupBy(groupFn).ToDictionary((item) => item.Key, (item) => item.Count());
        }
        static public void DrawVertical(List<int> dataList, int height, string yAxisName)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var data = getDictionaryFromList(dataList, (a) => a);

            int maxKey = data.Keys.Max() + 1;
            int maxValue = data.Values.Max();
            int cellHeight = maxValue / height;

            for (int i = height; i >= 0; i -= 1)
            {
                string line = "";
                int currentHeight = i * cellHeight;
                for (int j = 0; j < maxKey; j += 1)
                {
                    data.TryGetValue(j, out var value);

                    char symbol = ' ';
                    if (i == 0 && value == 0)
                    {
                        symbol = '_';
                    }
                    else if (value >= currentHeight)
                    {
                        symbol = '▒';
                    }

                    line += "".PadRight(2, symbol).PadRight(3, ' ');
                }

                if (i == height)
                {
                    line += $"{maxValue} {yAxisName}";
                }
                else if (i == 0)
                {
                    line += $"0 {yAxisName}";
                }
                Console.WriteLine(line);
            }
            for (int i = 0; i < maxKey; i += 1)
            {
                Console.Write("{0,2:D2} ", i);
            }
            Console.Write("\n");
        }

        static public void DrawHorizontalRanges(List<int> data, int intervalSize, int width, string title, string valuesTitle)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var groupedData = new SortedDictionary<int, int>(
                getDictionaryFromList(data, (value) => value - value % intervalSize)
            );

            int maxKey = groupedData.Keys.Max();
            int maxValue = groupedData.Values.Max();
            int cellWidth = maxValue / width;

            int maxRangesDigitsCount = $"{maxKey}".Length;

            bool isTitleDrawed = false;
            foreach (var item in groupedData)
            {
                int key = item.Key;
                int value = item.Value;

                string leftRange = $"{key}".PadLeft(maxRangesDigitsCount, ' ');
                string rightRange = $"{key + intervalSize - 1}".PadLeft(maxRangesDigitsCount, ' ');

                string bar = "";
                for (int i = 0; i < width; i += 1)
                {
                    bar += (i * cellWidth <= value) && (value != 0) ? '▒' : ' ';
                }

                string mainContentLine = $"[{leftRange} - {rightRange}] ¦ {bar} ";
                if (!isTitleDrawed)
                {
                    isTitleDrawed = true;
                    var maxLen = mainContentLine.Length;
                    title += " ";
                    string mainTitle = title.Length <= maxLen ? title.PadRight(maxLen, '─') : title.Substring(0, maxLen);
                    Console.WriteLine($"{mainTitle}¦ {valuesTitle}");
                }

                string line = $"{mainContentLine}¦ {value}";
                Console.WriteLine(line);
            }
        }
    }
}