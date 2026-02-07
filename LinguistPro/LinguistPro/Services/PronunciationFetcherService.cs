using System.Text.Json;
using LinguistPro.Models;

namespace LinguistPro.Services
{
    /// <summary>
    /// Service to fetch pronunciation and IPA data from free APIs
    /// Uses multiple free sources: Wiktionary API, Free Dictionary API
    /// </summary>
    public class PronunciationFetcherService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PronunciationFetcherService> _logger;

        public PronunciationFetcherService(HttpClient httpClient, ILogger<PronunciationFetcherService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Fetch pronunciation data from Free Dictionary API
        /// Supports: en, es, fr, de, it, ko, ja, zh, ar, hin, ur, ja, pt, nl, no, pl, ru, tr
        /// </summary>
        public async Task<PronunciationData?> FetchFromFreeDictionaryAsync(string word, string languageCode)
        {
            try
            {
                // Map language codes to Free Dictionary API language codes
                var langMap = new Dictionary<string, string>
                {
                    { "en", "en" },
                    { "de", "de" },
                    { "fr", "fr" },
                    { "es", "es" },
                    { "ru", "ru" },
                    { "ko", "ko" },
                    { "it", "it" },
                    { "pt", "pt" },
                    { "nl", "nl" }
                };

                if (!langMap.ContainsKey(languageCode))
                    return null;

                var apiLangCode = langMap[languageCode];
                var url = $"https://api.dictionaryapi.dev/api/v2/entries/{apiLangCode}/{word.ToLower()}";

                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Failed to fetch from Free Dictionary API: {word} ({languageCode})");
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(content);

                if (doc.RootElement.ValueKind != JsonValueKind.Array || doc.RootElement.GetArrayLength() == 0)
                    return null;

                var entry = doc.RootElement[0];

                var pronunciation = new PronunciationData
                {
                    Word = word,
                    LanguageCode = languageCode,
                    CreatedDate = DateTime.UtcNow,
                    DifficultySyllables = "moderate"
                };

                // Get IPA from phonetic field
                if (entry.TryGetProperty("phonetic", out var phoneticElement))
                {
                    pronunciation.IPA = phoneticElement.GetString() ?? "";
                }

                // Try to get pronunciation from phonetics array
                if (entry.TryGetProperty("phonetics", out var phoneticsArray))
                {
                    foreach (var phonetic in phoneticsArray.EnumerateArray())
                    {
                        if (phonetic.TryGetProperty("text", out var ipaText))
                        {
                            pronunciation.IPA = ipaText.GetString() ?? pronunciation.IPA;
                        }

                        if (phonetic.TryGetProperty("audio", out var audioUrl))
                        {
                            var audio = audioUrl.GetString();
                            if (!string.IsNullOrEmpty(audio))
                            {
                                pronunciation.AudioUrl = audio;
                            }
                        }
                    }
                }

                // Get definition/meaning for pronunciation notes
                if (entry.TryGetProperty("meanings", out var meaningsArray) && meaningsArray.GetArrayLength() > 0)
                {
                    var meaning = meaningsArray[0];
                    if (meaning.TryGetProperty("definitions", out var definitionsArray) && definitionsArray.GetArrayLength() > 0)
                    {
                        var definition = definitionsArray[0];
                        if (definition.TryGetProperty("definition", out var def))
                        {
                            pronunciation.PronunciationNotes = def.GetString();
                        }
                    }
                }

                return string.IsNullOrEmpty(pronunciation.IPA) ? null : pronunciation;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching from Free Dictionary API: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Fetch IPA from Wiktionary API (more comprehensive for multiple languages)
        /// </summary>
        public async Task<PronunciationData?> FetchFromWiktionaryAsync(string word, string languageCode)
        {
            try
            {
                // Map to Wiktionary language names
                var langMap = new Dictionary<string, string>
                {
                    { "de", "German" },
                    { "fr", "French" },
                    { "es", "Spanish" },
                    { "ru", "Russian" },
                    { "ko", "Korean" },
                    { "en", "English" },
                    { "it", "Italian" }
                };

                if (!langMap.ContainsKey(languageCode))
                    return null;

                var wiktionaryLang = langMap[languageCode];
                var url = $"https://en.wiktionary.org/api/rest_v1/page/html/{word}";

                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"Failed to fetch from Wiktionary: {word}");
                    return null;
                }

                var html = await response.Content.ReadAsStringAsync();

                // Extract IPA using regex - look for IPA pattern
                var ipaPattern = @"/[^/]+/"; // Matches /text/ format
                var ipaMatch = System.Text.RegularExpressions.Regex.Match(html, ipaPattern);

                if (!ipaMatch.Success)
                    return null;

                var pronunciation = new PronunciationData
                {
                    Word = word,
                    LanguageCode = languageCode,
                    IPA = ipaMatch.Value,
                    DifficultySyllables = "moderate",
                    CreatedDate = DateTime.UtcNow
                };

                return pronunciation;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching from Wiktionary: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Fetch Google Translate TTS audio URL
        /// Free, no API key required
        /// </summary>
        public string GetGoogleTranslateAudioUrl(string word, string languageCode)
        {
            var langMap = new Dictionary<string, string>
            {
                { "de", "de" },
                { "fr", "fr" },
                { "es", "es" },
                { "ru", "ru" },
                { "ko", "ko" },
                { "en", "en" }
            };

            if (!langMap.ContainsKey(languageCode))
                return "";

            var lang = langMap[languageCode];
            return $"https://translate.google.com/translate_tts?ie=UTF-8&client=tw-ob&q={Uri.EscapeDataString(word)}&tl={lang}";
        }

        /// <summary>
        /// Try multiple APIs to get comprehensive pronunciation data
        /// </summary>
        public async Task<PronunciationData?> FetchComprehensiveAsync(string word, string languageCode)
        {
            // Try Free Dictionary first (best for IPA)
            var result = await FetchFromFreeDictionaryAsync(word, languageCode);

            // If no result, try Wiktionary
            if (result == null)
            {
                result = await FetchFromWiktionaryAsync(word, languageCode);
            }

            // If we got some result, add Google Translate audio if not present
            if (result != null && string.IsNullOrEmpty(result.AudioUrl))
            {
                result.AudioUrl = GetGoogleTranslateAudioUrl(word, languageCode);
            }

            return result;
        }

        /// <summary>
        /// Parse syllables from IPA (basic implementation)
        /// </summary>
        public string ParseSyllablesFromWord(string word, string languageCode)
        {
            // Simple syllable breakdown - can be enhanced
            // For now, just return the word in uppercase
            // In production, would use language-specific rules

            return languageCode switch
            {
                "de" => BreakGermanSyllables(word),
                "fr" => BreakFrenchSyllables(word),
                "es" => BreakSpanishSyllables(word),
                "ru" => BreakRussianSyllables(word),
                "ko" => BreakKoreanSyllables(word),
                _ => word.ToUpper()
            };
        }

        private string BreakGermanSyllables(string word)
        {
            // German syllable rules (basic)
            var syllables = new List<string>();
            var current = "";

            for (int i = 0; i < word.Length; i++)
            {
                current += word[i];
                if (i < word.Length - 1 && IsVowel(word[i]) && !IsVowel(word[i + 1]))
                {
                    syllables.Add(current);
                    current = "";
                }
            }

            if (!string.IsNullOrEmpty(current))
                syllables.Add(current);

            return string.Join("-", syllables).ToUpper();
        }

        private string BreakFrenchSyllables(string word) => word.ToUpper(); // Simplified
        private string BreakSpanishSyllables(string word) => word.ToUpper(); // Simplified
        private string BreakRussianSyllables(string word) => word.ToUpper(); // Simplified
        private string BreakKoreanSyllables(string word) => word; // Korean already syllabic

        private bool IsVowel(char c) =>
            char.ToLower(c) switch
            {
                'a' or 'e' or 'i' or 'o' or 'u' or 'ä' or 'ö' or 'ü' or 'á' or 'é' or 'í' or 'ó' or 'ú' => true,
                _ => false
            };
    }
}
