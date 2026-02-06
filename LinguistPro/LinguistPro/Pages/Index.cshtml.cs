using LinguistPro.Models;
using LinguistPro.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LinguistPro.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly DictionaryService _dictionary;
        private readonly WiktionaryVerbService _verbs;

        public IndexModel(AppDbContext db, DictionaryService dictionary, WiktionaryVerbService verbs)
        {
            _db = db;
            _dictionary = dictionary;
            _verbs = verbs;
        }

        [BindProperty(SupportsGet = true)]
        public string Mode { get; set; } = "Vocab";

        public int GlobalMastery { get; private set; }

        public List<VocabularyItem> Vocabulary { get; private set; } = [];
        public List<VerbEntry> VerbList { get; private set; } = [];

        [BindProperty]
        public string NewWord { get; set; } = string.Empty;

        [BindProperty]
        public string NewVerb { get; set; } = string.Empty;


        [BindProperty(SupportsGet = true)]
        public string SelectedLanguage { get; set; } = "de";

        [BindProperty]
        public bool FetchVocabularyOnline { get; set; }

        [BindProperty]
        public bool FetchVerbOnline { get; set; }

        /* Manual Vocabulary */
        [BindProperty]
        public string ManualTerm { get; set; } = string.Empty;

        [BindProperty]
        public string ManualMeaning { get; set; } = string.Empty;

        [BindProperty]
        public string ManualUsage { get; set; } = string.Empty;

        /* Manual Verb */
        [BindProperty]
        public VerbEntry ManualVerb { get; set; } = new() { Language = "de", Infinitive = string.Empty };

        public async Task OnGetAsync()
        {
            Vocabulary = await _db.Vocabulary.OrderBy(v => v.Term).ToListAsync();

            VerbList = await _db.Verbs.ToListAsync();

            GlobalMastery = Vocabulary.Count == 0 ? 0 : (int)Vocabulary.Average(v => v.Mastery);
        }

        public async Task<IActionResult> OnPostAddVocabularyAsync()
        {
            VocabularyItem? item;

            if (FetchVocabularyOnline)
            {
                item = await _dictionary.FetchAsync(ManualTerm, SelectedLanguage);
            }
            else
            {
                item = new VocabularyItem
                {
                    Language = SelectedLanguage,
                    Term = ManualTerm,
                    Meaning = ManualMeaning,
                    Definition = ManualMeaning,
                    UsageExample = ManualUsage
                };
            }

            if (item != null)
            {
                _db.Vocabulary.Add(item);
                await _db.SaveChangesAsync();
            }

            return RedirectToPage(new { Mode = "Vocab", SelectedLanguage });
        }

        public async Task<IActionResult> OnPostAddVerbAsync()
        {
            VerbEntry? verb;

            if (FetchVerbOnline)
            {
                verb = await _verbs.FetchGermanPresentAsync(ManualVerb.Infinitive);
            }
            else
            {
                verb = ManualVerb;
                verb.Language = SelectedLanguage;
            }

            if (verb != null)
            {
                _db.Verbs.Add(verb);
                await _db.SaveChangesAsync();
            }

            return RedirectToPage(new { Mode = "Verbs", SelectedLanguage });
        }

    }
}
