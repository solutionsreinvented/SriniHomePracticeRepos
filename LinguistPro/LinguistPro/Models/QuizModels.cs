using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinguistPro.Models
{
    /// <summary>
    /// Quiz attempt record for tracking user quiz participation
    /// </summary>
    public class QuizAttempt
    {
        [Key]
        public int QuizAttemptId { get; set; }

        [ForeignKey(nameof(LanguageProfile))]
        public int LanguageProfileId { get; set; }

        /// <summary>
        /// Quiz type: "vocabulary", "verbs", "mixed"
        /// </summary>
        [Required]
        [StringLength(20)]
        public string QuizType { get; set; } = "vocabulary";

        /// <summary>
        /// Difficulty level: "easy", "medium", "hard"
        /// </summary>
        [Required]
        [StringLength(10)]
        public string DifficultyLevel { get; set; } = "medium";

        /// <summary>
        /// Total questions in this quiz
        /// </summary>
        [Range(1, 50)]
        public int TotalQuestions { get; set; } = 10;

        /// <summary>
        /// Number of questions answered correctly
        /// </summary>
        [Range(0, 50)]
        public int CorrectAnswers { get; set; } = 0;

        /// <summary>
        /// Score as percentage (0-100)
        /// </summary>
        [Range(0, 100)]
        public int ScorePercentage { get; set; } = 0;

        /// <summary>
        /// Time spent on quiz in seconds
        /// </summary>
        public long TimeSpentSeconds { get; set; } = 0;

        /// <summary>
        /// When the quiz was started
        /// </summary>
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// When the quiz was completed
        /// </summary>
        public DateTime? CompletedAt { get; set; }

        /// <summary>
        /// Quiz status: "in_progress", "completed", "abandoned"
        /// </summary>
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "in_progress";

        // Navigation properties
        public virtual LanguageProfile? LanguageProfile { get; set; }
        public virtual ICollection<QuizQuestion> Questions { get; set; } = new List<QuizQuestion>();
    }

    /// <summary>
    /// Individual question in a quiz
    /// </summary>
    public class QuizQuestion
    {
        [Key]
        public int QuizQuestionId { get; set; }

        [ForeignKey(nameof(QuizAttempt))]
        public int QuizAttemptId { get; set; }

        [ForeignKey(nameof(VocabularyItem))]
        public int? VocabularyItemId { get; set; }

        [ForeignKey(nameof(VerbEntry))]
        public int? VerbEntryId { get; set; }

        /// <summary>
        /// Question type: "multiple_choice", "fill_blank", "matching"
        /// </summary>
        [Required]
        [StringLength(20)]
        public string QuestionType { get; set; } = "multiple_choice";

        /// <summary>
        /// The question text
        /// </summary>
        [Required]
        public string QuestionText { get; set; } = string.Empty;

        /// <summary>
        /// Comma-separated options for multiple choice
        /// </summary>
        public string? Options { get; set; }

        /// <summary>
        /// The correct answer
        /// </summary>
        [Required]
        public string CorrectAnswer { get; set; } = string.Empty;

        /// <summary>
        /// User's provided answer
        /// </summary>
        public string? UserAnswer { get; set; }

        /// <summary>
        /// Was this question answered correctly?
        /// </summary>
        public bool IsCorrect { get; set; } = false;

        /// <summary>
        /// Question difficulty for this quiz: "easy", "medium", "hard"
        /// </summary>
        [StringLength(10)]
        public string Difficulty { get; set; } = "medium";

        /// <summary>
        /// Time spent on this question in seconds
        /// </summary>
        public long TimeSpentSeconds { get; set; } = 0;

        // Navigation properties
        public virtual QuizAttempt? QuizAttempt { get; set; }
        public virtual VocabularyItem? VocabularyItem { get; set; }
        public virtual VerbEntry? VerbEntry { get; set; }
    }

    /// <summary>
    /// Quiz statistics for a language profile
    /// </summary>
    public class QuizStatistics
    {
        [Key]
        public int QuizStatisticsId { get; set; }

        [ForeignKey(nameof(LanguageProfile))]
        public int LanguageProfileId { get; set; }

        /// <summary>
        /// Total quizzes taken
        /// </summary>
        public int TotalQuizzesTaken { get; set; } = 0;

        /// <summary>
        /// Best score achieved (0-100)
        /// </summary>
        [Range(0, 100)]
        public int BestScore { get; set; } = 0;

        /// <summary>
        /// Average score across all quizzes
        /// </summary>
        [Range(0, 100)]
        public double AverageScore { get; set; } = 0;

        /// <summary>
        /// Total time spent on quizzes in seconds
        /// </summary>
        public long TotalTimeSpentSeconds { get; set; } = 0;

        /// <summary>
        /// Last quiz completion date
        /// </summary>
        public DateTime? LastQuizDate { get; set; }

        /// <summary>
        /// Current quiz streak (consecutive days with quiz taken)
        /// </summary>
        public int CurrentQuizStreak { get; set; } = 0;

        /// <summary>
        /// Best quiz streak achieved
        /// </summary>
        public int BestQuizStreak { get; set; } = 0;

        // Navigation property
        public virtual LanguageProfile? LanguageProfile { get; set; }
    }
}
