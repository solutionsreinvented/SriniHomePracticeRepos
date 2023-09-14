using System.Collections.Generic;
using System.Linq;

using DesignStructs.TestingSuiteUI.Models;

using ReInvented.Sections.Domain.Interfaces;
using ReInvented.Sections.Domain.Models;

namespace DesignStructs.TestingSuiteUI.Services
{
    public class SectionDesignService
    {
        public static void Design(IEnumerable<Classification> classifications, SectionDesignData designData)
        {
            List<IRolledSection> sections = classifications.SelectMany(c => c.Sections).ToList();

        }
    }
}
