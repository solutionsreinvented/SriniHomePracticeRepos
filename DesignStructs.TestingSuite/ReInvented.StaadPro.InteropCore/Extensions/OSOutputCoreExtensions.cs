using System.Collections.Generic;

using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Enums;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.Interop.Interfaces;
using ReInvented.StaadPro.Interop.Models;
using ReInvented.StaadPro.InteropCore.Models;

namespace ReInvented.StaadPro.InteropCore.Extensions
{
    public static class OSOutputCoreExtensions
    {
        #region Supports' Results

        public static LoadCaseForces GetSupportForces(this OSOutputCore output, int supportId, int loadCaseId)
        {
            return output.ComObject.GetSupportForces(supportId, loadCaseId);
        }

        public static LoadCaseForces GetSupportReactions(this OSOutputCore output, int supportId, int loadCaseId)
        {
            return output.ComObject.GetSupportReactions(supportId, loadCaseId);
        }

        public static HashSet<StaticCheckResult> GetStaticCheckResults(this OSOutputCore output, IEnumerable<int> loadCaseIds)
        {
            return output.ComObject.GetStaticCheckResults(loadCaseIds);
        }

        #endregion

        #region Member Results

        public static Dictionary<Beam, double> GetMemberSteelUtilizationRatios(this OSOutputCore output, HashSet<Beam> beams)
        {
            return output.ComObject.GetMemberSteelUtilizationRatios(beams);
        }

        public static MemberForces GetMemberEndForces(this OSOutputCore output, string lcTitle, int beamId, int loadCaseId, MemberEnd memberEnd)
        {
            return output.GetMemberEndForces(lcTitle, beamId, loadCaseId, memberEnd);
        }

        #endregion

        #region Plate Results

        public static PlateCenterForces GetPlateCenterForces(this OSOutputCore output, ILoadCase loadCase, Plate plate)
        {
            return output.ComObject.GetPlateCenterForces(loadCase, plate);
        }

        public static PlateCenterMoments GetPlateCenterMoments(this OSOutputCore output, ILoadCase loadCase, Plate plate)
        {
            return output.ComObject.GetPlateCenterMoments(loadCase, plate);
        }

        public static PlateCenterVonMises GetPlateCenterVonMises(this OSOutputCore output, ILoadCase loadCase, Plate plate)
        {
            return output.ComObject.GetPlateCenterVonMises(loadCase, plate);
        }

        public static PlatePrincipalStresses GetPlatePrincipalStresses(this OSOutputCore output, ILoadCase loadCase, Plate plate)
        {
            return output.ComObject.GetPlatePrincipalStresses(loadCase, plate);
        }

        public static PlateCenterResults GetPlateCenterResults(this OSOutputCore output, ILoadCase loadCase, Plate plate)
        {
            //PlateCenterForces pdForces = GetPlateCenterForces(output, loadCase, plate);
            //PlateCenterMoments pdMoments = GetPlateCenterMoments(output, loadCase, plate);
            //PlateCenterVonMises pdVonMises = GetPlateCenterVonMises(output, loadCase, plate);
            //PlatePrincipalStresses pdPrincipal = GetPlatePrincipalStresses(output, loadCase, plate);

            //return PlateCenterResults.Create(pdForces, pdMoments, pdVonMises, pdPrincipal);

            return output.GetPlateCenterResults(loadCase, plate);
        }

        public static IEnumerable<PlateCenterResults> GetPlateCenterResults(this OSOutputCore output, IEnumerable<ILoadCase> loadCases, IEnumerable<Plate> plates)
        {
            return output.ComObject.GetPlateCenterResults(loadCases, plates);
        }

        #endregion
    }
}
