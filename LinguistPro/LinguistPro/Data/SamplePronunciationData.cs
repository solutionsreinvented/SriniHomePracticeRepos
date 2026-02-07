// Script to create sample pronunciation data
// This helps demonstrate the pronunciation guide feature

using LinguistPro.Models;

namespace LinguistPro.Data
{
    public class SamplePronunciationData
    {
        public static List<PronunciationData> GetSamplePronunciations()
        {
            return new List<PronunciationData>
            {
                // German
                new PronunciationData
                {
                    Word = "hallo",
                    IPA = "/ˈhaloː/",
                    LanguageCode = "de",
                    SyllableBreakdown = "HAL-LO",
                    PronunciationNotes = "Long 'o' sound, similar to English 'hello'",
                    DifficultySyllables = "simple",
                    AudioUrl = "/audio/pronunciations/de-hallo.mp3"
                },
                new PronunciationData
                {
                    Word = "danke",
                    IPA = "/ˈdɑŋkə/",
                    LanguageCode = "de",
                    SyllableBreakdown = "DAHN-KUH",
                    PronunciationNotes = "Nasal sound before 'ng', schwa sound at end",
                    DifficultySyllables = "simple",
                    AudioUrl = "/audio/pronunciations/de-danke.mp3"
                },
                new PronunciationData
                {
                    Word = "schmetterling",
                    IPA = "/ˈʃmɛtɐlɪŋ/",
                    LanguageCode = "de",
                    SyllableBreakdown = "SHMET-TER-LING",
                    PronunciationNotes = "'sch' sounds like English 'sh', rolled 'r' is optional",
                    DifficultySyllables = "complex"
                },

                // French
                new PronunciationData
                {
                    Word = "bonjour",
                    IPA = "/bɔ̃ʒuʁ/",
                    LanguageCode = "fr",
                    SyllableBreakdown = "BON-JOUR",
                    PronunciationNotes = "Nasal sound on 'on', rolled 'r' at end, 'ou' like English 'oo'",
                    DifficultySyllables = "moderate",
                    AudioUrl = "/audio/pronunciations/fr-bonjour.mp3"
                },
                new PronunciationData
                {
                    Word = "merci",
                    IPA = "/meʁsi/",
                    LanguageCode = "fr",
                    SyllableBreakdown = "MER-SEE",
                    PronunciationNotes = "Rolled 'r', 'ci' pronounced like 'see'",
                    DifficultySyllables = "simple"
                },
                new PronunciationData
                {
                    Word = "croissant",
                    IPA = "/kʁwasɑ̃/",
                    LanguageCode = "fr",
                    SyllableBreakdown = "KRWA-SAHN",
                    PronunciationNotes = "Rolled 'r', nasal 'an' sound at end",
                    DifficultySyllables = "moderate"
                },

                // Spanish
                new PronunciationData
                {
                    Word = "hola",
                    IPA = "/ˈola/",
                    LanguageCode = "es",
                    SyllableBreakdown = "O-LA",
                    PronunciationNotes = "Silent 'h', clear vowel sounds",
                    DifficultySyllables = "simple",
                    AudioUrl = "/audio/pronunciations/es-hola.mp3"
                },
                new PronunciationData
                {
                    Word = "gracias",
                    IPA = "/ˈɡɾasjas/",
                    LanguageCode = "es",
                    SyllableBreakdown = "GRA-CIAS",
                    PronunciationNotes = "Rolled 'r', 'cias' like 'thee-ahs' in some regions",
                    DifficultySyllables = "moderate"
                },
                new PronunciationData
                {
                    Word = "hermoso",
                    IPA = "/erˈmoso/",
                    LanguageCode = "es",
                    SyllableBreakdown = "ER-MO-SO",
                    PronunciationNotes = "Rolled 'r', clear vowels",
                    DifficultySyllables = "moderate"
                },

                // Russian
                new PronunciationData
                {
                    Word = "привет",
                    IPA = "/prɪˈvʲet/",
                    LanguageCode = "ru",
                    SyllableBreakdown = "pri-VET",
                    PronunciationNotes = "Soft 'y' sound before 'e', stress on second syllable",
                    DifficultySyllables = "moderate"
                },
                new PronunciationData
                {
                    Word = "спасибо",
                    IPA = "/spəˈsʲibə/",
                    LanguageCode = "ru",
                    SyllableBreakdown = "spa-SI-ba",
                    PronunciationNotes = "Stress on second syllable, soft 's' sound",
                    DifficultySyllables = "moderate"
                },
                new PronunciationData
                {
                    Word = "пожалуйста",
                    IPA = "/pəˈʒɑlstə/",
                    LanguageCode = "ru",
                    SyllableBreakdown = "pa-ZHAHL-sta",
                    PronunciationNotes = "'zh' like measure in English, stress on second syllable",
                    DifficultySyllables = "complex"
                },

                // Korean
                new PronunciationData
                {
                    Word = "안녕하세요",
                    IPA = "/ɑnnjʌŋhɑseːjo/",
                    LanguageCode = "ko",
                    SyllableBreakdown = "AN-NYEONG-HA-SE-YO",
                    PronunciationNotes = "Formal polite greeting, 'nn' is a double sound",
                    DifficultySyllables = "moderate"
                },
                new PronunciationData
                {
                    Word = "감사합니다",
                    IPA = "/kɑmsɑhɑmnidɑ/",
                    LanguageCode = "ko",
                    SyllableBreakdown = "GAM-SA-HAM-NI-DA",
                    PronunciationNotes = "Formal thank you, clear syllable separation",
                    DifficultySyllables = "moderate"
                },
                new PronunciationData
                {
                    Word = "미안해요",
                    IPA = "/miɑnhɛjo/",
                    LanguageCode = "ko",
                    SyllableBreakdown = "MI-AN-HAE-YO",
                    PronunciationNotes = "Casual apology, rising tone at end for politeness",
                    DifficultySyllables = "simple"
                }
            };
        }
    }
}

// Usage in Program.cs during app initialization:
/*
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    // Seed pronunciation data
    if (!db.PronunciationData.Any())
    {
        var pronunciations = SamplePronunciationData.GetSamplePronunciations();
        db.PronunciationData.AddRange(pronunciations);
        await db.SaveChangesAsync();
    }
}
*/
