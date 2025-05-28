using dateManagementHTML.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dateManagementHTML.Controllers
{
    public class HolidayController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HolidayController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetFromDb(int year, string countryCode)
        {
            var holidays = await _context.Holidays
                .Where(h => h.Date.Year == year && h.CountryCode == countryCode)
                .Select(h => new
                {
                    date = h.Date.ToString("yyyy-MM-dd"),
                    name = h.Name
                })
                .ToListAsync();

            return Json(holidays);
        }
    }
}