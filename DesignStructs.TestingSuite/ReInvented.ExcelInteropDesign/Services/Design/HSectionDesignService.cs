using System.Collections.Generic;

using ReInvented.ExcelInteropDesign.Interfaces;
using ReInvented.Sections.Domain.Interfaces;

namespace ReInvented.ExcelInteropDesign.Services
{
    public class HSectionDesignService : ISectionDesignService
    {
        public Dictionary<string, double> DesignBeams(IEnumerable<IRolledSection> sections)
        {
            throw new System.NotImplementedException();
        }

        public Dictionary<string, double> DesignColumns(IEnumerable<IRolledSection> sections)
        {
            throw new System.NotImplementedException();
        }
    }
}
