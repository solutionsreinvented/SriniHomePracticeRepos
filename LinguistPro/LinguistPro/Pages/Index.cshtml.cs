using LinguistPro.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LinguistPro.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;
        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string Mode { get; set; } = "Vocab";

        public List<VocabularyItem> Vocabulary { get; set; }
        public List<VerbEntity> Verbs { get; set; }
        public double GlobalMastery { get; set; }

        public async Task OnGetAsync()
        {
            await SeedGermanData(); // Initialize with German content
            Vocabulary = await _context.Vocabulary.ToListAsync();
            Verbs = await _context.Verbs.ToListAsync();
            GlobalMastery = Vocabulary.Any() ? Vocabulary.Average(v => v.MasteryPoints) : 0;
        }

        public async Task<IActionResult> OnPostLogProgressAsync(int id)
        {
            var item = await _context.Vocabulary.FindAsync(id);
            if (item != null)
            {
                item.MasteryPoints = Math.Min(item.MasteryPoints + 15, 100);
                item.LastInteraction = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            return RedirectToPage(new { Mode = "Vocab" });
        }

        private async Task SeedGermanData()
        {
            if (!await _context.Vocabulary.AnyAsync())
            {
                _context.Vocabulary.Add(new VocabularyItem
                {
                    SourceWord = "Zeit",
                    TargetMeaning = "Time",
                    Definition = "Indefinite continued progress of existence.",
                    UsageExample = "Die Zeit vergeht wie im Flug.",
                    MasteryPoints = 10
                });
                _context.Verbs.Add(new VerbEntity
                {
                    Infinitive = "haben",
                    Meaning = "to have",
                    S1 = "habe",
                    P1 = "haben",
                    S2 = "hast",
                    P2 = "habt",
                    S2F = "haben",
                    P2F = "haben",
                    S3 = "hat",
                    P3 = "haben"
                });
                await _context.SaveChangesAsync();
            }
        }
    }
}
