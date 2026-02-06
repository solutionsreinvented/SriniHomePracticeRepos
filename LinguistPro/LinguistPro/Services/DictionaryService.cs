using LinguistPro.Models;

namespace LinguistPro.Services
{
    internal sealed class DictionaryApiResponse
    {
        public List<DictionaryMeaning> Meanings { get; set; } = [];
    }

    internal sealed class DictionaryMeaning
    {
        public string PartOfSpeech { get; set; } = string.Empty;
        public List<DictionaryDefinition> Definitions { get; set; } = [];
    }

    internal sealed class DictionaryDefinition
    {
        public string Definition { get; set; } = string.Empty;
        public string? Example { get; set; }
    }

    public sealed class DictionaryService
    {
        private readonly HttpClient _http;

        public DictionaryService(HttpClient http)
        {
            _http = http;
        }

        public async Task<VocabularyItem?> FetchAsync(string term, string language)
        {
            if (string.IsNullOrWhiteSpace(term))
                return null;

            var url = $"https://api.dictionaryapi.dev/api/v2/entries/en/{term.ToLowerInvariant()}";

            List<DictionaryApiResponse>? response;

            try
            {
                response = await _http.GetFromJsonAsync<List<DictionaryApiResponse>>(url);
            }
            catch
            {
                return null;
            }

            if (response == null || response.Count == 0)
                return null;

            var meaning = response[0].Meanings.FirstOrDefault();
            if (meaning == null || meaning.Definitions.Count == 0)
                return null;

            var def = meaning.Definitions[0];

            return new VocabularyItem
            {
                Language = language,
                Term = term,
                Meaning = def.Definition,
                Definition = def.Definition,
                UsageExample = def.Example ?? string.Empty
            };
        }
    }

}
