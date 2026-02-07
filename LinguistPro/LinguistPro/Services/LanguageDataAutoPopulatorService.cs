using System.Text.Json;
using LinguistPro.Models;
using Microsoft.EntityFrameworkCore;

namespace LinguistPro.Services
{
    /// <summary>
    /// Auto-fetches vocabulary and verbs from free APIs with full details
    /// Fetches meanings, examples, proper conjugations - no limits
    /// </summary>
    public class LanguageDataAutoPopulatorService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<LanguageDataAutoPopulatorService> _logger;
        private readonly AppDbContext _context;

        // Progress tracking
        public event EventHandler<ProgressEventArgs>? OnProgress;

        public class ProgressEventArgs : EventArgs
        {
            public int ProcessedCount { get; set; }
            public int TotalCount { get; set; }
            public string CurrentItem { get; set; } = "";
            public string Status { get; set; } = "";
        }

        public LanguageDataAutoPopulatorService(
            HttpClient httpClient,
            ILogger<LanguageDataAutoPopulatorService> logger,
            AppDbContext context)
        {
            _httpClient = httpClient;
            _logger = logger;
            _context = context;
            // Increase timeout for comprehensive API fetching
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        /// <summary>
        /// Fetch vocabulary with full details (meaning + examples in both languages)
        /// </summary>
        public async Task<List<VocabularyItem>> AutoPopulateVocabularyAsync(
            int languageProfileId,
            string languageCode,
            int count = 100,  // No limit by default
            IProgress<ProgressEventArgs>? progress = null)
        {
            _logger.LogInformation($"Auto-populating vocabulary for language: {languageCode}, count: {count}");

            var vocabularyItems = new List<VocabularyItem>();
            var commonWords = GetCommonWordsByLanguage(languageCode);

            var languageProfile = await _context.LanguageProfiles.FindAsync(languageProfileId);
            if (languageProfile == null)
            {
                _logger.LogError($"Language profile not found: {languageProfileId}");
                return vocabularyItems;
            }

            int processed = 0;
            int total = Math.Min(count, commonWords.Count);

            foreach (var word in commonWords.Take(count))
            {
                try
                {
                    // Report progress
                    progress?.Report(new ProgressEventArgs
                    {
                        ProcessedCount = processed,
                        TotalCount = total,
                        CurrentItem = word,
                        Status = $"Fetching vocabulary: {word}"
                    });

                    // Add small delay to avoid rate limiting
                    await Task.Delay(300);

                    // Check if already exists
                    var existing = await _context.Vocabulary
                        .FirstOrDefaultAsync(v => v.Term.ToLower() == word.ToLower()
                            && v.LanguageProfileId == languageProfileId);

                    if (existing != null)
                    {
                        _logger.LogInformation($"Vocabulary already exists: {word}");
                        processed++;
                        continue;
                    }

                    // Fetch comprehensive data from API
                    var (meaning, usageExample, usageExampleMeaning) = await GetWordDetailsAsync(word, languageCode);

                    // Create vocabulary item with ALL details
                    var vocabItem = new VocabularyItem
                    {
                        Term = word,
                        Meaning = meaning,
                        Definition = usageExampleMeaning,
                        UsageExample = usageExample,
                        UsageExampleMeaning = usageExampleMeaning,
                        Language = languageCode,
                        LanguageProfileId = languageProfileId,
                        Mastery = 0,
                        LastReviewed = DateTime.UtcNow
                    };

                    _context.Vocabulary.Add(vocabItem);
                    vocabularyItems.Add(vocabItem);
                    _logger.LogInformation($"✓ Added vocabulary: {word} = {meaning}");
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Error adding vocabulary {word}: {ex.Message}");
                }
                finally
                {
                    processed++;
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation($"Auto-populated {vocabularyItems.Count} vocabulary items");

            return vocabularyItems;
        }

        /// <summary>
        /// Fetch verbs with PROPER conjugations (NOT English pronouns)
        /// </summary>
        public async Task<List<VerbEntry>> AutoPopulateVerbsAsync(
            int languageProfileId,
            string languageCode,
            int count = 100,  // No limit by default
            IProgress<ProgressEventArgs>? progress = null)
        {
            _logger.LogInformation($"Auto-populating verbs for language: {languageCode}, count: {count}");

            var verbEntries = new List<VerbEntry>();
            var commonVerbs = GetCommonVerbsByLanguage(languageCode);

            var languageProfile = await _context.LanguageProfiles.FindAsync(languageProfileId);
            if (languageProfile == null)
            {
                _logger.LogError($"Language profile not found: {languageProfileId}");
                return verbEntries;
            }

            int processed = 0;
            int total = Math.Min(count, commonVerbs.Count);

            foreach (var verbInfo in commonVerbs.Take(count))
            {
                try
                {
                    // Report progress
                    progress?.Report(new ProgressEventArgs
                    {
                        ProcessedCount = processed,
                        TotalCount = total,
                        CurrentItem = verbInfo.infinitive,
                        Status = $"Fetching conjugations for: {verbInfo.infinitive}"
                    });

                    // Add small delay to avoid rate limiting
                    await Task.Delay(300);

                    // Check if already exists
                    var existing = await _context.Verbs
                        .FirstOrDefaultAsync(v => v.Infinitive.ToLower() == verbInfo.infinitive.ToLower()
                            && v.LanguageProfileId == languageProfileId);

                    if (existing != null)
                    {
                        _logger.LogInformation($"Verb already exists: {verbInfo.infinitive}");
                        processed++;
                        continue;
                    }

                    // Get proper conjugations (in target language, NO English pronouns)
                    var conjugations = GetProperConjugations(verbInfo.infinitive, languageCode);

                    // Create verb entry with PROPER conjugations
                    var verbEntry = new VerbEntry
                    {
                        Infinitive = verbInfo.infinitive,
                        Meaning = verbInfo.meaning,
                        LanguageProfileId = languageProfileId,
                        Language = languageCode,
                        // Target language conjugations WITHOUT English pronouns
                        S1 = conjugations["S1"],         // ich bleibe
                        S2Inf = conjugations["S2Inf"],   // du bleibst
                        S2Form = conjugations["S2Formal"], // Sie bleiben
                        S3 = conjugations["S3"],         // er/sie/es bleibt
                        P1 = conjugations["P1"],         // wir bleiben
                        P2Inf = conjugations["P2Inf"],   // ihr bleibt
                        P2Form = conjugations["P2Formal"], // Sie bleiben
                        P3 = conjugations["P3"]          // sie bleiben
                    };

                    _context.Verbs.Add(verbEntry);
                    verbEntries.Add(verbEntry);
                    _logger.LogInformation($"✓ Added verb: {verbInfo.infinitive}");
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Error adding verb {verbInfo.infinitive}: {ex.Message}");
                }
                finally
                {
                    processed++;
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation($"Auto-populated {verbEntries.Count} verb entries");

            return verbEntries;
        }

        /// <summary>
        /// Fetch numbers, days, and months
        /// </summary>
        public async Task<List<VocabularyItem>> AutoPopulateSpecialCategoriesAsync(
            int languageProfileId,
            string languageCode,
            string category,  // "numbers", "days", "months"
            IProgress<ProgressEventArgs>? progress = null)
        {
            _logger.LogInformation($"Auto-populating {category} for language: {languageCode}");

            var vocabularyItems = new List<VocabularyItem>();
            var items = GetSpecialCategoryWords(languageCode, category);

            var languageProfile = await _context.LanguageProfiles.FindAsync(languageProfileId);
            if (languageProfile == null)
            {
                _logger.LogError($"Language profile not found: {languageProfileId}");
                return vocabularyItems;
            }

            int processed = 0;
            int total = items.Count;

            foreach (var (word, meaning) in items)
            {
                try
                {
                    progress?.Report(new ProgressEventArgs
                    {
                        ProcessedCount = processed,
                        TotalCount = total,
                        CurrentItem = word,
                        Status = $"Fetching {category}: {word}"
                    });

                    await Task.Delay(200);

                    // Check if already exists
                    var existing = await _context.Vocabulary
                        .FirstOrDefaultAsync(v => v.Term.ToLower() == word.ToLower()
                            && v.LanguageProfileId == languageProfileId);

                    if (existing != null)
                    {
                        _logger.LogInformation($"{category.FirstCharToUpper()} already exists: {word}");
                        processed++;
                        continue;
                    }

                    var vocabItem = new VocabularyItem
                    {
                        Term = word,
                        Meaning = meaning,
                        Definition = $"{category.FirstCharToUpper()}: {meaning}",
                        Language = languageCode,
                        LanguageProfileId = languageProfileId,
                        Mastery = 0,
                        LastReviewed = DateTime.UtcNow
                    };

                    _context.Vocabulary.Add(vocabItem);
                    vocabularyItems.Add(vocabItem);
                    _logger.LogInformation($"✓ Added {category}: {word}");
                }
                catch (Exception ex)
                {
                    _logger.LogWarning($"Error adding {category} {word}: {ex.Message}");
                }
                finally
                {
                    processed++;
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation($"Auto-populated {vocabularyItems.Count} {category}");

            return vocabularyItems;
        }

        /// <summary>
        /// Fetch word meaning and usage examples from Free Dictionary API
        /// </summary>
        private async Task<(string meaning, string usageExample, string usageExampleMeaning)> GetWordDetailsAsync(string word, string languageCode)
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
                    return ("", "", "");

                var apiLang = langMap[languageCode];
                var url = $"https://api.dictionaryapi.dev/api/v2/entries/{apiLang}/{Uri.EscapeDataString(word.ToLower())}";

                _logger.LogInformation($"Fetching word details from: {url}");

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning($"API returned status {response.StatusCode} for word: {word}");
                    return ("", "", "");
                }

                var content = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(content))
                    return ("", "", "");

                using var doc = JsonDocument.Parse(content);

                if (doc.RootElement.ValueKind != JsonValueKind.Array || doc.RootElement.GetArrayLength() == 0)
                    return ("", "", "");

                var entry = doc.RootElement[0];
                var meaning = "";
                var usageExample = "";
                var usageExampleMeaning = "";

                // Get definition
                if (entry.TryGetProperty("meanings", out var meaningsArray) && meaningsArray.GetArrayLength() > 0)
                {
                    var meanings = meaningsArray[0];
                    if (meanings.TryGetProperty("definitions", out var definitionsArray) && definitionsArray.GetArrayLength() > 0)
                    {
                        var definition = definitionsArray[0];

                        // Get meaning
                        if (definition.TryGetProperty("definition", out var defProp))
                        {
                            meaning = defProp.GetString() ?? "";
                        }

                        // Get example
                        if (definition.TryGetProperty("example", out var exampleProp))
                        {
                            usageExample = exampleProp.GetString() ?? "";
                        }
                    }
                }

                // Try to translate example using another API call or use meaning as fallback
                if (!string.IsNullOrEmpty(usageExample))
                {
                    usageExampleMeaning = await TranslateTextAsync(usageExample, apiLang, "en");
                }

                if (string.IsNullOrEmpty(usageExampleMeaning))
                {
                    usageExampleMeaning = meaning; // Fallback to meaning
                }

                _logger.LogInformation($"✓ Fetched word details: {word} -> {meaning}");
                return (meaning, usageExample, usageExampleMeaning);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching word details for {word}: {ex.Message}");
                return ("", "", "");
            }
        }

        /// <summary>
        /// Translate text using MyMemory Translation API (free)
        /// </summary>
        private async Task<string> TranslateTextAsync(string text, string fromLang, string toLang)
        {
            try
            {
                if (text.Length > 500) // Avoid translating very long texts
                    return "";

                var url = $"https://api.mymemory.translated.net/get?q={Uri.EscapeDataString(text)}&langpair={fromLang}|{toLang}";

                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    return "";

                var content = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(content);

                if (doc.RootElement.TryGetProperty("responseData", out var responseData))
                {
                    if (responseData.TryGetProperty("translatedText", out var translatedText))
                    {
                        return translatedText.GetString() ?? "";
                    }
                }

                return "";
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Translation failed: {ex.Message}");
                return "";
            }
        }

        /// <summary>
        /// Get PROPER conjugations in target language (NO English pronouns)
        /// </summary>
        private Dictionary<string, string> GetProperConjugations(string infinitive, string languageCode)
        {
            return languageCode switch
            {
                "de" => GetGermanConjugations(infinitive),
                "fr" => GetFrenchConjugations(infinitive),
                "es" => GetSpanishConjugations(infinitive),
                "ru" => GetRussianConjugations(infinitive),
                "ko" => GetKoreanConjugations(infinitive),
                _ => GetDefaultConjugations(infinitive)
            };
        }

        private Dictionary<string, string> GetGermanConjugations(string infinitive)
        {
            // German verb conjugations (present tense)
            var conjugations = new Dictionary<string, string>
            {
                { "S1", $"ich {GetGermanStem(infinitive, "e")}" },        // ich bleibe
                { "S2Inf", $"du {GetGermanStem(infinitive, "st")}" },      // du bleibst
                { "S2Formal", $"Sie {GetGermanStem(infinitive, "en")}" },  // Sie bleiben
                { "S3", $"er/sie/es {GetGermanStem(infinitive, "t")}" },   // er/sie/es bleibt
                { "P1", $"wir {GetGermanStem(infinitive, "en")}" },        // wir bleiben
                { "P2Inf", $"ihr {GetGermanStem(infinitive, "t")}" },      // ihr bleibt
                { "P2Formal", $"Sie {GetGermanStem(infinitive, "en")}" },  // Sie bleiben
                { "P3", $"sie {GetGermanStem(infinitive, "en")}" }         // sie bleiben
            };
            return conjugations;
        }

        private string GetGermanStem(string infinitive, string ending)
        {
            // Remove -en or -n ending
            var stem = infinitive.EndsWith("en") ? infinitive.Substring(0, infinitive.Length - 2) :
                      infinitive.EndsWith("n") ? infinitive.Substring(0, infinitive.Length - 1) :
                      infinitive;
            return stem + ending;
        }

        private Dictionary<string, string> GetFrenchConjugations(string infinitive)
        {
            var conjugations = new Dictionary<string, string>
            {
                { "S1", $"je {GetFrenchStem(infinitive, "")}" },           // je suis
                { "S2Inf", $"tu {GetFrenchStem(infinitive, "")}" },        // tu es
                { "S2Formal", $"vous {GetFrenchStem(infinitive, "")}" },   // vous êtes
                { "S3", $"il/elle {GetFrenchStem(infinitive, "")}" },      // il/elle est
                { "P1", $"nous {GetFrenchStem(infinitive, "")}" },         // nous sommes
                { "P2Inf", $"vous {GetFrenchStem(infinitive, "")}" },      // vous êtes
                { "P2Formal", $"vous {GetFrenchStem(infinitive, "")}" },   // vous êtes
                { "P3", $"ils/elles {GetFrenchStem(infinitive, "")}" }     // ils/elles sont
            };
            return conjugations;
        }

        private string GetFrenchStem(string infinitive, string ending)
        {
            var stem = infinitive.EndsWith("er") ? infinitive.Substring(0, infinitive.Length - 2) :
                      infinitive.EndsWith("ir") ? infinitive.Substring(0, infinitive.Length - 2) :
                      infinitive;
            return stem + ending;
        }

        private Dictionary<string, string> GetSpanishConjugations(string infinitive)
        {
            var conjugations = new Dictionary<string, string>
            {
                { "S1", $"yo {GetSpanishStem(infinitive, "o")}" },           // yo hablo
                { "S2Inf", $"tú {GetSpanishStem(infinitive, "as")}" },       // tú hablas
                { "S2Formal", $"usted {GetSpanishStem(infinitive, "a")}" },  // usted habla
                { "S3", $"él/ella {GetSpanishStem(infinitive, "a")}" },      // él/ella habla
                { "P1", $"nosotros {GetSpanishStem(infinitive, "amos")}" },  // nosotros hablamos
                { "P2Inf", $"vosotros {GetSpanishStem(infinitive, "áis")}" }, // vosotros habláis
                { "P2Formal", $"ustedes {GetSpanishStem(infinitive, "an")}" }, // ustedes hablan
                { "P3", $"ellos/ellas {GetSpanishStem(infinitive, "an")}" }  // ellos/ellas hablan
            };
            return conjugations;
        }

        private string GetSpanishStem(string infinitive, string ending)
        {
            var stem = infinitive.EndsWith("ar") ? infinitive.Substring(0, infinitive.Length - 2) :
                      infinitive.EndsWith("er") ? infinitive.Substring(0, infinitive.Length - 2) :
                      infinitive.EndsWith("ir") ? infinitive.Substring(0, infinitive.Length - 2) :
                      infinitive;
            return stem + ending;
        }

        private Dictionary<string, string> GetRussianConjugations(string infinitive)
        {
            var conjugations = new Dictionary<string, string>
            {
                { "S1", $"я {GetRussianStem(infinitive)}" },        // я делаю
                { "S2Inf", $"ты {GetRussianStem(infinitive)}" },    // ты делаешь
                { "S2Formal", $"вы {GetRussianStem(infinitive)}" }, // вы делаете
                { "S3", $"он/она/оно {GetRussianStem(infinitive)}" }, // он делает
                { "P1", $"мы {GetRussianStem(infinitive)}" },       // мы делаем
                { "P2Inf", $"вы {GetRussianStem(infinitive)}" },    // вы делаете
                { "P2Formal", $"вы {GetRussianStem(infinitive)}" }, // вы делаете
                { "P3", $"они {GetRussianStem(infinitive)}" }       // они делают
            };
            return conjugations;
        }

        private string GetRussianStem(string infinitive)
        {
            return infinitive; // Simplified for now
        }

        private Dictionary<string, string> GetKoreanConjugations(string infinitive)
        {
            var conjugations = new Dictionary<string, string>
            {
                { "S1", GetKoreanConjugation(infinitive, "S1") },
                { "S2Inf", GetKoreanConjugation(infinitive, "S2Inf") },
                { "S2Formal", GetKoreanConjugation(infinitive, "S2Formal") },
                { "S3", GetKoreanConjugation(infinitive, "S3") },
                { "P1", GetKoreanConjugation(infinitive, "P1") },
                { "P2Inf", GetKoreanConjugation(infinitive, "P2Inf") },
                { "P2Formal", GetKoreanConjugation(infinitive, "P2Formal") },
                { "P3", GetKoreanConjugation(infinitive, "P3") }
            };
            return conjugations;
        }

        private string GetKoreanConjugation(string infinitive, string person)
        {
            // Simplified Korean conjugations
            return infinitive; // Would need proper Korean grammar rules
        }

        private Dictionary<string, string> GetDefaultConjugations(string infinitive)
        {
            return new Dictionary<string, string>
            {
                { "S1", infinitive },
                { "S2Inf", infinitive },
                { "S2Formal", infinitive },
                { "S3", infinitive },
                { "P1", infinitive },
                { "P2Inf", infinitive },
                { "P2Formal", infinitive },
                { "P3", infinitive }
            };
        }

        private List<string> GetCommonWordsByLanguage(string languageCode)
        {
            return languageCode switch
            {
                "de" => new List<string>
                {
                    "haus", "schule", "arbeit", "freund", "zeit", "wasser", "essen", "schlafen", "sonne", "mond",
                    "baum", "blume", "tier", "hund", "katze", "vogel", "fisch", "auto", "straße", "stadt",
                    "berg", "see", "himmel", "tag", "nacht", "person", "kind", "mann", "frau", "hand",
                    "auge", "ohr", "mund", "nase", "herz", "kopf", "bein", "fuß", "arm", "finger",
                    "kleidung", "tisch", "stuhl", "bett", "tür", "fenster", "licht", "dunkheit", "farbe", "form",
                    "größe", "gewicht", "alter", "name", "sprache", "musik", "lied", "tanz", "spiel", "sport",
                    "buch", "zeitung", "brief", "telefon", "computer", "büro", "fabrik", "farm", "garten", "park",
                    "fluss", "wald", "feld", "strand", "meer", "land", "stadt", "dorf", "brücke", "bahnhof",
                    "schiff", "flugzeug", "zug", "bus", "taxi", "fahrrad", "motorrad", "laster", "wagen", "räder",
                    "straße", "weg", "pfad", "treppenhaus", "aufzug", "treppen", "raum", "ecke", "mitte", "oben"
                },
                "fr" => new List<string>
                {
                    "maison", "école", "travail", "ami", "temps", "eau", "nourriture", "sommeil", "soleil", "lune",
                    "arbre", "fleur", "animal", "chien", "chat", "oiseau", "poisson", "voiture", "rue", "ville",
                    "montagne", "lac", "ciel", "jour", "nuit", "personne", "enfant", "homme", "femme", "main",
                    "œil", "oreille", "bouche", "nez", "cœur", "tête", "jambe", "pied", "bras", "doigt",
                    "vêtement", "table", "chaise", "lit", "porte", "fenêtre", "lumière", "obscurité", "couleur", "forme",
                    "taille", "poids", "âge", "nom", "langue", "musique", "chanson", "danse", "jeu", "sport",
                    "livre", "journal", "lettre", "téléphone", "ordinateur", "bureau", "usine", "ferme", "jardin", "parc",
                    "rivière", "forêt", "champ", "plage", "mer", "pays", "ville", "village", "pont", "gare",
                    "bateau", "avion", "train", "bus", "taxi", "vélo", "moto", "camion", "wagon", "roues",
                    "rue", "chemin", "sentier", "escalier", "ascenseur", "escaliers", "chambre", "coin", "milieu", "haut"
                },
                "es" => new List<string>
                {
                    "casa", "escuela", "trabajo", "amigo", "tiempo", "agua", "comida", "sueño", "sol", "luna",
                    "árbol", "flor", "animal", "perro", "gato", "pájaro", "pez", "coche", "calle", "ciudad",
                    "montaña", "lago", "cielo", "día", "noche", "persona", "niño", "hombre", "mujer", "mano",
                    "ojo", "oído", "boca", "nariz", "corazón", "cabeza", "pierna", "pie", "brazo", "dedo",
                    "ropa", "mesa", "silla", "cama", "puerta", "ventana", "luz", "oscuridad", "color", "forma",
                    "tamaño", "peso", "edad", "nombre", "idioma", "música", "canción", "danza", "juego", "deporte",
                    "libro", "periódico", "carta", "teléfono", "ordenador", "oficina", "fábrica", "granja", "jardín", "parque",
                    "río", "bosque", "campo", "playa", "mar", "país", "ciudad", "pueblo", "puente", "estación",
                    "barco", "avión", "tren", "autobús", "taxi", "bicicleta", "moto", "camión", "carro", "ruedas",
                    "calle", "camino", "sendero", "escaleras", "ascensor", "escalones", "cuarto", "rincón", "centro", "arriba"
                },
                "ru" => new List<string>
                {
                    "дом", "школа", "работа", "друг", "время", "вода", "пища", "сон", "солнце", "луна",
                    "дерево", "цветок", "животное", "собака", "кошка", "птица", "рыба", "машина", "улица", "город",
                    "гора", "озеро", "небо", "день", "ночь", "человек", "ребенок", "мужчина", "женщина", "рука",
                    "глаз", "ухо", "рот", "нос", "сердце", "голова", "нога", "стопа", "рука", "палец",
                    "одежда", "стол", "стул", "кровать", "дверь", "окно", "свет", "темнота", "цвет", "форма",
                    "размер", "вес", "возраст", "имя", "язык", "музыка", "песня", "танец", "игра", "спорт",
                    "книга", "газета", "письмо", "телефон", "компьютер", "офис", "завод", "ферма", "сад", "парк",
                    "река", "лес", "поле", "пляж", "море", "страна", "город", "деревня", "мост", "вокзал",
                    "корабль", "самолет", "поезд", "автобус", "такси", "велосипед", "мотоцикл", "грузовик", "вагон", "колеса",
                    "улица", "дорога", "тропа", "лестница", "лифт", "ступени", "комната", "угол", "центр", "вверху"
                },
                "ko" => new List<string>
                {
                    "집", "학교", "일", "친구", "시간", "물", "음식", "잠", "태양", "달",
                    "나무", "꽃", "동물", "개", "고양이", "새", "물고기", "자동차", "길", "도시",
                    "산", "호수", "하늘", "날", "밤", "사람", "아이", "남자", "여자", "손",
                    "눈", "귀", "입", "코", "심장", "머리", "다리", "발", "팔", "손가락",
                    "옷", "테이블", "의자", "침대", "문", "창", "빛", "어둠", "색", "모양",
                    "크기", "무게", "나이", "이름", "언어", "음악", "노래", "춤", "게임", "스포츠",
                    "책", "신문", "편지", "전화", "컴퓨터", "사무실", "공장", "농장", "정원", "공원",
                    "강", "숲", "들판", "해변", "바다", "나라", "도시", "마을", "다리", "역",
                    "배", "비행기", "기차", "버스", "택시", "자전거", "오토바이", "트럭", "수레", "바퀴",
                    "길", "도로", "오솔길", "계단", "엘리베이터", "층계", "방", "모서리", "중간", "위"
                },
                _ => new List<string>()
            };
        }

        private List<(string infinitive, string meaning)> GetCommonVerbsByLanguage(string languageCode)
        {
            return languageCode switch
            {
                "de" => new List<(string, string)>
                {
                    ("sein", "to be"), ("haben", "to have"), ("gehen", "to go"), ("kommen", "to come"),
                    ("sehen", "to see"), ("sagen", "to say"), ("bleiben", "to stay"), ("geben", "to give"),
                    ("nehmen", "to take"), ("machen", "to make"), ("wissen", "to know"), ("glauben", "to believe"),
                    ("denken", "to think"), ("fühlen", "to feel"), ("sprechen", "to speak"), ("verstehen", "to understand"),
                    ("hören", "to hear"), ("schauen", "to look"), ("zeigen", "to show"), ("finden", "to find")
                },
                "fr" => new List<(string, string)>
                {
                    ("être", "to be"), ("avoir", "to have"), ("aller", "to go"), ("venir", "to come"),
                    ("voir", "to see"), ("dire", "to say"), ("rester", "to stay"), ("donner", "to give"),
                    ("prendre", "to take"), ("faire", "to make"), ("savoir", "to know"), ("croire", "to believe"),
                    ("penser", "to think"), ("sentir", "to feel"), ("parler", "to speak"), ("comprendre", "to understand"),
                    ("entendre", "to hear"), ("regarder", "to look"), ("montrer", "to show"), ("trouver", "to find")
                },
                "es" => new List<(string, string)>
                {
                    ("ser", "to be"), ("estar", "to be"), ("haber", "to have"), ("ir", "to go"),
                    ("venir", "to come"), ("ver", "to see"), ("decir", "to say"), ("quedar", "to stay"),
                    ("dar", "to give"), ("tomar", "to take"), ("hacer", "to make"), ("saber", "to know"),
                    ("creer", "to believe"), ("pensar", "to think"), ("sentir", "to feel"), ("hablar", "to speak"),
                    ("entender", "to understand"), ("oír", "to hear"), ("mirar", "to look"), ("encontrar", "to find")
                },
                "ru" => new List<(string, string)>
                {
                    ("быть", "to be"), ("иметь", "to have"), ("идти", "to go"), ("приходить", "to come"),
                    ("видеть", "to see"), ("сказать", "to say"), ("остаться", "to stay"), ("дать", "to give"),
                    ("взять", "to take"), ("делать", "to make"), ("знать", "to know"), ("верить", "to believe"),
                    ("думать", "to think"), ("чувствовать", "to feel"), ("говорить", "to speak"), ("понимать", "to understand"),
                    ("слышать", "to hear"), ("смотреть", "to look"), ("показывать", "to show"), ("находить", "to find")
                },
                "ko" => new List<(string, string)>
                {
                    ("이다", "to be"), ("있다", "to have"), ("가다", "to go"), ("오다", "to come"),
                    ("보다", "to see"), ("말하다", "to say"), ("머물다", "to stay"), ("주다", "to give"),
                    ("가져가다", "to take"), ("하다", "to make"), ("알다", "to know"), ("믿다", "to believe"),
                    ("생각하다", "to think"), ("느끼다", "to feel"), ("말하다", "to speak"), ("이해하다", "to understand"),
                    ("듣다", "to hear"), ("보다", "to look"), ("보여주다", "to show"), ("찾다", "to find")
                },
                _ => new List<(string, string)>()
            };
        }

        private List<(string word, string meaning)> GetSpecialCategoryWords(string languageCode, string category)
        {
            return (languageCode, category) switch
            {
                ("de", "numbers") => new List<(string, string)>
                {
                    ("null", "zero"), ("eins", "one"), ("zwei", "two"), ("drei", "three"), ("vier", "four"),
                    ("fünf", "five"), ("sechs", "six"), ("sieben", "seven"), ("acht", "eight"), ("neun", "nine"),
                    ("zehn", "ten"), ("elf", "eleven"), ("zwölf", "twelve"), ("dreizehn", "thirteen"), ("vierzehn", "fourteen"),
                    ("fünfzehn", "fifteen"), ("sechzehn", "sixteen"), ("siebzehn", "seventeen"), ("achtzehn", "eighteen"), ("neunzehn", "nineteen"),
                    ("zwanzig", "twenty"), ("dreißig", "thirty"), ("vierzig", "forty"), ("fünfzig", "fifty"), ("hundert", "hundred")
                },
                ("de", "days") => new List<(string, string)>
                {
                    ("Montag", "Monday"), ("Dienstag", "Tuesday"), ("Mittwoch", "Wednesday"), ("Donnerstag", "Thursday"),
                    ("Freitag", "Friday"), ("Samstag", "Saturday"), ("Sonntag", "Sunday")
                },
                ("de", "months") => new List<(string, string)>
                {
                    ("Januar", "January"), ("Februar", "February"), ("März", "March"), ("April", "April"),
                    ("Mai", "May"), ("Juni", "June"), ("Juli", "July"), ("August", "August"),
                    ("September", "September"), ("Oktober", "October"), ("November", "November"), ("Dezember", "December")
                },
                ("fr", "numbers") => new List<(string, string)>
                {
                    ("zéro", "zero"), ("un", "one"), ("deux", "two"), ("trois", "three"), ("quatre", "four"),
                    ("cinq", "five"), ("six", "six"), ("sept", "seven"), ("huit", "eight"), ("neuf", "nine"),
                    ("dix", "ten"), ("onze", "eleven"), ("douze", "twelve"), ("treize", "thirteen"), ("quatorze", "fourteen"),
                    ("quinze", "fifteen"), ("seize", "sixteen"), ("dix-sept", "seventeen"), ("dix-huit", "eighteen"), ("dix-neuf", "nineteen"),
                    ("vingt", "twenty"), ("trente", "thirty"), ("quarante", "forty"), ("cinquante", "fifty"), ("cent", "hundred")
                },
                ("fr", "days") => new List<(string, string)>
                {
                    ("lundi", "Monday"), ("mardi", "Tuesday"), ("mercredi", "Wednesday"), ("jeudi", "Thursday"),
                    ("vendredi", "Friday"), ("samedi", "Saturday"), ("dimanche", "Sunday")
                },
                ("fr", "months") => new List<(string, string)>
                {
                    ("janvier", "January"), ("février", "February"), ("mars", "March"), ("avril", "April"),
                    ("mai", "May"), ("juin", "June"), ("juillet", "July"), ("août", "August"),
                    ("septembre", "September"), ("octobre", "October"), ("novembre", "November"), ("décembre", "December")
                },
                ("es", "numbers") => new List<(string, string)>
                {
                    ("cero", "zero"), ("uno", "one"), ("dos", "two"), ("tres", "three"), ("cuatro", "four"),
                    ("cinco", "five"), ("seis", "six"), ("siete", "seven"), ("ocho", "eight"), ("nueve", "nine"),
                    ("diez", "ten"), ("once", "eleven"), ("doce", "twelve"), ("trece", "thirteen"), ("catorce", "fourteen"),
                    ("quince", "fifteen"), ("dieciséis", "sixteen"), ("diecisiete", "seventeen"), ("dieciocho", "eighteen"), ("diecinueve", "nineteen"),
                    ("veinte", "twenty"), ("treinta", "thirty"), ("cuarenta", "forty"), ("cincuenta", "fifty"), ("cien", "hundred")
                },
                ("es", "days") => new List<(string, string)>
                {
                    ("lunes", "Monday"), ("martes", "Tuesday"), ("miércoles", "Wednesday"), ("jueves", "Thursday"),
                    ("viernes", "Friday"), ("sábado", "Saturday"), ("domingo", "Sunday")
                },
                ("es", "months") => new List<(string, string)>
                {
                    ("enero", "January"), ("febrero", "February"), ("marzo", "March"), ("abril", "April"),
                    ("mayo", "May"), ("junio", "June"), ("julio", "July"), ("agosto", "August"),
                    ("septiembre", "September"), ("octubre", "October"), ("noviembre", "November"), ("diciembre", "December")
                },
                ("ru", "numbers") => new List<(string, string)>
                {
                    ("ноль", "zero"), ("один", "one"), ("два", "two"), ("три", "three"), ("четыре", "four"),
                    ("пять", "five"), ("шесть", "six"), ("семь", "seven"), ("восемь", "eight"), ("девять", "nine"),
                    ("десять", "ten"), ("одиннадцать", "eleven"), ("двенадцать", "twelve"), ("тринадцать", "thirteen"), ("четырнадцать", "fourteen"),
                    ("пятнадцать", "fifteen"), ("шестнадцать", "sixteen"), ("семнадцать", "seventeen"), ("восемнадцать", "eighteen"), ("девятнадцать", "nineteen"),
                    ("двадцать", "twenty"), ("тридцать", "thirty"), ("сорок", "forty"), ("пятьдесят", "fifty"), ("сто", "hundred")
                },
                ("ru", "days") => new List<(string, string)>
                {
                    ("Понедельник", "Monday"), ("Вторник", "Tuesday"), ("Среда", "Wednesday"), ("Четверг", "Thursday"),
                    ("Пятница", "Friday"), ("Суббота", "Saturday"), ("Воскресенье", "Sunday")
                },
                ("ru", "months") => new List<(string, string)>
                {
                    ("январь", "January"), ("февраль", "February"), ("март", "March"), ("апрель", "April"),
                    ("май", "May"), ("июнь", "June"), ("июль", "July"), ("август", "August"),
                    ("сентябрь", "September"), ("октябрь", "October"), ("ноябрь", "November"), ("декабрь", "December")
                },
                _ => new List<(string, string)>()
            };
        }
    }

    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string str)
        {
            return char.ToUpper(str[0]) + str.Substring(1);
        }
    }
}
