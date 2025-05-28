using dateManagementHTML.Data;
using dateManagementHTML.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dateManagementHTML.Controllers
{
    public class CountryController : Controller
    {
        private readonly CountrySeeder _countrySeeder;
        private readonly ApplicationDbContext _context;

        public CountryController(CountrySeeder seeder, ApplicationDbContext context)
        {
            _countrySeeder = seeder;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: /Country/Sync
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Sync()
        {
            await _countrySeeder.SeedCountriesAsync();
            TempData["Message"] = "Country list successfully updated from Nager.Date.";
            return RedirectToAction("Index", "Admin");
        }

        // GET: /Country/List (used by dropdown)
        [HttpGet]
        public IActionResult List()
        {
            var countries = _context.Countries
                .OrderBy(c => c.Name)
                .Select(c => new { c.CountryCode, c.Name })
                .ToList();

            return Json(countries);
        }
    }
}
