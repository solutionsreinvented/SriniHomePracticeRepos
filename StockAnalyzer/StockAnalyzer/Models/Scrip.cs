using System;

using StockAnalyzer.Enums;

namespace StockAnalyzer.Models
{
    public class Scrip
    {

        public static Scrip Parse(string data)
        {
            Scrip parsedScrip = new Scrip();

            string[] dataArray = data.Split(',');

            parsedScrip.Code = int.Parse(dataArray[0]);
            parsedScrip.Name = dataArray[1];
            parsedScrip.Group = (ScripGroup)Enum.Parse(typeof(ScripGroup), dataArray[2]);
            parsedScrip.Type = dataArray[3];
            parsedScrip.Open = decimal.Parse(dataArray[4]);
            parsedScrip.High = decimal.Parse(dataArray[5]);
            parsedScrip.Low = decimal.Parse(dataArray[6]);
            parsedScrip.Close = decimal.Parse(dataArray[7]);
            parsedScrip.LastTraded = decimal.Parse(dataArray[8]);
            parsedScrip.PreviousClose = decimal.Parse(dataArray[9]);

            parsedScrip.NumberOfTrades = int.Parse(dataArray[10]);
            parsedScrip.NumberOfShares = int.Parse(dataArray[11]);
            parsedScrip.NetTurnOver = decimal.Parse(dataArray[12]);

            return parsedScrip;
        }

        public int Code { get; set; }

        public string Name { get; set; }

        public ScripGroup Group { get; set; }

        public string Type { get; set; }

        public decimal Open { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Close { get; set; }

        public decimal LastTraded { get; set; }

        public decimal PreviousClose { get; set; }

        public int NumberOfTrades { get; set; }

        public int NumberOfShares { get; set; }

        public decimal NetTurnOver { get; set; }

    }
}
