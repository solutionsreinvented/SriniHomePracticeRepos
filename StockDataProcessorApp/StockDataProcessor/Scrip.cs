using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockDataProcessor
{
    public class Scrip
    {
        public Scrip()
        {

        }

        public static Scrip Parse(string line)
        {
            Scrip parsedScrip = new Scrip();
            var scripData = line.Split(',');

            parsedScrip.Symbol = scripData[0];
            parsedScrip.CompanyName = scripData[1];
            parsedScrip.Series = scripData[2];
            parsedScrip.ListedOn = DateTime.Parse(scripData[3]);
            parsedScrip.PaidUpValue = decimal.Parse(scripData[4]);
            parsedScrip.MarketLot = int.Parse(scripData[5]);
            parsedScrip.ISIN = scripData[6];
            parsedScrip.FaceValue = decimal.Parse(scripData[7]);

            return parsedScrip;
        }

        public string Symbol { get; set; }

        public string CompanyName { get; set; }

        public string Series { get; set; }

        public DateTime ListedOn { get; set; }

        public decimal PaidUpValue { get; set; }

        public int MarketLot { get; set; }

        public string ISIN { get; set; }

        public decimal FaceValue { get; set; }
    }
}
