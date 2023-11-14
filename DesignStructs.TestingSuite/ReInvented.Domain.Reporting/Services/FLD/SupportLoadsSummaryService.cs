using System.Collections.Generic;
using System.Linq;

using ReInvented.Domain.Reporting.Models;
using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Models;

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
                LoadCaseForces lcForces = new LoadCaseForces()
                {
                    Id = supportLoadsCollection[0].Loads.ToList()[i].Id,
                    Forces = new Forces()
                };

                lcForces.Forces.Fx = supportLoadsCollection.Sum(slc => slc.Loads.ToList()[i].Forces.Fx);
                lcForces.Forces.Fy = supportLoadsCollection.Sum(slc => slc.Loads.ToList()[i].Forces.Fy);
                lcForces.Forces.Fz = supportLoadsCollection.Sum(slc => slc.Loads.ToList()[i].Forces.Fz);

                lcForces.Forces.Mx = supportLoadsCollection.Sum(slc => slc.Loads.ToList()[i].Forces.Mx) +
                                     supportLoadsCollection.Sum(slc => (-1.0) * slc.Loads.ToList()[i].Forces.Fy * (slc.Support.Z - center.Z));

                lcForces.Forces.My = supportLoadsCollection.Sum(slc => slc.Loads.ToList()[i].Forces.My) +
                                     supportLoadsCollection.Sum(slc => 1.0 * slc.Loads.ToList()[i].Forces.Fx * (slc.Support.Z - center.Z)) +
                                     supportLoadsCollection.Sum(slc => (-1.0) * slc.Loads.ToList()[i].Forces.Fz * (slc.Support.X - center.X));

                lcForces.Forces.Mz = supportLoadsCollection.Sum(slc => slc.Loads.ToList()[i].Forces.Mz) +
                                     supportLoadsCollection.Sum(slc => 1.0 * slc.Loads.ToList()[i].Forces.Fy * (slc.Support.X - center.X));



                _ = supportLoadsSummary.Add(lcForces);
            }

            return supportLoadsSummary;
        }
    }
}
