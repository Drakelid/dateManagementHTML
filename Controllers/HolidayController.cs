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
        public IActionResult GetFromDb(int year, string countryCode)
        {
            var holidays = _context.Holidays
                .Where(h => h.IsActive && h.Date.Year == year && h.CountryCode == countryCode)
                .Select(h => new {
                    h.Name,
                    date = h.Date.ToString("yyyy-MM-dd")
                })
                .ToList();

            return Json(holidays);
        }
    }
}