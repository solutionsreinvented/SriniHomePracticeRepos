using System.Collections.Generic;
using System.Linq;

using OpenSTAADUI;

using ReInvented.Domain.Design.ExcelInterop.Models;
using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Enums;
using ReInvented.StaadPro.Interactivity.Models;

namespace ReInvented.Domain.Design.ExcelInterop.Services
{
    public class StaadOutputServices
    {

        public static HashSet<MemberForces> RetrieveForces(IOSOutputUI openStaadOutput, IEnumerable<Beam> beams, IEnumerable<int> loadCases)
        {
            HashSet<MemberForces> memberForcesCollection = new HashSet<MemberForces>();

            foreach (int lc in loadCases)
            {
                foreach (Beam b in beams)
                {

                }
            }

            return new HashSet<MemberForces>();
        }

        public static HashSet<MemberForces> RetrieveForces(StaadModelWrapper wrapper, IEnumerable<int> beams, IEnumerable<int> loadCases)
        {
            IOSOutputUI openStaadOutput = wrapper.Output;
            IOSLoadUI openStaadLoad = wrapper.Load;

            if (!openStaadOutput.AreResultsAvailable())
            {
                return null;
            }

            int capacity = beams.Count() * 2 * loadCases.Count();

            HashSet<MemberForces> memberForcesCollection = new HashSet<MemberForces>(capacity);

            object forcesArray = new double[6];

            foreach (int lc in loadCases)
            {
                dynamic lcTitle = openStaadLoad.GetLoadCaseTitle(lc);

                foreach (int b in beams)
                {
                    openStaadOutput.GetMemberEndForces(b, MemberEnd.Start, lc, ref forcesArray, LocalOrGlobal.Local);

                    forcesArray = (double[])forcesArray;

                    _ = memberForcesCollection.Add(new MemberForces()
                    {
                        LoadCaseId = lc,
                        LoadCaseTitle = lcTitle,
                        MemberId = b,
                        End = MemberEnd.Start,
                        Fx = ((double[])forcesArray)[0],
                        Fy = ((double[])forcesArray)[1],
                        Fz = ((double[])forcesArray)[2],
                        Mx = ((double[])forcesArray)[3],
                        My = ((double[])forcesArray)[4],
                        Mz = ((double[])forcesArray)[5]
                    });

                    openStaadOutput.GetMemberEndForces(b, MemberEnd.End, lc, ref forcesArray, LocalOrGlobal.Local);

                    _ = memberForcesCollection.Add(new MemberForces()
                    {
                        LoadCaseId = lc,
                        LoadCaseTitle = lcTitle,
                        MemberId = b,
                        End = MemberEnd.End,
                        Fx = ((double[])forcesArray)[0],
                        Fy = ((double[])forcesArray)[1],
                        Fz = ((double[])forcesArray)[2],
                        Mx = ((double[])forcesArray)[3],
                        My = ((double[])forcesArray)[4],
                        Mz = ((double[])forcesArray)[5]
                    });
                }
            }

            return memberForcesCollection;
        }

        public static List<MemberForces> SummarizeForces(HashSet<MemberForces> memberForcesSet)
        {
            List<MemberForces> summarySet = new List<MemberForces>(12)
            {
                memberForcesSet.OrderByDescending(f => f.Fx).FirstOrDefault(),
                memberForcesSet.OrderBy(f => f.Fx).FirstOrDefault(),
                memberForcesSet.OrderByDescending(f => f.Fy).FirstOrDefault(),
                memberForcesSet.OrderBy(f => f.Fy).FirstOrDefault(),
                memberForcesSet.OrderByDescending(f => f.Fz).FirstOrDefault(),
                memberForcesSet.OrderBy(f => f.Fz).FirstOrDefault(),
                memberForcesSet.OrderByDescending(f => f.Mx).FirstOrDefault(),
                memberForcesSet.OrderBy(f => f.Mx).FirstOrDefault(),
                memberForcesSet.OrderByDescending(f => f.My).FirstOrDefault(),
                memberForcesSet.OrderBy(f => f.My).FirstOrDefault(),
                memberForcesSet.OrderByDescending(f => f.Mz).FirstOrDefault(),
                memberForcesSet.OrderBy(f => f.Mz).FirstOrDefault()
            };

            return summarySet;
        }
    }
}
