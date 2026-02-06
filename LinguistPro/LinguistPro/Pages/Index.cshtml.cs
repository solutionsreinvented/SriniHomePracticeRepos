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

        public List<VocabularyItem> Vocabulary { get; private set; } = new List<VocabularyItem>();
        public List<VerbEntry> VerbList { get; private set; } = new List<VerbEntry>();

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

        /* Edit / Delete bindings for Vocabulary */
        [BindProperty]
        public int? EditVocabId { get; set; }

        [BindProperty]
        public string EditVocabTerm { get; set; } = string.Empty;

        [BindProperty]
        public string EditVocabMeaning { get; set; } = string.Empty;

        [BindProperty]
        public string EditVocabUsage { get; set; } = string.Empty;

        /* Edit / Delete bindings for Verb */
        [BindProperty]
        public int? EditVerbId { get; set; }

        [BindProperty]
        public string EditVerbInfinitive { get; set; } = string.Empty;

        [BindProperty]
        public string EditVerbMeaning { get; set; } = string.Empty;

        [BindProperty]
        public string EditS1 { get; set; } = string.Empty;
        [BindProperty]
        public string EditS2Inf { get; set; } = string.Empty;
        [BindProperty]
        public string EditS2Form { get; set; } = string.Empty;
        [BindProperty]
        public string EditS3 { get; set; } = string.Empty;
        [BindProperty]
        public string EditP1 { get; set; } = string.Empty;
        [BindProperty]
        public string EditP2Inf { get; set; } = string.Empty;
        [BindProperty]
        public string EditP2Form { get; set; } = string.Empty;
        [BindProperty]
        public string EditP3 { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            Vocabulary = await _db.Vocabulary.OrderBy(v => v.Term).ToListAsync();

            VerbList = await _db.Verbs.ToListAsync();

            GlobalMastery = Vocabulary.Count == 0 ? 0 : (int)Vocabulary.Average(v => v.Mastery);
        }

        public async Task<IActionResult> OnPostEditVocabularyAsync()
        {
            if (EditVocabId == null) return RedirectToPage(new { Mode = "Vocab", SelectedLanguage });

            var item = await _db.Vocabulary.FindAsync(EditVocabId.Value);
            if (item != null)
            {
                item.Term = EditVocabTerm;
                item.Meaning = EditVocabMeaning;
                item.UsageExample = EditVocabUsage;
                await _db.SaveChangesAsync();
            }

            return RedirectToPage(new { Mode = "Vocab", SelectedLanguage });
        }

        public async Task<IActionResult> OnPostDeleteVocabularyAsync()
        {
            if (EditVocabId == null) return RedirectToPage(new { Mode = "Vocab", SelectedLanguage });

            var item = await _db.Vocabulary.FindAsync(EditVocabId.Value);
            if (item != null)
            {
                _db.Vocabulary.Remove(item);
                await _db.SaveChangesAsync();
            }

            return RedirectToPage(new { Mode = "Vocab", SelectedLanguage });
        }

        public async Task<IActionResult> OnPostEditVerbAsync()
        {
            if (EditVerbId == null) return RedirectToPage(new { Mode = "Verbs", SelectedLanguage });

            var v = await _db.Verbs.FindAsync(EditVerbId.Value);
            if (v != null)
            {
                v.Infinitive = EditVerbInfinitive;
                v.Meaning = EditVerbMeaning;
                v.S1 = EditS1;
                v.S2Inf = EditS2Inf;
                v.S2Form = EditS2Form;
                v.S3 = EditS3;
                v.P1 = EditP1;
                v.P2Inf = EditP2Inf;
                v.P2Form = EditP2Form;
                v.P3 = EditP3;

                await _db.SaveChangesAsync();
            }

            return RedirectToPage(new { Mode = "Verbs", SelectedLanguage });
        }

        public async Task<IActionResult> OnPostDeleteVerbAsync()
        {
            if (EditVerbId == null) return RedirectToPage(new { Mode = "Verbs", SelectedLanguage });

            var v = await _db.Verbs.FindAsync(EditVerbId.Value);
            if (v != null)
            {
                _db.Verbs.Remove(v);
                await _db.SaveChangesAsync();
            }

            return RedirectToPage(new { Mode = "Verbs", SelectedLanguage });
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
