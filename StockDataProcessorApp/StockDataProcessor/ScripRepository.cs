using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace StockDataProcessor
{
    public class ScripRepository
    {
        public ScripRepository()
        {

        }

        public static IEnumerable<Scrip> GetAll(string csvFilePath)
        {
            List<Scrip> scrips;

            if (File.Exists(csvFilePath))
            {
                scrips = File.ReadAllLines(csvFilePath).Skip(1).Select(line => Scrip.Parse(line)).ToList();
            }
            else
            {
                scrips = null;
            }

            return scrips;
        }

    }
}
