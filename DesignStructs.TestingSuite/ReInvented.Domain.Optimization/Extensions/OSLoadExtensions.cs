using System.Collections.Generic;

using OpenSTAADUI;

using ReInvented.Shared.Interfaces;
using ReInvented.StaadPro.Interop.Base;
using ReInvented.StaadPro.Interop.Enums;
using ReInvented.StaadPro.Interop.Extensions;
using ReInvented.StaadPro.Interop.Interfaces;

namespace ReInvented.Domain.Optimization.Extensions
{
    public class StaadLoadCombination : LoadCombination, ILoadCombination, IEntity
    {
        #region Default Constructor

        public StaadLoadCombination(int id, string title) : base(id, title)
        {
            Factors = new Dictionary<ILoadCase, double>();
        }

        #endregion

        #region Public Properties

        public IDictionary<ILoadCase, double> Factors { get; set; }

        #endregion
    }

    public static class OSLoadExtensions
    {
        public static StaadLoadCombination GetLoadCombination(this OSLoadUI load, int id)
        {
            int pairCount = load.GetNoOfLoadAndFactorPairsForCombination(id);

            if (pairCount > 0)
            {
                string title = load.GetLoadCaseTitle(id);
                StaadLoadCombination loadCombination = new StaadLoadCombination(id, title);

                object objLoadNos = new int[pairCount];
                object objLoadFactors = new double[pairCount];

                load.GetLoadAndFactorForCombination(id, ref objLoadNos, ref objLoadFactors);

                int[] loadNos = (int[])objLoadNos;
                double[] loadFactors = (double[])objLoadFactors;

                for (int i = 0; i < loadNos.Length; i++)
                {
                    ILoadCase lc = load.GetLoadCase(loadNos[i], LoadCaseType.PrimaryLoad);
                    loadCombination.Factors.Add(lc, loadFactors[i]);
                }

                return loadCombination;
            }

            return null;
        }
    }
}
