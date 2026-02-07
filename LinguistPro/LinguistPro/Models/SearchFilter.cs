namespace LinguistPro.Models
{
    /// <summary>
    /// Filter and search criteria for vocabulary and language items
    /// </summary>
    public class SearchFilterCriteria
    {
        /// <summary>
        /// Search term for vocabulary/language item
        /// </summary>
        public string? SearchTerm { get; set; }

        /// <summary>
        /// Filter by mastery level (0-100)
        /// </summary>
        public int? MasteryMinimum { get; set; }
        public int? MasteryMaximum { get; set; }

        /// <summary>
        /// Filter by item type (Vocabulary, Verb, Number, Month, Day)
        /// </summary>
        public string? ItemType { get; set; }

        /// <summary>
        /// Filter by date added (date range)
        /// </summary>
        public DateTime? DateAddedFrom { get; set; }
        public DateTime? DateAddedTo { get; set; }

        /// <summary>
        /// Filter by last reviewed date
        /// </summary>
        public DateTime? LastReviewedFrom { get; set; }
        public DateTime? LastReviewedTo { get; set; }

        /// <summary>
        /// Sort by field: "term", "mastery", "dateAdded", "lastReviewed"
        /// </summary>
        public string SortBy { get; set; } = "term";

        /// <summary>
        /// Sort direction: "asc" or "desc"
        /// </summary>
        public string SortDirection { get; set; } = "asc";

        /// <summary>
        /// Pagination
        /// </summary>
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// Language profile ID for filtering
        /// </summary>
        public int LanguageProfileId { get; set; }

        /// <summary>
        /// Tags/categories to filter by
        /// </summary>
        public List<string>? Tags { get; set; }

        /// <summary>
        /// Show only unmastered items
        /// </summary>
        public bool? ShowOnlyUnmastered { get; set; }

        /// <summary>
        /// Show only items due for review
        /// </summary>
        public bool? ShowOnlyDueForReview { get; set; }
    }

    /// <summary>
    /// Result set from search/filter
    /// </summary>
    public class SearchFilterResult<T>
    {
        public List<T> Items { get; set; } = [];
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }

    /// <summary>
    /// Saved filter for quick access
    /// </summary>
    public class SavedFilter
    {
        public int FilterId { get; set; }
        public int UserId { get; set; }
        public string FilterName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public SearchFilterCriteria Criteria { get; set; } = new();
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime LastUsedDate { get; set; } = DateTime.UtcNow;
        public int UseCount { get; set; } = 0;
    }
}
