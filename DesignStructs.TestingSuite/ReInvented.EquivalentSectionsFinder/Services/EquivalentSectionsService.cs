using System;
using System.Collections.Generic;
using System.Linq;

using ReInvented.EquivalentSectionsFinder.Models;
using ReInvented.Sections.Domain.Interfaces;
using ReInvented.Sections.Domain.Models;
using ReInvented.Sections.Domain.Repositories;

namespace ReInvented.EquivalentSectionsFinder.Services
{
    public sealed class EquivalentSectionsService
    {
        public static IEnumerable<SectionPropertyComparisonResult> GetEquivalentSections(IRolledSection currentSection, IEnumerable<Database> databases, double percentDeviation, double matchProbabilityPercent)
        {
            SectionsLibrary library = SectionsRepository.Instance.GetSectionsLibrary();

            Database currentDb = library.Databases.FirstOrDefault(db => db.SectionShapes.SelectMany(sh => sh.Classifications).SelectMany(c => c.Sections).Contains(currentSection));
            SectionShape currentShape = currentDb.SectionShapes.FirstOrDefault(sh => sh.Classifications.SelectMany(c => c.Sections).Contains(currentSection));

            List<IRolledSection> allSections = databases.SelectMany(db => db.SectionShapes.Where(sh => sh.Shape == currentShape.Shape).SelectMany(sh => sh.Classifications).SelectMany(c => c.Sections)).ToList();

            IEnumerable<SectionPropertyComparisonResult> results = allSections.Select(s => new SectionPropertyComparisonResult(currentSection, s)).Where(r => r.IsEquivalent(percentDeviation, matchProbabilityPercent));

            return results;
        }
    }
}
