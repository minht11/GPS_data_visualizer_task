using System;

namespace GPS_data_visualizer_task.Histograms
{
    abstract class HBase<T>
    {
        public void Display(T options)
        {
            var encoding = Console.OutputEncoding;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine();
            DisplayImplementation(options);
            Console.WriteLine();

            Console.OutputEncoding = encoding;
        }

        abstract protected void DisplayImplementation(T options);
    }
}
