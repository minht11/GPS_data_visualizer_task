using System;
using System.Collections.Generic;
using System.Linq;

namespace GPS_data_visualizer_task.Histograms
{
    class HVertical : HBase<HVertical.DisplayOptions>
    {
        public class DisplayOptions
        {
            public List<int> Data { get; set; } = new();
            public int Height { get; set; }
            public string YAxisLabel { get; set; } = "";
        }

        override protected void DisplayImplementation(DisplayOptions options)
        {
            var data = Helpers.GetGroupedCountDictionary(options.Data, (a) => a);
            var height = options.Height;
            var yAxisLabel = options.YAxisLabel;

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
    }
}
