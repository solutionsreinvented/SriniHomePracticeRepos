using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CourseNamesProcessor
{
    class Program
    {
        private const string _path = @"C:\Users\srini\Desktop\01. Directories (Organized).txt";

        static void Main(string[] args)
        {
            string publisher = string.Empty;
            bool processNext;
            do
            {
                Console.WriteLine("Enter the publisher name:");
                publisher = Console.ReadLine();
                ProcessContents(publisher);
                Console.WriteLine("Do you want to process another [Y/N]? : ");
                processNext = Console.ReadLine().ToUpper() == "Y".ToUpper();
            } while (processNext);


            Console.ReadLine();
        }

        private static void ProcessContents(string publisher)
        {
            if (File.Exists(_path))
            {
                File.ReadAllLines(_path).Where(l => l.Contains(publisher)).ToList().ForEach(l => Console.WriteLine(l));
                Console.WriteLine("Number of courses: " + File.ReadAllLines(_path).Where(l => l.Contains(publisher)).Count());
            }
        }
    }
}
