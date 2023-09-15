using System.Collections.Generic;

using ReInvented.Sections.Domain.Interfaces;

namespace ReInvented.ExcelInteropDesign.Interfaces
{
    public interface ISectionDesignService
    {
        Dictionary<string, double> DesignBeams(IEnumerable<IRolledSection> sections);

        Dictionary<string, double> DesignColumns(IEnumerable<IRolledSection> sections);
    }
}
