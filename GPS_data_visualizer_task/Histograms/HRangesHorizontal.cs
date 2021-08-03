using System;
using System.Collections.Generic;
using System.Linq;

namespace GPS_data_visualizer_task.Histograms
{
    internal class HRangesHorizontal : HBase<HRangesHorizontal.DisplayOptions>
    {
        public class DisplayOptions
        {
            public List<int> Data = new();
            public int Width;
            public int IntervalSize;
            public string Title = "";
            public string ValuesLabel = "";
        }

        override protected void DisplayImplementation(DisplayOptions options)
        {
            var data = options.Data;
            var width = options.Width;
            var intervalSize = options.IntervalSize;

            var groupedData = new SortedDictionary<int, int>(
                Helpers.GetGroupedCountDictionary(data, (value) => value - value % intervalSize)
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
                    string mainTitle = Helpers.MakeStringFixedSize($"{options.Title} ", mainContentLine.Length, '─');
                    Console.WriteLine($"{mainTitle}¦ {options.ValuesLabel}");
                }

                string line = $"{mainContentLine}¦ {value}";
                Console.WriteLine(line);
            }
        }
    }
}
