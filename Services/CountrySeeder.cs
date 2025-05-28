using dateManagementHTML.Data;

namespace dateManagementHTML.Services
{
    public class CountrySeeder
    {
        private readonly ApplicationDbContext _context;
        private readonly NagerApiService _api;

        public CountrySeeder(ApplicationDbContext context, NagerApiService api)
        {
            _context = context;
            _api = api;
        }

        public async Task SeedCountriesAsync()
        {
            var countries = await _api.FetchAvailableCountriesAsync();

            // Optional: clear old ones first
            _context.Countries.RemoveRange(_context.Countries);
            _context.Countries.AddRange(countries);
            await _context.SaveChangesAsync();
        }
    }
}
