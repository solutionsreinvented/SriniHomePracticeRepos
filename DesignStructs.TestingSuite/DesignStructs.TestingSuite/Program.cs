using System;

using ReInvented.TestingSuite.Interfaces;
using ReInvented.TestingSuite.Models;

namespace ReInvented.TestingSuite
{
    class Program
    {
        static void Main(string[] args)
        {
            ///DerivedClassSingletonVerification();
            StaadQuery.Run();
            Console.ReadLine();
        }

 

        #region Previous

        private static void DerivedClassSingletonVerification()
        {
            ISection h1 = SectionFactory.Get<HSection>();
            ISection c1 = SectionFactory.Get<CSection>();
            ISection l1 = SectionFactory.Get<LSection>();
            ISection o1 = SectionFactory.Get<OSection>();

            ISection h2 = SectionFactory.Get<HSection>();
            ISection c2 = SectionFactory.Get<CSection>();
            ISection l2 = SectionFactory.Get<LSection>();
            ISection o2 = SectionFactory.Get<OSection>();

            Console.WriteLine($"{nameof(h1)} and {nameof(h2)} are {AreEqualResult(h1, h2)}equal.");
            Console.WriteLine($"{nameof(h1)} and {nameof(c1)} are {AreEqualResult(h1, c1)}equal.");

            Console.WriteLine($"{nameof(c1)} and {nameof(c2)} are {AreEqualResult(c1, c2)}equal.");
            Console.WriteLine($"{nameof(c1)} and {nameof(l1)} are {AreEqualResult(c1, l1)}equal.");

            Console.WriteLine($"{nameof(l1)} and {nameof(l2)} are {AreEqualResult(l1, l2)}equal.");
            Console.WriteLine($"{nameof(l1)} and {nameof(o1)} are {AreEqualResult(l1, o1)}equal.");

            Console.WriteLine($"{nameof(o1)} and {nameof(o2)} are {AreEqualResult(o1, o2)}equal.");
            Console.WriteLine($"{nameof(o1)} and {nameof(h1)} are {AreEqualResult(o1, h1)}equal.");
        }

        private static string AreEqualResult(ISection left, ISection right) => left == right ? string.Empty : "not ";
    }

    class SectionFactory
    {
        public static ISection Get<TSection>() where TSection : ISection
        {
            ISection section;

            if (typeof(TSection) == typeof(HSection))
            {
                section = HSection.Instance;
            }
            else if (typeof(TSection) == typeof(CSection))
            {
                section = CSection.Instance;
            }
            else if (typeof(TSection) == typeof(LSection))
            {
                section = LSection.Instance;
            }
            else
            {
                section = OSection.Instance;
            }

            return section;
        }
    } 

    #endregion
}

