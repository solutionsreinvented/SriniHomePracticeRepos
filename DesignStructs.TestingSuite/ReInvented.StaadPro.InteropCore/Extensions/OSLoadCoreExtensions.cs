using System.Collections.Generic;

using ReInvented.StaadPro.Interop.Entities;
using ReInvented.StaadPro.Interop.Enums;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.Interop.Interfaces;
using ReInvented.StaadPro.InteropCore.Models;

namespace ReInvented.StaadPro.InteropCore.Extensions
{
    public static class OSLoadCoreExtensions
    {
        #region Public Fluent Functions

        public static OSLoadCore CreateReferenceLoadCases(this OSLoadCore load, HashSet<ILoadCase> referenceLoads)
        {
            load.ComObject.CreateReferenceLoadCases(referenceLoads);
            return load;
        }

        public static OSLoadCore CreatePrimaryLoadCases(this OSLoadCore load, HashSet<ILoadCase> primaryLoads)
        {
            load.ComObject.CreatePrimaryLoadCases(primaryLoads);
            return load;
        }

        public static OSLoadCore AddSelfweightInXYZTo(this OSLoadCore load, int loadCaseId, IEnumerable<int> entities, SelftWeightDirection loadDirection = SelftWeightDirection.GlobalY, double factor = -1.0)
        {
            load.ComObject.AddSelfweightInXYZTo(loadCaseId, entities, loadDirection, factor);
            return load;
        }

        public static OSLoadCore AddAxialThermalGradientTo(this OSLoadCore load, int loadCaseId, IEnumerable<int> entities, double axialThermalGradient)
        {
            load.ComObject.AddAxialThermalGradientTo(loadCaseId, entities, axialThermalGradient);
            return load;
        }

        public static OSLoadCore AddAxialThermalGradientTo(this OSLoadCore load, int loadCaseId, IEnumerable<int> entities, double axialThermalGradient, int nThreads)
        {
            /// TODO: The COM implementation uses 'ProcessOnMultipleThreads'. Change this to AsParallel();
            load.ComObject.AddAxialThermalGradientTo(loadCaseId, entities, axialThermalGradient, nThreads);
            return load;
        }

        public static OSLoadCore AddReferenceLoadsTo(this OSLoadCore load, int loadCaseId, Dictionary<int, double> loadCaseNumberFactorPairs)
        {
            load.ComObject.AddReferenceLoadsTo(loadCaseId, loadCaseNumberFactorPairs);
            return load;
        }

        public static OSLoadCore AddRepeatLoadsTo(this OSLoadCore load, int loadCaseId, IDictionary<int, double> loadCaseNumberFactorPairs)
        {
            load.ComObject.AddRepeatLoadsTo(loadCaseId, loadCaseNumberFactorPairs);
            return load;
        }

        public static OSLoadCore CreateLoadCombinations(this OSLoadCore load, IEnumerable<EditorLoadCombination> loadCombinations)
        {
            load.ComObject.CreateLoadCombinations(loadCombinations);
            return load;
        }

        public static OSLoadCore CreateLoadEnvelopExt(this OSLoadCore load, int envelopId, LoadEnvelopType envelopType, IEnumerable<int> loadCaseList)
        {
            load.ComObject.CreateLoadEnvelopExt(envelopId, envelopType, loadCaseList);
            return load;
        }

        public static OSLoadCore CreateLoadEnvelopExt(this OSLoadCore load, int envelopId, LoadEnvelopType envelopType, IEnumerable<EditorLoadCombination> loadCombinations)
        {
            load.ComObject.CreateLoadEnvelopExt(envelopId, envelopType, loadCombinations);
            return load;
        }

        #endregion

        #region Public Functions

        public static List<ILoadCase> GetLoadCasesOfType(this OSLoadCore load, LoadCaseType loadCaseType)
        {
            return load.ComObject.GetLoadCasesOfType(loadCaseType);
        }

        public static HashSet<ILoadCase> GetAllPrimaryLoadCases(this OSLoadCore load) => load.ComObject.GetAllPrimaryLoadCases();

        public static HashSet<ILoadCase> GetAllReferenceLoadCases(this OSLoadCore load)
        {
            return load.ComObject.GetAllReferenceLoadCases();
        }

        public static HashSet<ILoadCase> GetAllLoadCombinationCases(this OSLoadCore load)
        {
            return load.ComObject.GetAllLoadCombinationCases();
        }

        public static HashSet<ILoadCase> GetRepeatLoadCases(this OSLoadCore load)
        {
            return load.ComObject.GetRepeatLoadCases();
        }

        public static ILoadCase GetPrimaryLoadCaseFromId(this OSLoadCore load, int lcId)
        {
            return load.ComObject.GetPrimaryLoadCaseFromId(lcId);
        }

        public static ILoadCase GetReferenceLoadCaseFromId(this OSLoadCore load, int lcId)
        {
            return load.ComObject.GetReferenceLoadCaseFromId(lcId);
        }

        public static ILoadCase GetLoadCombinationCaseFromId(this OSLoadCore load, int lcId)
        {
            return load.ComObject.GetLoadCombinationCaseFromId(lcId);
        }

        public static List<ILoadCase> GetPrimaryLoadCasesFromIds(this OSLoadCore load, IEnumerable<int> loadCasesIds)
        {
            return load.ComObject.GetPrimaryLoadCasesFromIds(loadCasesIds);
        }

        public static List<ILoadCase> GetReferenceLoadCasesFromIds(this OSLoadCore load, IEnumerable<int> loadCasesIds)
        {
            return load.ComObject.GetReferenceLoadCasesFromIds(loadCasesIds);
        }

        public static IEnumerable<ILoadCase> GetLoadCombinationCasesFromIds(this OSLoadCore load, IEnumerable<int> lcIds)
        {
            return load.ComObject.GetLoadCombinationCasesFromIds(lcIds);
        }

        public static IEnumerable<ILoadCase> GetAllLoadCasesOfType(this OSLoadCore load, LoadCaseType loadCaseType = LoadCaseType.PrimaryLoad)
        {
            return load.ComObject.GetAllLoadCasesOfType(loadCaseType);
        }

        public static ILoadCase GetLoadCase(this OSLoadCore load, int lcId, LoadCaseType loadCaseType)
        {
            return load.GetLoadCase(lcId, loadCaseType);
        }

        public static IEnumerable<ILoadCase> GetLoadCases(this OSLoadCore load, int fromId, int toId, LoadCaseType loadCaseType)
        {
            return load.GetLoadCases(fromId, toId, loadCaseType);
        }

        public static IEnumerable<ILoadCase> GetLoadCasesFromIds(this OSLoadCore load, IEnumerable<int> loadCasesIds, LoadCaseType loadCaseType)
        {
            return load.GetLoadCasesFromIds(loadCasesIds, loadCaseType);
        }

        #endregion
    }
}
