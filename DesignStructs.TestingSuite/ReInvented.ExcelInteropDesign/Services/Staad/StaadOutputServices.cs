using System.Collections.Generic;
using System.Linq;

using OpenSTAADUI;

using ReInvented.ExcelInteropDesign.Models;
using ReInvented.StaadPro.Interactivity.Entities;
using ReInvented.StaadPro.Interactivity.Enums;
using ReInvented.StaadPro.Interactivity.Models;

namespace ReInvented.ExcelInteropDesign.Services
{
    public class StaadOutputServices
    {

        public static HashSet<MemberForces> SummarizeForces(IOSOutputUI openStaadOutput, IEnumerable<Beam> beams, IEnumerable<int> loadCases)
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

        public static HashSet<MemberForces> SummarizeForces(StaadModelWrapper wrapper, IEnumerable<int> beams, IEnumerable<int> loadCases)
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
    }
}
