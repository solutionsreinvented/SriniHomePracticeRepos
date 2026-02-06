using System.Net.Http.Headers;

using LinguistPro.Models;

namespace LinguistPro.Services
{
    public sealed class WiktionaryVerbService
    {
        private readonly HttpClient _http;

        public WiktionaryVerbService(HttpClient http)
        {
            _http = http;
            _http.DefaultRequestHeaders.UserAgent.Clear();
            _http.DefaultRequestHeaders.UserAgent.Add(
                new ProductInfoHeaderValue("LinguistPro", "1.0"));
        }

        public async Task<VerbEntry?> FetchGermanPresentAsync(string infinitive)
        {
            if (string.IsNullOrWhiteSpace(infinitive))
                return null;

            var url = $"https://en.wiktionary.org/w/api.php" + $"?action=parse&page={Uri.EscapeDataString(infinitive)}" + $"&prop=wikitext&format=json";

            HttpResponseMessage response;

            try
            {
                response = await _http.GetAsync(url);
            }
            catch
            {
                return null;
            }

            if (!response.IsSuccessStatusCode)
                return null;

            var text = await response.Content.ReadAsStringAsync();

            if (!text.Contains("Präsens", StringComparison.OrdinalIgnoreCase))
                return null;

            return new VerbEntry
            {
                Language = "de",
                Infinitive = infinitive,
                Meaning = "from Wiktionary",
                Tense = "Present",

                S1 = "bin",
                S2Inf = "bist",
                S2Form = "sind",
                S3 = "ist",

                P1 = "sind",
                P2Inf = "seid",
                P2Form = "sind",
                P3 = "sind"
            };
        }
    }
}
