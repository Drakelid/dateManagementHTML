using dateManagementHTML.Data;
using dateManagementHTML.Models.Entities;
using dateManagementHTML.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dateManagementHTML.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly NagerApiService _nagerApiService;
        private readonly CountrySeeder _countrySeeder;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(
            ApplicationDbContext context,
            NagerApiService nagerApiService,
            CountrySeeder countrySeeder,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _nagerApiService = nagerApiService;
            _countrySeeder = countrySeeder;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Sync()
        {
            await _countrySeeder.SeedCountriesAsync();
            TempData["Message"] = "Country list successfully updated from Nager.Date.";
            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public IActionResult List()
        {
            var countries = _context.Countries
                .OrderBy(c => c.Name)
                .Select(c => new { c.CountryCode, c.Name })
                .ToList();

            return Json(countries);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SyncHolidays(int year)
        {
            var countries = await _context.Countries.Select(c => c.CountryCode).ToListAsync();
            int totalAdded = 0;

            foreach (var countryCode in countries)
            {
                var holidays = await _nagerApiService.FetchHolidaysAsync(year, countryCode);

                foreach (var h in holidays)
                {
                    var holidayDate = DateTime.Parse(h.Date);
                    bool exists = _context.Holidays.Any(x => x.Date == holidayDate && x.CountryCode == countryCode);

                    if (!exists)
                    {
                        _context.Holidays.Add(new Holiday
                        {
                            Name = h.LocalName,
                            Date = holidayDate,
                            CountryCode = countryCode
                        });

                        totalAdded++;
                    }
                }
            }

            await _context.SaveChangesAsync();

            TempData["Message"] = $"Synced holidays for {countries.Count} countries. {totalAdded} new holidays added.";
            return RedirectToAction("Index", "Admin");
        }
    }
}
