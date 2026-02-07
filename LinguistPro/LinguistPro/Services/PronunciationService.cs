using LinguistPro.Models;
using Microsoft.EntityFrameworkCore;

namespace LinguistPro.Services
{
    /// <summary>
    /// Service for managing pronunciation guides and audio
    /// </summary>
    public class PronunciationService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHost;

        public PronunciationService(AppDbContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }

        /// <summary>
        /// Get pronunciation data for a word
        /// </summary>
        public async Task<PronunciationData?> GetPronunciationAsync(string word, string languageCode)
        {
            return await _context.PronunciationData
                .FirstOrDefaultAsync(p => p.Word.ToLower() == word.ToLower() 
                    && p.LanguageCode == languageCode);
        }

        /// <summary>
        /// Add or update pronunciation data
        /// </summary>
        public async Task<PronunciationData> AddOrUpdatePronunciationAsync(PronunciationData pronunciation)
        {
            var existing = await _context.PronunciationData
                .FirstOrDefaultAsync(p => p.Word.ToLower() == pronunciation.Word.ToLower() 
                    && p.LanguageCode == pronunciation.LanguageCode);

            if (existing != null)
            {
                existing.IPA = pronunciation.IPA;
                existing.AudioUrl = pronunciation.AudioUrl;
                existing.DifficultySyllables = pronunciation.DifficultySyllables;
                existing.SyllableBreakdown = pronunciation.SyllableBreakdown;
                existing.PronunciationNotes = pronunciation.PronunciationNotes;
                _context.PronunciationData.Update(existing);
                await _context.SaveChangesAsync();
                return existing;
            }

            _context.PronunciationData.Add(pronunciation);
            await _context.SaveChangesAsync();
            return pronunciation;
        }

        /// <summary>
        /// Get pronunciations for a language
        /// </summary>
        public async Task<List<PronunciationData>> GetLanguagePronunciationsAsync(string languageCode)
        {
            return await _context.PronunciationData
                .Where(p => p.LanguageCode == languageCode)
                .OrderBy(p => p.Word)
                .ToListAsync();
        }

        /// <summary>
        /// Record a pronunciation listen
        /// </summary>
        public async Task RecordPlayAsync(int pronunciationId)
        {
            var pronunciation = await _context.PronunciationData
                .FirstOrDefaultAsync(p => p.PronunciationId == pronunciationId);

            if (pronunciation != null)
            {
                pronunciation.PlayCount++;
                _context.PronunciationData.Update(pronunciation);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Search pronunciations by word
        /// </summary>
        public async Task<List<PronunciationData>> SearchPronunciationsAsync(string searchTerm, string languageCode)
        {
            return await _context.PronunciationData
                .Where(p => p.LanguageCode == languageCode 
                    && (p.Word.Contains(searchTerm) || p.IPA.Contains(searchTerm)))
                .OrderByDescending(p => p.PlayCount)
                .Take(10)
                .ToListAsync();
        }

        /// <summary>
        /// Get most popular pronunciations for a language
        /// </summary>
        public async Task<List<PronunciationData>> GetPopularPronunciationsAsync(string languageCode, int limit = 10)
        {
            return await _context.PronunciationData
                .Where(p => p.LanguageCode == languageCode)
                .OrderByDescending(p => p.PlayCount)
                .Take(limit)
                .ToListAsync();
        }

        /// <summary>
        /// Generate audio pronunciation using Azure Speech Services or Google Translate
        /// </summary>
        public async Task<string?> GenerateAudioUrlAsync(string word, string languageCode)
        {
            try
            {
                // Using Google Translate text-to-speech API
                // Format: https://translate.google.com/translate_tts?ie=UTF-8&client=tw-ob&q={word}&tl={lang}
                var langMap = new Dictionary<string, string>
                {
                    { "de", "de" },
                    { "fr", "fr" },
                    { "es", "es" },
                    { "ru", "ru" },
                    { "ko", "ko" }
                };

                if (langMap.TryGetValue(languageCode, out var googleLang))
                {
                    var audioUrl = $"https://translate.google.com/translate_tts?ie=UTF-8&client=tw-ob&q={Uri.EscapeDataString(word)}&tl={googleLang}";
                    return audioUrl;
                }
            }
            catch
            {
                // Fallback if API fails
            }

            return null;
        }

        /// <summary>
        /// Get IPA transcription for a word (basic implementation)
        /// </summary>
        public async Task<string?> GetIPATranscriptionAsync(string word, string languageCode)
        {
            var existing = await GetPronunciationAsync(word, languageCode);
            return existing?.IPA;
        }

        /// <summary>
        /// Batch import pronunciations from CSV
        /// </summary>
        public async Task<int> BatchImportPronunciationsAsync(List<PronunciationData> pronunciations)
        {
            int count = 0;
            foreach (var pronunciation in pronunciations)
            {
                await AddOrUpdatePronunciationAsync(pronunciation);
                count++;
            }
            return count;
        }
    }
}
