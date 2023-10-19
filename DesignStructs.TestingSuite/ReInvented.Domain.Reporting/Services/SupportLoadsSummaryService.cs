using System.Collections.Generic;
using System.Linq;

using ReInvented.Domain.Reporting.Models;
using ReInvented.StaadPro.Interactivity.Entities;

namespace ReInvented.Domain.Reporting.Services
{
    public class SupportLoadsSummaryService
    {
        public static HashSet<LoadCaseForces> Generate(PCDLoads pcdLoads)
        {
            Node center = pcdLoads.SupportsInformation.Center;
            List<SupportLoads> supportLoadsCollection = pcdLoads.SupportLoadsCollection.ToList();
            int nSupports = supportLoadsCollection.Count();
            int nLoadCaseCount = supportLoadsCollection[0].Loads.Count();

            HashSet<LoadCaseForces> supportLoadsSummary = new HashSet<LoadCaseForces>();



            for (int i = 0; i < nLoadCaseCount; i++)
            {
                List<LoadCaseForces> loads = supportLoadsCollection[i].Loads.ToList();

                LoadCaseForces lcForces = new LoadCaseForces()
                {
                    Fx = supportLoadsCollection.Sum(slc => slc.Loads.ToList()[i].Fx),
                    Fy = supportLoadsCollection.Sum(slc => slc.Loads.ToList()[i].Fy),
                    Fz = supportLoadsCollection.Sum(slc => slc.Loads.ToList()[i].Fz),
                    Mx = supportLoadsCollection.Sum(slc => slc.Loads.ToList()[i].Mx) + supportLoadsCollection.Sum(slc => (-1) * slc.Loads.ToList()[i].Fy * (slc.Support.Z - center.Z)),
                    My = supportLoadsCollection.Sum(slc => slc.Loads.ToList()[i].My) + supportLoadsCollection.Sum(slc => 1 * slc.Loads.ToList()[i].Fy * (slc.Support.Z - center.Z))
                }
            }


            return null;
        }
    }
}
