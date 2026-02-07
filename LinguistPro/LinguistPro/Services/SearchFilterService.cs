using LinguistPro.Models;
using Microsoft.EntityFrameworkCore;

namespace LinguistPro.Services
{
    /// <summary>
    /// Service for advanced search and filtering of vocabulary items
    /// </summary>
    public class SearchFilterService
    {
        private readonly AppDbContext _context;

        public SearchFilterService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Search and filter vocabulary items
        /// </summary>
        public async Task<SearchFilterResult<VocabularyItem>> SearchVocabularyAsync(SearchFilterCriteria criteria)
        {
            var query = _context.Vocabulary
                .Where(v => v.LanguageProfileId == criteria.LanguageProfileId)
                .AsQueryable();

            // Search term
            if (!string.IsNullOrWhiteSpace(criteria.SearchTerm))
            {
                var searchLower = criteria.SearchTerm.ToLower();
                query = query.Where(v => v.Term.ToLower().Contains(searchLower) ||
                                       v.Meaning.ToLower().Contains(searchLower));
            }

            // Mastery filter
            if (criteria.MasteryMinimum.HasValue)
                query = query.Where(v => v.Mastery >= criteria.MasteryMinimum.Value);

            if (criteria.MasteryMaximum.HasValue)
                query = query.Where(v => v.Mastery <= criteria.MasteryMaximum.Value);

            // Unmastered filter
            if (criteria.ShowOnlyUnmastered == true)
                query = query.Where(v => v.Mastery < 90);

            // Last reviewed filter
            if (criteria.LastReviewedFrom.HasValue)
                query = query.Where(v => v.LastReviewed >= criteria.LastReviewedFrom.Value);

            if (criteria.LastReviewedTo.HasValue)
                query = query.Where(v => v.LastReviewed <= criteria.LastReviewedTo.Value);

            // Sort
            query = ApplySorting(query, criteria.SortBy, criteria.SortDirection);

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Pagination
            var skip = (criteria.PageNumber - 1) * criteria.PageSize;
            var items = await query
                .Skip(skip)
                .Take(criteria.PageSize)
                .ToListAsync();

            return new SearchFilterResult<VocabularyItem>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = criteria.PageNumber,
                PageSize = criteria.PageSize
            };
        }

        /// <summary>
        /// Search and filter language items
        /// </summary>
        public async Task<SearchFilterResult<LanguageItem>> SearchLanguageItemsAsync(SearchFilterCriteria criteria)
        {
            var query = _context.LanguageItems
                .Where(l => l.LanguageProfileId == criteria.LanguageProfileId)
                .AsQueryable();

            // Search term
            if (!string.IsNullOrWhiteSpace(criteria.SearchTerm))
            {
                var searchLower = criteria.SearchTerm.ToLower();
                query = query.Where(l => l.Term.ToLower().Contains(searchLower) ||
                                       l.Meaning.ToLower().Contains(searchLower));
            }

            // Item type filter
            if (!string.IsNullOrWhiteSpace(criteria.ItemType))
                query = query.Where(l => l.ItemType == criteria.ItemType);

            // Mastery filter
            if (criteria.MasteryMinimum.HasValue)
                query = query.Where(l => l.Mastery >= criteria.MasteryMinimum.Value);

            if (criteria.MasteryMaximum.HasValue)
                query = query.Where(l => l.Mastery <= criteria.MasteryMaximum.Value);

            // Unmastered filter
            if (criteria.ShowOnlyUnmastered == true)
                query = query.Where(l => l.Mastery < 90);

            // Last reviewed filter
            if (criteria.LastReviewedFrom.HasValue)
                query = query.Where(l => l.LastReviewed >= criteria.LastReviewedFrom.Value);

            if (criteria.LastReviewedTo.HasValue)
                query = query.Where(l => l.LastReviewed <= criteria.LastReviewedTo.Value);

            // Sort
            query = ApplySorting(query, criteria.SortBy, criteria.SortDirection);

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Pagination
            var skip = (criteria.PageNumber - 1) * criteria.PageSize;
            var items = await query
                .Skip(skip)
                .Take(criteria.PageSize)
                .ToListAsync();

            return new SearchFilterResult<LanguageItem>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = criteria.PageNumber,
                PageSize = criteria.PageSize
            };
        }

        /// <summary>
        /// Quick search across all vocabulary in a language
        /// </summary>
        public async Task<List<VocabularyItem>> QuickSearchAsync(string searchTerm, int languageProfileId, int limit = 10)
        {
            var criteria = new SearchFilterCriteria
            {
                SearchTerm = searchTerm,
                LanguageProfileId = languageProfileId,
                PageSize = limit
            };

            var result = await SearchVocabularyAsync(criteria);
            return result.Items;
        }

        /// <summary>
        /// Get mastery statistics for a language
        /// </summary>
        public async Task<Dictionary<string, int>> GetMasteryStatisticsAsync(int languageProfileId)
        {
            var vocabItems = await _context.Vocabulary
                .Where(v => v.LanguageProfileId == languageProfileId)
                .ToListAsync();

            return new Dictionary<string, int>
            {
                { "NotStarted", vocabItems.Count(v => v.Mastery == 0) },
                { "Learning", vocabItems.Count(v => v.Mastery > 0 && v.Mastery < 50) },
                { "Intermediate", vocabItems.Count(v => v.Mastery >= 50 && v.Mastery < 90) },
                { "Mastered", vocabItems.Count(v => v.Mastery >= 90) }
            };
        }

        /// <summary>
        /// Apply sorting to query
        /// </summary>
        private IQueryable<VocabularyItem> ApplySorting(IQueryable<VocabularyItem> query, string sortBy, string direction)
        {
            var isAscending = direction?.ToLower() == "asc";

            return sortBy?.ToLower() switch
            {
                "mastery" => isAscending ? query.OrderBy(v => v.Mastery) : query.OrderByDescending(v => v.Mastery),
                "lastreviewed" => isAscending ? query.OrderBy(v => v.LastReviewed) : query.OrderByDescending(v => v.LastReviewed),
                _ => isAscending ? query.OrderBy(v => v.Term) : query.OrderByDescending(v => v.Term)
            };
        }

        /// <summary>
        /// Apply sorting to language items query
        /// </summary>
        private IQueryable<LanguageItem> ApplySorting(IQueryable<LanguageItem> query, string sortBy, string direction)
        {
            var isAscending = direction?.ToLower() == "asc";

            return sortBy?.ToLower() switch
            {
                "mastery" => isAscending ? query.OrderBy(l => l.Mastery) : query.OrderByDescending(l => l.Mastery),
                "lastreviewed" => isAscending ? query.OrderBy(l => l.LastReviewed) : query.OrderByDescending(l => l.LastReviewed),
                _ => isAscending ? query.OrderBy(l => l.Term) : query.OrderByDescending(l => l.Term)
            };
        }

        /// <summary>
        /// Get items due for review
        /// </summary>
        public async Task<List<VocabularyItem>> GetItemsDueForReviewAsync(int languageProfileId)
        {
            return await _context.Vocabulary
                .Where(v => v.LanguageProfileId == languageProfileId &&
                           v.LastReviewed.AddDays(v.Mastery / 10) < DateTime.UtcNow)
                .OrderBy(v => v.LastReviewed)
                .Take(10)
                .ToListAsync();
        }

        /// <summary>
        /// Get challenging items (low mastery)
        /// </summary>
        public async Task<List<VocabularyItem>> GetChallengingItemsAsync(int languageProfileId, int limit = 10)
        {
            return await _context.Vocabulary
                .Where(v => v.LanguageProfileId == languageProfileId && v.Mastery < 50)
                .OrderBy(v => v.Mastery)
                .Take(limit)
                .ToListAsync();
        }
    }
}
