using System.Collections.Generic;
using System.Threading.Tasks;

using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Enums;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.Interop.Interfaces;
using ReInvented.StaadPro.Interop.Models;
using ReInvented.StaadPro.InteropCore.Models;

namespace ReInvented.StaadPro.InteropCore.Extensions
{
    public static class OSOutputExtensionsAsync
    {
        #region Support Results

        public static async Task<LoadCaseForces> GetSupportForcesAsync(this OSOutputCore output, int supportId, int loadCaseId)
        {
            return await output.ComObject.GetSupportForcesAsync(supportId, loadCaseId);
        }

        public static async Task<LoadCaseForces> GetSupportReactionsAsync(this OSOutputCore output, int supportId, int loadCaseId)
        {
            return await output.ComObject.GetSupportReactionsAsync(supportId, loadCaseId);
        }

        public static async Task<StaticCheckResult> GetStaticCheckResultAsync(this OSOutputCore output, int loadCaseId)
        {
            return await output.ComObject.GetStaticCheckResultAsync(loadCaseId);
        }

        public static async Task<HashSet<StaticCheckResult>> GetStaticCheckResultsAsync(this OSOutputCore output, IEnumerable<int> loadCaseIds)
        {
            return await output.ComObject.GetStaticCheckResultsAsync(loadCaseIds);
        }

        #endregion

        #region Member Results

        public static async Task<double> GetMemberSteelUtilizationRatioAsync(this OSOutputCore output, int beamId)
        {
            return await output.GetMemberSteelUtilizationRatioAsync(beamId);
        }

        public static async Task<double> GetMemberSteelUtilizationRatioAsync(this OSOutputCore output, Beam b)
        {
            return await output.ComObject.GetMemberSteelUtilizationRatioAsync(b);
        }

        public static async Task<Dictionary<Beam, double>> GetMemberSteelUtilizationRatiosAsync(this OSOutputCore output, HashSet<Beam> beams)
        {
            return await output.ComObject.GetMemberSteelUtilizationRatiosAsync(beams);
        }

        public static async Task<MemberForces> GetMemberEndForcesAsync(this OSOutputCore output, string lcTitle, int beamId, int loadCaseId, MemberEnd memberEnd)
        {
            return await output.ComObject.GetMemberEndForcesAsync(lcTitle, beamId, loadCaseId, memberEnd);
        }

        #endregion

        #region Plate Results

        public static async Task<PlateCenterForces> GetPlateCenterForcesAsync(this OSOutputCore output, ILoadCase loadCase, Plate plate)
        {
            return await output.ComObject.GetPlateCenterForcesAsync(loadCase, plate);
        }

        public static async Task<PlateCenterMoments> GetPlateCenterMomentsAsync(this OSOutputCore output, ILoadCase loadCase, Plate plate)
        {
            return await output.ComObject.GetPlateCenterMomentsAsync(loadCase, plate);
        }

        public static async Task<PlateCenterVonMises> GetPlateCenterVonMisesAsync(this OSOutputCore output, ILoadCase loadCase, Plate plate)
        {
            return await output.ComObject.GetPlateCenterVonMisesAsync(loadCase, plate);
        }

        public static async Task<PlatePrincipalStresses> GetPlatePrincipalStressesAsync(this OSOutputCore output, ILoadCase loadCase, Plate plate)
        {
            return await output.ComObject.GetPlatePrincipalStressesAsync(loadCase, plate);
        }

        public static async Task<PlateCenterResults> GetPlateCenterResultsAsync(this OSOutputCore output, ILoadCase loadCase, Plate plate)
        {
            //PlateCenterForces pdForces = await GetPlateCenterForcesAsync(output, loadCase, plate);
            //PlateCenterMoments pdMoments = await GetPlateCenterMomentsAsync(output, loadCase, plate);
            //PlateCenterVonMises pdVonMises = await GetPlateCenterVonMisesAsync(output, loadCase, plate);
            //PlatePrincipalStresses pdPrincipal = await GetPlatePrincipalStressesAsync(output, loadCase, plate);

            //return PlateCenterResults.Create(pdForces, pdMoments, pdVonMises, pdPrincipal);

            return await output.ComObject.GetPlateCenterResultsAsync(loadCase, plate);
        }

        public static async Task<IEnumerable<PlateCenterResults>> GetPlateCenterResultsAsync(this OSOutputCore output, IEnumerable<ILoadCase> loadCases, IEnumerable<Plate> plates)
        {
            return await output.ComObject.GetPlateCenterResultsAsync(loadCases, plates);
        }

        #endregion
    }
}
