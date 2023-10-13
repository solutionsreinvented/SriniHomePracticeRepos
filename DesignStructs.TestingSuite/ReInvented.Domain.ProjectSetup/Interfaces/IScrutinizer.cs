using System;

namespace ReInvented.Domain.ProjectSetup.Interfaces
{
    public interface IScrutinizer
    {
        string FullName { get; set; }

        string ShortName { get; set; }

        DateTime DateOfScrutiny { get; set; }
    }
}
