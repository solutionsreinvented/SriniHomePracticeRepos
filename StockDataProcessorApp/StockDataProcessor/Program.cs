using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockDataProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            string csvFilePath = @"C:\Users\srini\Desktop\EQ270622.csv";
            var scrips = ScripRepository.GetAll(csvFilePath);

            scrips.Where(s => s.CompanyName.StartsWith("M")).OrderBy(s => s.CompanyName).ToList().ForEach(s => Console.WriteLine(s.CompanyName));

            Console.ReadLine();
        }
    }
}
