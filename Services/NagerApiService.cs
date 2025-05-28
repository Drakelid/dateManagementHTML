using dateManagementHTML.Models.Entities;
using System.Text.Json.Serialization;

namespace dateManagementHTML.Services
{
    public class NagerApiService
    {
        private readonly HttpClient _httpClient;

        public NagerApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Country>> FetchAvailableCountriesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<NagerCountry>>("https://date.nager.at/api/v3/AvailableCountries");
            return response.Select(c => new Country { CountryCode = c.CountryCode, Name = c.Name }).ToList();
        }

        private class NagerCountry
        {
            public string CountryCode { get; set; }
            public string Name { get; set; }
        }

        public async Task<List<PublicHoliday>> FetchHolidaysAsync(int year, string countryCode)
        {
            var url = $"https://date.nager.at/api/v3/PublicHolidays/{year}/{countryCode}";
            var response = await _httpClient.GetFromJsonAsync<List<PublicHoliday>>(url);
            return response ?? new List<PublicHoliday>();
        }

        public class PublicHoliday
        {
            [JsonPropertyName("date")]
            public string Date { get; set; } // e.g., "2025-05-17"
            [JsonPropertyName("localName")]
            public string LocalName { get; set; } // e.g., "Grunnlovsdag"
            [JsonPropertyName("name")]
            public string Name { get; set; } // e.g., "Constitution Day"
            [JsonPropertyName("global")]
            public bool Global { get; set; }
            [JsonPropertyName("countryCode")]
            public string CountryCode { get; set; }
            [JsonPropertyName("fixed")]
            public bool Fixed { get; set; }
            [JsonPropertyName("type")]
            public string Type { get; set; }
        }

    }
}
