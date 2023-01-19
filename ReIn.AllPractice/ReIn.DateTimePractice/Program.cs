using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace ReIn.DateTimePractice
{
    class Program
    {
        static void Main(string[] args)
        {
            Head srini = new Head("Srinivas");


            Console.ReadLine();
        }

        private static IPerson DeepClone<TSource>(TSource sourceObject) where TSource : IPerson
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, sourceObject);
                stream.Position = 0;

                return (IPerson)formatter.Deserialize(stream);
            }
        }

        interface IPerson
        {
            int Id { get; }

            string Name { get; set; }
        }


        [Serializable]
        class Person : IPerson
        {
            private static int _currentId;

            public Person(string name)
            {
                _currentId++;
                Id = _currentId;
                Name = name;
            }

            public int Id { get; private set; }

            public string Name { get; set; }
        }

        [Serializable]
        class Head : Person, IPerson
        {
            public Head(string name) : base(name)
            {
                Description = "Head";
            }

            public string Description { get; set; }
        }

    }
}

#region Commented Code

//private static void EarlierTesting()
//{
//    int cutoffDay = 29;

//    var startDate = new DateTime(2019, 2, 1);
//    var endDate = new DateTime(2020, 3, 1);

//    var dates = DayOccurenceFinder.FindFor(startDate, endDate, cutoffDay);


//    Console.WriteLine($"Total months count: {dates.Count()}");
//    Console.WriteLine(string.Join("\n", dates.ToArray()));
//}

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

#endregion

