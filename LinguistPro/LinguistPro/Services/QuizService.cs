using LinguistPro.Models;
using Microsoft.EntityFrameworkCore;

namespace LinguistPro.Services
{
    /// <summary>
    /// Service for managing quizzes and testing features
    /// </summary>
    public class QuizService
    {
        private readonly AppDbContext _context;

        public QuizService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create a new quiz attempt
        /// </summary>
        public async Task<QuizAttempt> CreateQuizAsync(int languageProfileId, string quizType, string difficulty, int questionCount = 10)
        {
            var langProfile = await _context.LanguageProfiles.FindAsync(languageProfileId);
            if (langProfile is null)
                throw new InvalidOperationException("Language profile not found");

            var quiz = new QuizAttempt
            {
                LanguageProfileId = languageProfileId,
                QuizType = quizType, // "vocabulary", "verbs", "mixed"
                DifficultyLevel = difficulty, // "easy", "medium", "hard"
                TotalQuestions = questionCount,
                Status = "in_progress",
                StartedAt = DateTime.UtcNow
            };

            _context.QuizAttempts.Add(quiz);
            await _context.SaveChangesAsync();

            return quiz;
        }

        /// <summary>
        /// Generate quiz questions based on language profile
        /// </summary>
        public async Task<List<QuizQuestion>> GenerateQuestionsAsync(int quizId, string quizType, string difficulty, int questionCount)
        {
            var quiz = await _context.QuizAttempts
                .Include(q => q.LanguageProfile)
                .FirstOrDefaultAsync(q => q.QuizAttemptId == quizId);

            if (quiz is null)
                throw new InvalidOperationException("Quiz not found");

            var questions = new List<QuizQuestion>();
            var random = new Random();

            // Get vocabulary items based on difficulty
            var vocabQuery = _context.Vocabulary
                .Where(v => v.LanguageProfileId == quiz.LanguageProfileId);

            var verbQuery = _context.Verbs
                .Where(v => v.LanguageProfileId == quiz.LanguageProfileId);

            // Filter by difficulty if needed
            if (difficulty == "easy")
            {
                vocabQuery = vocabQuery.Where(v => v.Mastery < 50);
                verbQuery = verbQuery.Where(v => true); // No mastery on verbs yet
            }
            else if (difficulty == "hard")
            {
                vocabQuery = vocabQuery.Where(v => v.Mastery >= 70);
            }

            var vocabularyItems = await vocabQuery.Take(questionCount * 2).ToListAsync();
            var verbItems = await verbQuery.Take(questionCount * 2).ToListAsync();

            // Generate questions
            for (int i = 0; i < questionCount; i++)
            {
                QuizQuestion question = new();

                // Determine question type based on quiz type
                if (quizType == "verbs" && verbItems.Count > i)
                {
                    question = GenerateVerbQuestion(verbItems[i], difficulty);
                }
                else if (quizType == "mixed" && i % 2 == 0 && verbItems.Count > i / 2)
                {
                    question = GenerateVerbQuestion(verbItems[i / 2], difficulty);
                }
                else if (vocabularyItems.Count > i)
                {
                    question = GenerateVocabularyQuestion(vocabularyItems[i], difficulty);
                }
                else
                {
                    continue;
                }

                question.QuizAttemptId = quizId;
                question.Difficulty = difficulty;
                questions.Add(question);
            }

            if (questions.Count > 0)
            {
                _context.QuizQuestions.AddRange(questions);
                await _context.SaveChangesAsync();
            }

            return questions;
        }

        /// <summary>
        /// Generate a vocabulary question
        /// </summary>
        private static QuizQuestion GenerateVocabularyQuestion(VocabularyItem vocab, string difficulty)
        {
            var question = new QuizQuestion
            {
                VocabularyItemId = vocab.Id,
                QuestionType = "multiple_choice",
                QuestionText = $"What does '{vocab.Term}' mean?",
                CorrectAnswer = vocab.Meaning,
                Options = vocab.Meaning, // Will need more options in real implementation
                Difficulty = difficulty
            };

            return question;
        }

        /// <summary>
        /// Generate a verb question
        /// </summary>
        private static QuizQuestion GenerateVerbQuestion(VerbEntry verb, string difficulty)
        {
            var question = new QuizQuestion
            {
                VerbEntryId = verb.Id,
                QuestionType = "multiple_choice",
                QuestionText = $"What is the meaning of '{verb.Infinitive}'?",
                CorrectAnswer = verb.Meaning,
                Options = verb.Meaning,
                Difficulty = difficulty
            };

            return question;
        }

        /// <summary>
        /// Submit an answer to a quiz question
        /// </summary>
        public async Task<QuizQuestion> SubmitAnswerAsync(int quizQuestionId, string userAnswer, long timeSpent)
        {
            var question = await _context.QuizQuestions.FindAsync(quizQuestionId);
            if (question is null)
                throw new InvalidOperationException("Question not found");

            question.UserAnswer = userAnswer;
            question.TimeSpentSeconds = timeSpent;
            question.IsCorrect = userAnswer.Equals(question.CorrectAnswer, StringComparison.OrdinalIgnoreCase);

            _context.QuizQuestions.Update(question);
            await _context.SaveChangesAsync();

            return question;
        }

        /// <summary>
        /// Complete a quiz and calculate score
        /// </summary>
        public async Task<QuizAttempt> CompleteQuizAsync(int quizId)
        {
            var quiz = await _context.QuizAttempts
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.QuizAttemptId == quizId);

            if (quiz is null)
                throw new InvalidOperationException("Quiz not found");

            quiz.CompletedAt = DateTime.UtcNow;
            quiz.Status = "completed";
            quiz.CorrectAnswers = quiz.Questions.Count(q => q.IsCorrect);
            quiz.ScorePercentage = quiz.TotalQuestions > 0 
                ? (int)((quiz.CorrectAnswers * 100) / quiz.TotalQuestions)
                : 0;
            quiz.TimeSpentSeconds = quiz.Questions.Sum(q => q.TimeSpentSeconds);

            // Update quiz statistics
            await UpdateQuizStatisticsAsync(quiz);

            _context.QuizAttempts.Update(quiz);
            await _context.SaveChangesAsync();

            return quiz;
        }

        /// <summary>
        /// Update quiz statistics for the language profile
        /// </summary>
        private async Task UpdateQuizStatisticsAsync(QuizAttempt quiz)
        {
            var stats = await _context.QuizStatistics
                .FirstOrDefaultAsync(s => s.LanguageProfileId == quiz.LanguageProfileId);

            if (stats is null)
            {
                stats = new QuizStatistics
                {
                    LanguageProfileId = quiz.LanguageProfileId
                };
                _context.QuizStatistics.Add(stats);
            }

            stats.TotalQuizzesTaken++;
            stats.LastQuizDate = quiz.CompletedAt;
            stats.TotalTimeSpentSeconds += quiz.TimeSpentSeconds;

            if (quiz.ScorePercentage > stats.BestScore)
                stats.BestScore = quiz.ScorePercentage;

            // Recalculate average
            var allQuizzes = await _context.QuizAttempts
                .Where(q => q.LanguageProfileId == quiz.LanguageProfileId && q.Status == "completed")
                .ToListAsync();

            stats.AverageScore = allQuizzes.Any() ? allQuizzes.Average(q => q.ScorePercentage) : 0;

            _context.QuizStatistics.Update(stats);
        }

        /// <summary>
        /// Get quiz statistics for a language profile
        /// </summary>
        public async Task<QuizStatistics> GetQuizStatisticsAsync(int languageProfileId)
        {
            var stats = await _context.QuizStatistics
                .FirstOrDefaultAsync(s => s.LanguageProfileId == languageProfileId);

            if (stats is null)
            {
                stats = new QuizStatistics
                {
                    LanguageProfileId = languageProfileId
                };
                _context.QuizStatistics.Add(stats);
                await _context.SaveChangesAsync();
            }

            return stats;
        }

        /// <summary>
        /// Get recent quiz attempts
        /// </summary>
        public async Task<List<QuizAttempt>> GetRecentQuizzesAsync(int languageProfileId, int limit = 10)
        {
            return await _context.QuizAttempts
                .Where(q => q.LanguageProfileId == languageProfileId && q.Status == "completed")
                .OrderByDescending(q => q.CompletedAt)
                .Take(limit)
                .ToListAsync();
        }

        /// <summary>
        /// Get a quiz by ID with questions
        /// </summary>
        public async Task<QuizAttempt?> GetQuizWithQuestionsAsync(int quizId)
        {
            return await _context.QuizAttempts
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.QuizAttemptId == quizId);
        }
    }
}
