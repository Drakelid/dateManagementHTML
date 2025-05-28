using dateManagementHTML.Models.Entities;

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
    }
}
