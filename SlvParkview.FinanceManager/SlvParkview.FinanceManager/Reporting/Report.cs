using ReInvented.Shared.Stores;
using System;

namespace SlvParkview.FinanceManager.Reporting
{
    public class Report : PropertyStore, IReport
    {
        public Report()
        {

        }

        public string GeneratedOn => DateTime.Today.ToString("dd MMM yyyy");

        public string ApartmentName => "SLV Parkview Apartment";
    }
}
