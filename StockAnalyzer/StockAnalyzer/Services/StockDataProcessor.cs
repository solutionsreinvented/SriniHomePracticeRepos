using System.Collections.Generic;
using System.IO;
using System.Linq;

using StockAnalyzer.Models;

namespace StockAnalyzer.Services
{
    public class StockDataProcessor
    {
        ///private static readonly string _directoryPath = @"C:\Users\masanams\Desktop";
        ///private static readonly string _filename = $"EQ{DateTime.Today.Date.AddDays(-1).Day}{DateTime.Today.Month:D2}{DateTime.Today.Year.ToString().Substring(2, 2)}.CSV";


        public static IEnumerable<Scrip> GetScrips(string fullFilePath)
        {
            IEnumerable<Scrip> scrips = File.ReadAllLines(fullFilePath).Skip(1).Select(line => Scrip.Parse(line));

            return scrips;
        }
    }
}
