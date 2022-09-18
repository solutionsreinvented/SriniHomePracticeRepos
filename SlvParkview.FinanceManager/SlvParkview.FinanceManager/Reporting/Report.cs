using ReInvented.Shared.Stores;
using System;

namespace SlvParkview.FinanceManager.Reporting
{
    public abstract class Report : PropertyStore, IReport
    {
        public Report()
        {

        }

        public abstract void Generate();

        protected private abstract void CreateRequiredDirectories();

        protected private abstract void CreateHtmlFile();

        protected private abstract void CreateJsonFile();

        public string GeneratedOn => DateTime.Today.ToString("dd MMM yyyy");

        public string ApartmentName => "SLV Parkview Apartment";
    }
}
