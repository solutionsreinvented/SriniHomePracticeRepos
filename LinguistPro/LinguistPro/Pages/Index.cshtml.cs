using LinguistPro.Models;
using LinguistPro.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LinguistPro.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly DictionaryService _dictionary;
        private readonly WiktionaryVerbService _verbs;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly LearningStreakService _streakService;

        public IndexModel(AppDbContext db, DictionaryService dictionary, WiktionaryVerbService verbs, UserManager<ApplicationUser> userManager, LearningStreakService streakService)
        {
            _db = db;
            _dictionary = dictionary;
            _verbs = verbs;
            _userManager = userManager;
            _streakService = streakService;
        }

        private int? _currentUserId;
        private LanguageProfile? _currentLanguageProfile;

        private async Task<int> GetCurrentUserId()
        {
            if (_currentUserId == null)
            {
                var user = await _userManager.GetUserAsync(User);
                _currentUserId = user?.Id ?? 0;
            }
            return _currentUserId.Value;
        }

        /// <summary>
        /// Check if current user is authenticated as admin
        /// </summary>
        private bool IsAdminUser()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("AdminUser"));
        }

        private async Task<LanguageProfile?> GetCurrentLanguageProfile()
        {
            if (_currentLanguageProfile == null)
            {
                var userId = await GetCurrentUserId();
                _currentLanguageProfile = await _db.LanguageProfiles
                    .FirstOrDefaultAsync(l => l.UserId == userId && l.LanguageCode == SelectedLanguage);
            }
            return _currentLanguageProfile;
        }

        [BindProperty(SupportsGet = true)]
        public string Mode { get; set; } = "Vocab";

        public string CurrentUserFullName { get; set; } = string.Empty;

        // Available languages - easily extensible for future additions
        public static readonly Dictionary<string, string> AvailableLanguages = new()
        {
            { "de", "German" },
            { "fr", "French" },
            { "es", "Spanish" },
            { "ru", "Russian" },
            { "ko", "Korean" }
        };

        public int GlobalMastery { get; private set; }

        // Learning streak data
        public StreakStatistics? CurrentStreakStats { get; private set; }

        public List<VocabularyItem> Vocabulary { get; private set; } = new List<VocabularyItem>();
        public List<VerbEntry> VerbList { get; private set; } = new List<VerbEntry>();
        public List<LanguageItem> Numbers { get; private set; } = new List<LanguageItem>();
        public List<LanguageItem> Months { get; private set; } = new List<LanguageItem>();
        public List<LanguageItem> Days { get; private set; } = new List<LanguageItem>();

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

        [BindProperty]
        public string ManualUsageMeaning { get; set; } = string.Empty;

        /* Manual Language Item (Numbers, Months, Days) */
        [BindProperty]
        public string ManualItemType { get; set; } = "Number";

        [BindProperty]
        public string ManualItemTerm { get; set; } = string.Empty;

        [BindProperty]
        public string ManualItemMeaning { get; set; } = string.Empty;

        [BindProperty]
        public string ManualItemUsage { get; set; } = string.Empty;

        [BindProperty]
        public string ManualItemUsageMeaning { get; set; } = string.Empty;

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

        [BindProperty]
        public string EditVocabUsageMeaning { get; set; } = string.Empty;

        /* Edit / Delete bindings for Language Items */
        [BindProperty]
        public int? EditItemId { get; set; }

        [BindProperty]
        public string EditItemType { get; set; } = string.Empty;

        [BindProperty]
        public string EditItemTerm { get; set; } = string.Empty;

        [BindProperty]
        public string EditItemMeaning { get; set; } = string.Empty;

        [BindProperty]
        public string EditItemUsage { get; set; } = string.Empty;

        [BindProperty]
        public string EditItemUsageMeaning { get; set; } = string.Empty;

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

        [BindProperty]
        public List<int> selectedIds { get; set; } = new();

        public async Task OnGetAsync()
        {
            // Get current user and their language profile
            var userId = await GetCurrentUserId();

            // Get and set current user's full name
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                CurrentUserFullName = $"{user.FirstName} {user.LastName}".Trim();
            }

            var langProfile = await GetCurrentLanguageProfile();

            // If user has no language profile for selected language, create it or fallback to German
            if (langProfile == null)
            {
                // Check if the selected language is valid
                if (!AvailableLanguages.ContainsKey(SelectedLanguage))
                {
                    SelectedLanguage = "de"; // Default to German if invalid
                }

                // Try to get or create the language profile
                langProfile = await _db.LanguageProfiles
                    .FirstOrDefaultAsync(l => l.UserId == userId && l.LanguageCode == SelectedLanguage);

                // If still null, create a new profile for this language
                if (langProfile == null && AvailableLanguages.ContainsKey(SelectedLanguage))
                {
                    langProfile = new LanguageProfile
                    {
                        UserId = userId,
                        LanguageCode = SelectedLanguage,
                        LanguageName = AvailableLanguages[SelectedLanguage],
                        IsActive = true,
                        CreatedDate = DateTime.UtcNow,
                        MasteryLevel = 0
                    };
                    _db.LanguageProfiles.Add(langProfile);
                    await _db.SaveChangesAsync();
                }
            }

            var langProfileId = langProfile?.LanguageProfileId;

            // Load learning streak for current language
            if (langProfileId.HasValue)
            {
                CurrentStreakStats = await _streakService.GetStreakStatisticsAsync(langProfileId.Value);
            }

            // Load vocabulary for current user's language profile
            Vocabulary = await _db.Vocabulary
                .Where(v => v.LanguageProfileId == langProfileId)
                .OrderBy(v => v.Term)
                .ToListAsync();

            // Load verbs for current user's language profile
            VerbList = await _db.Verbs
                .Where(v => v.LanguageProfileId == langProfileId)
                .ToListAsync();

            // Load numbers for current user's language profile
            Numbers = await _db.LanguageItems
                .Where(x => x.ItemType == "Number" && x.LanguageProfileId == langProfileId)
                .ToListAsync();
            // Sort by numeric value - maintain proper ordering
            Numbers = Numbers.OrderBy(n => GetNumericValue(n.Meaning)).ToList();

            // Load days for current user's language profile
            Days = await _db.LanguageItems
                .Where(x => x.ItemType == "Day" && x.LanguageProfileId == langProfileId)
                .ToListAsync();
            // Sort by day of week order
            var dayOrder = new[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            Days = Days.OrderBy(d => 
            {
                var index = Array.FindIndex(dayOrder, day => day.Equals(d.Term, StringComparison.OrdinalIgnoreCase));
                return index >= 0 ? index : int.MaxValue;
            }).ToList();

            // Load months for current user's language profile
            Months = await _db.LanguageItems
                .Where(x => x.ItemType == "Month" && x.LanguageProfileId == langProfileId)
                .ToListAsync();
            // Sort by calendar month order
            var monthOrder = new[] { "January", "February", "March", "April", "May", "June", 
                                    "July", "August", "September", "October", "November", "December" };
            Months = Months.OrderBy(m => 
            {
                var index = Array.FindIndex(monthOrder, month => month.Equals(m.Term, StringComparison.OrdinalIgnoreCase));
                return index >= 0 ? index : int.MaxValue;
            }).ToList();

            GlobalMastery = Vocabulary.Count == 0 ? 0 : (int)Vocabulary.Average(v => v.Mastery);
        }

        public async Task<IActionResult> OnPostEditVocabularyAsync()
        {
            if (EditVocabId == null) return RedirectToPage(new { Mode = "Vocab", SelectedLanguage });

            // Get current user's language profile
            var userId = await GetCurrentUserId();
            var langProfile = await _db.LanguageProfiles
                .FirstOrDefaultAsync(l => l.UserId == userId && l.LanguageCode == SelectedLanguage);

            if (langProfile == null)
                return RedirectToPage(new { Mode = "Vocab", SelectedLanguage });

            // Fetch the item with user isolation check
            var item = await _db.Vocabulary
                .FirstOrDefaultAsync(v => v.Id == EditVocabId.Value && v.LanguageProfileId == langProfile.LanguageProfileId);

            if (item != null)
            {
                item.Term = EditVocabTerm ?? string.Empty;
                item.Meaning = EditVocabMeaning ?? string.Empty;
                item.UsageExample = EditVocabUsage ?? string.Empty;
                item.UsageExampleMeaning = EditVocabUsageMeaning ?? string.Empty;
                await _db.SaveChangesAsync();
            }

            return RedirectToPage(new { Mode = "Vocab", SelectedLanguage });
        }

        public async Task<IActionResult> OnPostDeleteVocabularyAsync()
        {
            if (EditVocabId == null) return RedirectToPage(new { Mode = "Vocab", SelectedLanguage });

            // Get current user's language profile
            var userId = await GetCurrentUserId();
            var langProfile = await _db.LanguageProfiles
                .FirstOrDefaultAsync(l => l.UserId == userId && l.LanguageCode == SelectedLanguage);

            if (langProfile == null)
                return RedirectToPage(new { Mode = "Vocab", SelectedLanguage });

            // Fetch the item with user isolation check
            var item = await _db.Vocabulary
                .FirstOrDefaultAsync(v => v.Id == EditVocabId.Value && v.LanguageProfileId == langProfile.LanguageProfileId);

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

            // Get current user's language profile
            var userId = await GetCurrentUserId();
            var langProfile = await _db.LanguageProfiles
                .FirstOrDefaultAsync(l => l.UserId == userId && l.LanguageCode == SelectedLanguage);

            if (langProfile == null)
                return RedirectToPage(new { Mode = "Verbs", SelectedLanguage });

            // Fetch the item with user isolation check
            var v = await _db.Verbs
                .FirstOrDefaultAsync(v => v.Id == EditVerbId.Value && v.LanguageProfileId == langProfile.LanguageProfileId);

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

            // Get current user's language profile
            var userId = await GetCurrentUserId();
            var langProfile = await _db.LanguageProfiles
                .FirstOrDefaultAsync(l => l.UserId == userId && l.LanguageCode == SelectedLanguage);

            if (langProfile == null)
                return RedirectToPage(new { Mode = "Verbs", SelectedLanguage });

            // Fetch the item with user isolation check
            var v = await _db.Verbs
                .FirstOrDefaultAsync(v => v.Id == EditVerbId.Value && v.LanguageProfileId == langProfile.LanguageProfileId);

            if (v != null)
            {
                _db.Verbs.Remove(v);
                await _db.SaveChangesAsync();
            }

            return RedirectToPage(new { Mode = "Verbs", SelectedLanguage });
        }

        public async Task<IActionResult> OnPostAddVocabularyAsync()
        {
            // Get current user's language profile
            var userId = await GetCurrentUserId();
            var langProfile = await _db.LanguageProfiles
                .FirstOrDefaultAsync(l => l.UserId == userId && l.LanguageCode == SelectedLanguage);

            if (langProfile == null)
                return RedirectToPage(new { Mode = "Vocab", SelectedLanguage });

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
                    Meaning = ManualMeaning ?? string.Empty,
                    Definition = ManualMeaning ?? string.Empty,
                    UsageExample = ManualUsage ?? string.Empty,
                    UsageExampleMeaning = ManualUsageMeaning ?? string.Empty,
                    LanguageProfileId = langProfile.LanguageProfileId
                };
            }

            if (item != null)
            {
                item.UsageExample = item.UsageExample ?? string.Empty;
                item.UsageExampleMeaning = item.UsageExampleMeaning ?? string.Empty;
                item.LanguageProfileId = langProfile.LanguageProfileId;
                _db.Vocabulary.Add(item);
                await _db.SaveChangesAsync();
            }

            return RedirectToPage(new { Mode = "Vocab", SelectedLanguage });
        }

        public async Task<IActionResult> OnPostAddVerbAsync()
        {
            // Get current user's language profile
            var userId = await GetCurrentUserId();
            var langProfile = await _db.LanguageProfiles
                .FirstOrDefaultAsync(l => l.UserId == userId && l.LanguageCode == SelectedLanguage);

            if (langProfile == null)
                return RedirectToPage(new { Mode = "Verbs", SelectedLanguage });

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
                verb.LanguageProfileId = langProfile.LanguageProfileId;
                _db.Verbs.Add(verb);
                await _db.SaveChangesAsync();
            }

            return RedirectToPage(new { Mode = "Verbs", SelectedLanguage });
        }

        public async Task<IActionResult> OnPostAddLanguageItemAsync()
        {
            // Get current user's language profile
            var userId = await GetCurrentUserId();
            var langProfile = await _db.LanguageProfiles
                .FirstOrDefaultAsync(l => l.UserId == userId && l.LanguageCode == SelectedLanguage);

            if (langProfile == null)
                return RedirectToPage(new { Mode = ManualItemType, SelectedLanguage });

            var item = new LanguageItem
            {
                Language = SelectedLanguage,
                ItemType = ManualItemType,
                Term = ManualItemTerm,
                Meaning = ManualItemMeaning ?? string.Empty,
                UsageExample = ManualItemUsage ?? string.Empty,
                UsageExampleMeaning = ManualItemUsageMeaning ?? string.Empty,
                LanguageProfileId = langProfile.LanguageProfileId
            };

            _db.LanguageItems.Add(item);
            await _db.SaveChangesAsync();

            return RedirectToPage(new { Mode = ManualItemType, SelectedLanguage });
        }

        public async Task<IActionResult> OnPostEditLanguageItemAsync()
        {
            if (EditItemId == null) return RedirectToPage(new { Mode = EditItemType, SelectedLanguage });

            // Get current user's language profile
            var userId = await GetCurrentUserId();
            var langProfile = await _db.LanguageProfiles
                .FirstOrDefaultAsync(l => l.UserId == userId && l.LanguageCode == SelectedLanguage);

            if (langProfile == null)
                return RedirectToPage(new { Mode = EditItemType, SelectedLanguage });

            // Fetch the item with user isolation check
            var item = await _db.LanguageItems
                .FirstOrDefaultAsync(i => i.Id == EditItemId.Value && i.LanguageProfileId == langProfile.LanguageProfileId);

            if (item != null)
            {
                item.Term = EditItemTerm ?? string.Empty;
                item.Meaning = EditItemMeaning ?? string.Empty;
                item.UsageExample = EditItemUsage ?? string.Empty;
                item.UsageExampleMeaning = EditItemUsageMeaning ?? string.Empty;
                await _db.SaveChangesAsync();
            }

            return RedirectToPage(new { Mode = EditItemType, SelectedLanguage });
        }

        public async Task<IActionResult> OnPostDeleteLanguageItemAsync()
        {
            if (EditItemId == null) return RedirectToPage(new { Mode = EditItemType, SelectedLanguage });

            // Get current user's language profile
            var userId = await GetCurrentUserId();
            var langProfile = await _db.LanguageProfiles
                .FirstOrDefaultAsync(l => l.UserId == userId && l.LanguageCode == SelectedLanguage);

            if (langProfile == null)
                return RedirectToPage(new { Mode = EditItemType, SelectedLanguage });

            // Fetch the item with user isolation check
            var item = await _db.LanguageItems
                .FirstOrDefaultAsync(i => i.Id == EditItemId.Value && i.LanguageProfileId == langProfile.LanguageProfileId);

            if (item != null)
            {
                _db.LanguageItems.Remove(item);
                await _db.SaveChangesAsync();
            }

            return RedirectToPage(new { Mode = EditItemType, SelectedLanguage });
        }

        // Helper method to convert English number names to numeric values for proper sorting
        private int GetNumericValue(string numberText)
        {
            var numberMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                { "zero", 0 }, { "one", 1 }, { "two", 2 }, { "three", 3 }, { "four", 4 },
                { "five", 5 }, { "six", 6 }, { "seven", 7 }, { "eight", 8 }, { "nine", 9 },
                { "ten", 10 }, { "eleven", 11 }, { "twelve", 12 }, { "thirteen", 13 }, { "fourteen", 14 },
                { "fifteen", 15 }, { "sixteen", 16 }, { "seventeen", 17 }, { "eighteen", 18 }, { "nineteen", 19 },
                { "twenty", 20 }, { "twenty one", 21 }, { "twenty two", 22 }, { "twenty three", 23 }, { "twenty four", 24 },
                { "twenty five", 25 }, { "twenty six", 26 }, { "twenty seven", 27 }, { "twenty eight", 28 }, { "twenty nine", 29 },
                { "thirty", 30 }, { "thirty one", 31 }, { "thirty two", 32 }, { "thirty three", 33 }, { "thirty four", 34 },
                { "thirty five", 35 }, { "thirty six", 36 }, { "thirty seven", 37 }, { "thirty eight", 38 }, { "thirty nine", 39 },
                { "forty", 40 }, { "forty one", 41 }, { "forty two", 42 }, { "forty three", 43 }, { "forty four", 44 },
                { "forty five", 45 }, { "forty six", 46 }, { "forty seven", 47 }, { "forty eight", 48 }, { "forty nine", 49 },
                { "fifty", 50 }, { "fifty one", 51 }, { "fifty two", 52 }, { "fifty three", 53 }, { "fifty four", 54 },
                { "fifty five", 55 }, { "fifty six", 56 }, { "fifty seven", 57 }, { "fifty eight", 58 }, { "fifty nine", 59 },
                { "sixty", 60 }, { "sixty one", 61 }, { "sixty two", 62 }, { "sixty three", 63 }, { "sixty four", 64 },
                { "sixty five", 65 }, { "sixty six", 66 }, { "sixty seven", 67 }, { "sixty eight", 68 }, { "sixty nine", 69 },
                { "seventy", 70 }, { "seventy one", 71 }, { "seventy two", 72 }, { "seventy three", 73 }, { "seventy four", 74 },
                { "seventy five", 75 }, { "seventy six", 76 }, { "seventy seven", 77 }, { "seventy eight", 78 }, { "seventy nine", 79 },
                { "eighty", 80 }, { "eighty one", 81 }, { "eighty two", 82 }, { "eighty three", 83 }, { "eighty four", 84 },
                { "eighty five", 85 }, { "eighty six", 86 }, { "eighty seven", 87 }, { "eighty eight", 88 }, { "eighty nine", 89 },
                { "ninety", 90 }, { "ninety one", 91 }, { "ninety two", 92 }, { "ninety three", 93 }, { "ninety four", 94 },
                { "ninety five", 95 }, { "ninety six", 96 }, { "ninety seven", 97 }, { "ninety eight", 98 }, { "ninety nine", 99 },
                { "hundred", 100 }, { "one hundred", 100 }, { "two hundred", 200 }, { "three hundred", 300 }, { "four hundred", 400 },
                { "five hundred", 500 }, { "six hundred", 600 }, { "seven hundred", 700 }, { "eight hundred", 800 }, { "nine hundred", 900 },
                { "thousand", 1000 }, { "one thousand", 1000 }, { "million", 1000000 }, { "one million", 1000000 }
            };

            if (numberMap.TryGetValue(numberText?.Trim() ?? "", out int value))
            {
                return value;
            }
            return int.MaxValue; // Unknown numbers go to the end
        }

        /// <summary>
        /// Bulk delete vocabulary items (admin only)
        /// </summary>
        public async Task<IActionResult> OnPostBulkDeleteVocabularyAsync()
        {
            // Security: Only admin can bulk delete
            if (!IsAdminUser())
            {
                return Unauthorized();
            }

            if (selectedIds == null || selectedIds.Count == 0)
                return RedirectToPage(new { Mode = "Vocab", SelectedLanguage });

            var userId = await GetCurrentUserId();
            var langProfile = await _db.LanguageProfiles
                .FirstOrDefaultAsync(l => l.UserId == userId && l.LanguageCode == SelectedLanguage);

            if (langProfile == null)
                return RedirectToPage(new { Mode = "Vocab", SelectedLanguage });

            // Delete only items belonging to current user's language profile
            var itemsToDelete = await _db.Vocabulary
                .Where(v => selectedIds.Contains(v.Id) && v.LanguageProfileId == langProfile.LanguageProfileId)
                .ToListAsync();

            _db.Vocabulary.RemoveRange(itemsToDelete);
            await _db.SaveChangesAsync();

            return RedirectToPage(new { Mode = "Vocab", SelectedLanguage });
        }

        /// <summary>
        /// Bulk delete verbs (admin only)
        /// </summary>
        public async Task<IActionResult> OnPostBulkDeleteVerbsAsync()
        {
            // Security: Only admin can bulk delete
            if (!IsAdminUser())
            {
                return Unauthorized();
            }

            if (selectedIds == null || selectedIds.Count == 0)
                return RedirectToPage(new { Mode = "Verbs", SelectedLanguage });

            var userId = await GetCurrentUserId();
            var langProfile = await _db.LanguageProfiles
                .FirstOrDefaultAsync(l => l.UserId == userId && l.LanguageCode == SelectedLanguage);

            if (langProfile == null)
                return RedirectToPage(new { Mode = "Verbs", SelectedLanguage });

            // Delete only items belonging to current user's language profile
            var itemsToDelete = await _db.Verbs
                .Where(v => selectedIds.Contains(v.Id) && v.LanguageProfileId == langProfile.LanguageProfileId)
                .ToListAsync();

            _db.Verbs.RemoveRange(itemsToDelete);
            await _db.SaveChangesAsync();

            return RedirectToPage(new { Mode = "Verbs", SelectedLanguage });
        }

        /// <summary>
        /// Bulk delete language items - Numbers/Days/Months (admin only)
        /// </summary>
        public async Task<IActionResult> OnPostBulkDeleteLanguageItemsAsync()
        {
            // Security: Only admin can bulk delete
            if (!IsAdminUser())
            {
                return Unauthorized();
            }

            if (selectedIds == null || selectedIds.Count == 0)
                return RedirectToPage(new { Mode, SelectedLanguage });

            var userId = await GetCurrentUserId();
            var langProfile = await _db.LanguageProfiles
                .FirstOrDefaultAsync(l => l.UserId == userId && l.LanguageCode == SelectedLanguage);

            if (langProfile == null)
                return RedirectToPage(new { Mode, SelectedLanguage });

            // Delete only items belonging to current user's language profile
            var itemsToDelete = await _db.LanguageItems
                .Where(i => selectedIds.Contains(i.Id) && i.LanguageProfileId == langProfile.LanguageProfileId)
                .ToListAsync();

            _db.LanguageItems.RemoveRange(itemsToDelete);
            await _db.SaveChangesAsync();

            return RedirectToPage(new { Mode, SelectedLanguage });
        }
    }
}
