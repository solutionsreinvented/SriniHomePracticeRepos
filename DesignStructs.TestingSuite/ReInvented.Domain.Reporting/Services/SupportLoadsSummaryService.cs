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
                LoadCaseForces lcForces = new LoadCaseForces()
                {
                    Id = supportLoadsCollection[0].Loads.ToList()[i].Id,
                    Fx = supportLoadsCollection.Sum(slc => slc.Loads.ToList()[i].Fx),
                    Fy = supportLoadsCollection.Sum(slc => slc.Loads.ToList()[i].Fy),
                    Fz = supportLoadsCollection.Sum(slc => slc.Loads.ToList()[i].Fz),
                    Mx = supportLoadsCollection.Sum(slc => slc.Loads.ToList()[i].Mx) + supportLoadsCollection.Sum(slc => (-1.0) * slc.Loads.ToList()[i].Fy * (slc.Support.Z - center.Z)),
                    My = supportLoadsCollection.Sum(slc => slc.Loads.ToList()[i].My) + supportLoadsCollection.Sum(slc => 1.0 * slc.Loads.ToList()[i].Fx * (slc.Support.Z - center.Z))
                                                                                     + supportLoadsCollection.Sum(slc => (-1.0) * slc.Loads.ToList()[i].Fz * (slc.Support.X - center.X)),
                    Mz = supportLoadsCollection.Sum(slc => slc.Loads.ToList()[i].Mz) + supportLoadsCollection.Sum(slc => 1.0 * slc.Loads.ToList()[i].Fy * (slc.Support.X - center.X))
                };

                _ = supportLoadsSummary.Add(lcForces);
            }

            return supportLoadsSummary;
        }
    }
}
