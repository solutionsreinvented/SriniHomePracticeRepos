using System.Text.Json;
using LinguistPro.Models;

namespace LinguistPro.Services
{
    /// <summary>
    /// Real-time vocabulary fetcher from free APIs
    /// No seeding - fetches data on demand
    /// </summary>
    public class VocabularyAutoFetcherService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<VocabularyAutoFetcherService> _logger;
        private readonly AppDbContext _context;

        public VocabularyAutoFetcherService(HttpClient httpClient, ILogger<VocabularyAutoFetcherService> logger, AppDbContext context)
        {
            _httpClient = httpClient;
            _logger = logger;
            _context = context;
            // Set timeout for API calls
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        /// <summary>
        /// Fetch vocabulary words from Free Dictionary API
        /// https://random-word-api.herokuapp.com/
        /// </summary>
        public async Task<List<(string word, string meaning)>> FetchRandomWordsAsync(string languageCode, int count = 20)
        {
            try
            {
                var words = new List<(string, string)>();

                // For now, use a predefined list of common words per language
                // In production, you could use an actual dictionary API
                var commonWords = GetCommonWordsByLanguage(languageCode);

                // Fetch pronunciation for each word
                foreach (var word in commonWords.Take(count))
                {
                    try
                    {
                        // Try to get meaning from Free Dictionary API
                        var meaning = await GetWordMeaningAsync(word, languageCode);
                        words.Add((word, meaning));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"Error fetching meaning for {word}: {ex.Message}");
                        words.Add((word, ""));
                    }
                }

                return words;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching random words: {ex.Message}");
                return new List<(string, string)>();
            }
        }

        /// <summary>
        /// Get word meaning from Free Dictionary API
        /// </summary>
        private async Task<string> GetWordMeaningAsync(string word, string languageCode)
        {
            try
            {
                var langMap = new Dictionary<string, string>
                {
                    { "en", "en" },
                    { "de", "de" },
                    { "fr", "fr" },
                    { "es", "es" },
                    { "ru", "ru" },
                    { "ko", "ko" }
                };

                if (!langMap.ContainsKey(languageCode))
                    return "";

                var apiLang = langMap[languageCode];
                var url = $"https://api.dictionaryapi.dev/api/v2/entries/{apiLang}/{word.ToLower()}";

                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    return "";

                var content = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(content);

                var meanings = doc.RootElement[0].GetProperty("meanings");
                if (meanings.GetArrayLength() > 0)
                {
                    var definitions = meanings[0].GetProperty("definitions");
                    if (definitions.GetArrayLength() > 0)
                    {
                        var definition = definitions[0].GetProperty("definition").GetString();
                        return definition?.Substring(0, Math.Min(100, definition.Length)) ?? "";
                    }
                }

                return "";
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Get common words for each language
        /// </summary>
        private List<string> GetCommonWordsByLanguage(string languageCode)
        {
            return languageCode switch
            {
                "de" => new List<string>
                {
                    "hallo", "danke", "ja", "nein", "bitte", "guten", "morgen", "abend", "nacht", "haus",
                    "schule", "arbeit", "freund", "familie", "liebe", "zeit", "wasser", "essen", "trinken", "schlafen",
                    "sonne", "mond", "stern", "baum", "blume", "tier", "hund", "katze", "vogel", "fisch"
                },
                "fr" => new List<string>
                {
                    "bonjour", "merci", "oui", "non", "s'il vous plaît", "au revoir", "amour", "ami", "famille", "maison",
                    "école", "travail", "jour", "nuit", "matin", "soir", "eau", "pain", "vin", "fromage",
                    "soleil", "lune", "étoile", "arbre", "fleur", "animal", "chien", "chat", "oiseau", "poisson"
                },
                "es" => new List<string>
                {
                    "hola", "gracias", "sí", "no", "por favor", "adiós", "amor", "amigo", "familia", "casa",
                    "escuela", "trabajo", "día", "noche", "mañana", "tarde", "agua", "pan", "vino", "queso",
                    "sol", "luna", "estrella", "árbol", "flor", "animal", "perro", "gato", "pájaro", "pez"
                },
                "ru" => new List<string>
                {
                    "привет", "спасибо", "да", "нет", "пожалуйста", "до свидания", "любовь", "друг", "семья", "дом",
                    "школа", "работа", "день", "ночь", "утро", "вечер", "вода", "хлеб", "вино", "сыр",
                    "солнце", "луна", "звезда", "дерево", "цветок", "животное", "собака", "кошка", "птица", "рыба"
                },
                "ko" => new List<string>
                {
                    "안녕하세요", "감사합니다", "네", "아니오", "제발", "안녕히", "사랑", "친구", "가족", "집",
                    "학교", "일", "날", "밤", "아침", "저녁", "물", "빵", "와인", "치즈",
                    "태양", "달", "별", "나무", "꽃", "동물", "개", "고양이", "새", "물고기"
                },
                _ => new List<string>()
            };
        }
    }
}
