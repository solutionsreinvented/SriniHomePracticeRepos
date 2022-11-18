using System;
using System.Linq;

namespace ReIn.DateTimePractice
{
    class Program
    {
        static void Main(string[] args)
        {

            int cutoffDay = 29;

            var startDate = new DateTime(2019, 2, 1);
            var endDate = new DateTime(2020, 3, 1);

            var dates = DayOccurenceFinder.FindFor(startDate, endDate, cutoffDay);


            Console.WriteLine($"Total months count: {dates.Count()}");
            Console.WriteLine(string.Join("\n", dates.ToArray()));
            Console.ReadLine();
        }
    }
}


//class Backup
//{
//    private void Calculate()
//    {
//        var countMethod1 = 0;
//        int cutoffDay = 16;

//        var startDate = new DateTime(2019, 2, 15);
//        var endDate = new DateTime(2019, 3, 17);

//        for (int year = startDate.Year; year <= endDate.Year; year++)
//        {
//            if (startDate.Year == endDate.Year)
//            {
//                countMethod1 = endDate.Month - startDate.Month - 1 + (startDate.Day <= cutoffDay ? 1 : 0) + (cutoffDay <= endDate.Day ? 1 : 0);
//            }
//            else if (year == endDate.Year)
//            {
//                countMethod1 += endDate.Month - 1 + (cutoffDay <= endDate.Day ? 1 : 0);
//            }
//            else/// if (year < endDate.Year)
//            {
//                if (year == startDate.Year)
//                {
//                    countMethod1 += 12 - startDate.Month + (startDate.Day <= cutoffDay ? 1 : 0);
//                }
//                else
//                {
//                    countMethod1 += 12;
//                }
//            }
//        }

//        Console.WriteLine($"Result from Method 1: {countMethod1}");
//    }
//}

