using LinguistPro.Models;
using LinguistPro.Services;

namespace LinguistPro.Data
{
    /// <summary>
    /// Seed pronunciation data from free APIs
    /// </summary>
    public class PronunciationSeeder
    {
        private readonly AppDbContext _context;
        private readonly PronunciationFetcherService _fetcher;
        private readonly ILogger<PronunciationSeeder> _logger;

        public PronunciationSeeder(AppDbContext context, PronunciationFetcherService fetcher, ILogger<PronunciationSeeder> logger)
        {
            _context = context;
            _fetcher = fetcher;
            _logger = logger;
        }

        /// <summary>
        /// Seed common words with pronunciation data
        /// </summary>
        public async Task SeedCommonWordsAsync()
        {
            // Check if already seeded
            if (_context.PronunciationData.Any())
            {
                _logger.LogInformation("Pronunciation data already exists, skipping seed");
                return;
            }

            _logger.LogInformation("Starting pronunciation data seeding...");

            var words = new Dictionary<string, List<string>>
            {
                { "de", new List<string> { "hallo", "danke", "schön", "guten", "morgen", "nacht", "haus", "wasser", "brot", "käse" } },
                { "fr", new List<string> { "bonjour", "merci", "oui", "non", "s'il vous plaît", "excusez", "amour", "eau", "pain", "fromage" } },
                { "es", new List<string> { "hola", "gracias", "sí", "no", "por favor", "disculpe", "amor", "agua", "pan", "queso" } },
                { "ru", new List<string> { "привет", "спасибо", "да", "нет", "пожалуйста", "извините", "любовь", "вода", "хлеб", "сыр" } },
                { "ko", new List<string> { "안녕하세요", "감사합니다", "네", "아니오", "제발", "죄송합니다", "사랑", "물", "빵", "치즈" } }
            };

            foreach (var language in words)
            {
                var langCode = language.Key;
                var wordList = language.Value;

                _logger.LogInformation($"Fetching pronunciation for {langCode}...");

                foreach (var word in wordList)
                {
                    try
                    {
                        // Check if already exists
                        var existing = _context.PronunciationData
                            .FirstOrDefault(p => p.Word.ToLower() == word.ToLower() && p.LanguageCode == langCode);

                        if (existing != null)
                            continue;

                        _logger.LogInformation($"Fetching: {word} ({langCode})");

                        // Fetch from free API
                        var pronunciation = await _fetcher.FetchComprehensiveAsync(word, langCode);

                        if (pronunciation != null)
                        {
                            // Add syllable breakdown if empty
                            if (string.IsNullOrEmpty(pronunciation.SyllableBreakdown))
                            {
                                pronunciation.SyllableBreakdown = _fetcher.ParseSyllablesFromWord(word, langCode);
                            }

                            _context.PronunciationData.Add(pronunciation);
                            _logger.LogInformation($"✓ Added: {word} - {pronunciation.IPA}");
                        }
                        else
                        {
                            _logger.LogWarning($"✗ Failed to fetch: {word}");
                        }

                        // Small delay to avoid rate limiting
                        await Task.Delay(500);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error processing {word}: {ex.Message}");
                    }
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Pronunciation data seeding completed!");
        }
    }
}
