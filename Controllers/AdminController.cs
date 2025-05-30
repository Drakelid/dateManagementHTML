using dateManagementHTML.Data;
using dateManagementHTML.Models.Entities;
using dateManagementHTML.Models.ViewModels;
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

        public async Task<IActionResult> Index(string countryCode = null, string tab = "sync")
        {
            var countries = await _context.Countries
                .OrderBy(c => c.Name)
                .ToListAsync();

            var holidaysQuery = _context.Holidays.AsQueryable();

            if (!string.IsNullOrWhiteSpace(countryCode))
            {
                holidaysQuery = holidaysQuery.Where(h => h.CountryCode == countryCode);
            }

            var model = new AdminHolidayViewModel
            {
                AvailableCountries = countries,
                Holidays = await holidaysQuery.OrderBy(h => h.Date).ToListAsync(),
                SelectedCountryCode = countryCode
            };

            ViewBag.ActiveTab = tab;
            return View(model);
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
                    var existing = _context.Holidays
    .FirstOrDefault(x => x.Date == holidayDate && x.CountryCode == countryCode);

                    if (existing == null)
                    {
                        _context.Holidays.Add(new Holiday
                        {
                            Name = h.LocalName,
                            Date = holidayDate,
                            CountryCode = countryCode,
                            IsActive = true,
                            IsCustom = false
                        });

                        totalAdded++;
                    }
                    else
                    {
                        // Optionally update name if it changed
                        if (existing.Name != h.LocalName)
                        {
                            existing.Name = h.LocalName;
                        }

                        // If previously disabled, leave it as-is to preserve admin intent
                        // You could also choose to auto-activate if you want:
                        // if (!existing.IsActive) existing.IsActive = true;
                    }
                }
            }

            await _context.SaveChangesAsync();

            TempData["Message"] = $"Synced holidays for {countries.Count} countries. {totalAdded} new holidays added.";
            return RedirectToAction("Index", "Admin");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Holidays(string? countryCode = null)
        {
            var holidaysQuery = _context.Holidays.AsQueryable();

            if (!string.IsNullOrWhiteSpace(countryCode))
            {
                holidaysQuery = holidaysQuery.Where(h => h.CountryCode == countryCode);
            }

            var model = new AdminHolidayViewModel
            {
                Holidays = holidaysQuery.OrderBy(h => h.Date).ToList(),
                SelectedCountryCode = countryCode,
                AvailableCountries = _context.Countries.OrderBy(c => c.Name).ToList() // ✅ ensure this is never null
            };

            return View("Index", model); // or just View(model) if Index.cshtml is the correct view
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddHoliday() => View();

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddHoliday(Holiday model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.IsCustom = true;
            _context.Holidays.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Admin", new { tab = "holidays", countryCode = model.CountryCode });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteHoliday(int id)
        {
            var holiday = await _context.Holidays.FindAsync(id);
            if (holiday == null || !holiday.IsCustom) return NotFound();

            _context.Holidays.Remove(holiday);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Admin", new { tab = "holidays", countryCode = holiday.CountryCode });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleHoliday(int id)
        {
            var holiday = await _context.Holidays.FindAsync(id);
            if (holiday == null)
                return NotFound();

            holiday.IsActive = !holiday.IsActive;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Admin", new { tab = "holidays", countryCode = holiday.CountryCode });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditHoliday(int id)
        {
            var holiday = await _context.Holidays.FindAsync(id);
            if (holiday == null || !holiday.IsCustom) return NotFound();

            return View(holiday);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditHoliday(Holiday model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existing = await _context.Holidays.FindAsync(model.Id);
            if (existing == null || !existing.IsCustom)
                return NotFound();

            existing.Name = model.Name;
            existing.Date = model.Date;
            existing.CountryCode = model.CountryCode;
            existing.IsActive = model.IsActive;

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Admin", new { tab = "holidays", countryCode = model.CountryCode });
        }

    }
}
