using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericsPractice
{
    public class SiteCoefficients
    {
        public SiteCoefficients()
        {

        }

        public DateTime Release { get; set; }

        public Dictionary<string, List<double>> FaTable { get; set; }
        public Dictionary<string, List<double>> FvTable { get; set; }
        
        //private void PopulateFaTable()
        //{
        //    FaTable = new Dictionary<string, List<double>>
        //    {
        //        { "Ss", new List<double>() { 0.25, 0.5, 0.75, 1.0, 1.25 } },
        //        { "A", new List<double>() { 0.25, 0.5, 0.75, 1.0, 1.25 } },
        //        { "B", new List<double>() { 0.25, 0.5, 0.75, 1.0, 1.25 } },
        //        { "C", new List<double>() { 0.25, 0.5, 0.75, 1.0, 1.25 } },
        //        { "D", new List<double>() { 0.25, 0.5, 0.75, 1.0, 1.25 } },
        //        { "E", new List<double>() { 0.25, 0.5, 0.75, 1.0, 1.25 } }
        //    };
        //}

        //private void PopulateFvTable()
        //{
        //    FvTable = new Dictionary<string, List<double>>
        //    {
        //        { "S1", new List<double>() { 0.25, 0.5, 0.75, 1.0, 1.25 } },
        //        { "A", new List<double>() { 0.25, 0.5, 0.75, 1.0, 1.25 } },
        //        { "B", new List<double>() { 0.25, 0.5, 0.75, 1.0, 1.25 } },
        //        { "C", new List<double>() { 0.25, 0.5, 0.75, 1.0, 1.25 } },
        //        { "D", new List<double>() { 0.25, 0.5, 0.75, 1.0, 1.25 } },
        //        { "E", new List<double>() { 0.25, 0.5, 0.75, 1.0, 1.25 } }
        //    };
        //}

    }
}
