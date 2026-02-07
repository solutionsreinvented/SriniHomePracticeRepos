using LinguistPro.Models;
using LinguistPro.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LinguistPro.Pages
{
    [Authorize]
    public class SearchModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SearchFilterService _searchFilterService;

        public SearchModel(UserManager<ApplicationUser> userManager, SearchFilterService searchFilterService)
        {
            _userManager = userManager;
            _searchFilterService = searchFilterService;
        }

        public SearchFilterCriteria? Criteria { get; set; }
        public SearchFilterResult<VocabularyItem>? Results { get; set; }

        public async Task OnGetAsync(
            string? searchTerm,
            int masteryMin = 0,
            int masteryMax = 100,
            string? itemType = null,
            bool showOnlyUnmastered = false,
            bool showOnlyDueForReview = false,
            string sortBy = "term",
            string sortDirection = "asc",
            int pageNumber = 1,
            int pageSize = 20,
            int? languageProfileId = null)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return;

            // Build criteria
            Criteria = new SearchFilterCriteria
            {
                SearchTerm = searchTerm,
                MasteryMinimum = masteryMin > 0 ? masteryMin : null,
                MasteryMaximum = masteryMax < 100 ? masteryMax : null,
                ItemType = itemType,
                ShowOnlyUnmastered = showOnlyUnmastered,
                ShowOnlyDueForReview = showOnlyDueForReview,
                SortBy = sortBy,
                SortDirection = sortDirection,
                PageNumber = pageNumber,
                PageSize = pageSize,
                LanguageProfileId = languageProfileId ?? 0
            };

            // If no language profile specified, use the first active one
            if (Criteria.LanguageProfileId == 0)
            {
                // This would need to be fetched from user's active language
                // For now, set to a default or get from query
            }

            // Execute search
            try
            {
                Results = await _searchFilterService.SearchVocabularyAsync(Criteria);
            }
            catch
            {
                Results = new SearchFilterResult<VocabularyItem>();
            }
        }
    }
}
