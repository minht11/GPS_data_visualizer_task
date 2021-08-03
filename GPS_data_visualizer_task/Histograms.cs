using System;
using System.Collections.Generic;
using System.Linq;

namespace GPS_data_visualizer_task
{
    class Histograms
    {
        static private Dictionary<int, int> GetDictionaryFromList(List<int> list, Func<int, int> groupFn)
        {
            return list.GroupBy(groupFn).ToDictionary((item) => item.Key, (item) => item.Count());
        }

        static private string MakeStringFixedSize(string value, int maxLength, char fillChar = ' ')
        {
            return value.Length <= maxLength
                ? value.PadRight(maxLength, fillChar)
                : value.Substring(0, maxLength);
        }

        static private void Setup()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine();
        }

        static public void DisplayVertical(List<int> dataList, int height, string yAxisLabel)
        {
            Setup();

            var data = GetDictionaryFromList(dataList, (a) => a);

            int width = data.Keys.Max() + 1;
            int maxValue = data.Values.Max();
            int cellHeight = maxValue / height;

            for (int i = height; i >= 0; i -= 1)
            {
                int currentHeight = i * cellHeight;
                string line = Enumerable.Range(0, width).Select((j) =>
                {
                    data.TryGetValue(j, out var value);

                    if (i == 0 && value == 0) return '_';
                    if (value >= currentHeight) return '▒';
                    return ' ';
                }).Aggregate("", (curr, s) => curr + " ".PadLeft(3, s));

                if (i == height)
                {
                    line += $"{maxValue} {yAxisLabel}";
                }
                else if (i == 0)
                {
                    line += $"0 {yAxisLabel}";
                }
                Console.WriteLine(line);
            }

            for (int i = 0; i < width; i += 1)
            {
                Console.Write("{0,2:D2} ", i);
            }
            Console.Write("\n");
        }

        static public void DisplayHorizontalRanges(List<int> data, int intervalSize, int width, string title, string valuesLabel)
        {
            Setup();

            var groupedData = new SortedDictionary<int, int>(
                GetDictionaryFromList(data, (value) => value - value % intervalSize)
            );

            int maxValue = groupedData.Values.Max();
            double cellWidth = 1.0 * maxValue / width;

            int maxRangesDigitsCount = $"{groupedData.Keys.Max()}".Length;

            bool isTitleDisplayed = false;
            foreach (var item in groupedData)
            {
                int key = item.Key;
                int value = item.Value;

                string leftRangeLabel = $"{key}".PadLeft(maxRangesDigitsCount, ' ');
                string rightRangeLabel = $"{key + intervalSize - 1}".PadLeft(maxRangesDigitsCount, ' ');

                int barFilledWidth = Convert.ToInt32(Math.Ceiling(value / cellWidth));
                string bar = "".PadRight(barFilledWidth, '▒').PadRight(width, ' ');

                string mainContentLine = $"[{leftRangeLabel} - {rightRangeLabel}] ¦ {bar} ";
                if (!isTitleDisplayed)
                {
                    isTitleDisplayed = true;
                    string mainTitle = MakeStringFixedSize($"{title} ", mainContentLine.Length, '─');
                    Console.WriteLine($"{mainTitle}¦ {valuesLabel}");
                }

                string line = $"{mainContentLine}¦ {value}";
                Console.WriteLine(line);
            }
        }
    }
}
