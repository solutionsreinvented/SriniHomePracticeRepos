using ReInvented.TestingSuite.Base;
using ReInvented.TestingSuite.Interfaces;

namespace ReInvented.TestingSuite.Models
{
    public class HSection : Section, ISection
    {
        private static ISection _instance;

        private HSection()
        {

        }

        public static ISection Instance => _instance ?? (_instance = new HSection());
    }

    public class CSection : Section, ISection
    {
        private static ISection _instance;

        private CSection()
        {

        }

        public static ISection Instance => _instance ?? (_instance = new CSection());
    }

    public class LSection : Section, ISection
    {
        private static ISection _instance;

        private LSection()
        {

        }

        public static ISection Instance => _instance ?? (_instance = new LSection());
    }

    public class OSection : Section, ISection
    {
        private static ISection _instance;

        private OSection()
        {

        }

        public static ISection Instance => _instance ?? (_instance = new OSection());
    }
}
