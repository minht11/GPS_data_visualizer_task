using System;
using System.Collections.Generic;
using System.Linq;

namespace GPS_data_visualizer_task
{
    class Helpers
    {
        static public Dictionary<int, int> GetGroupedCountDictionary(List<int> list, Func<int, int> groupFn)
        {
            return list.GroupBy(groupFn).ToDictionary((item) => item.Key, (item) => item.Count());
        }

        static public string MakeStringFixedSize(string value, int maxLength, char fillChar = ' ')
        {
            return value.Length <= maxLength
                ? value.PadRight(maxLength, fillChar)
                : value.Substring(0, maxLength);
        }
    }
}
