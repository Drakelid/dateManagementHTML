using dateManagementHTML.Data;
using dateManagementHTML.Models.Entities; // Needed for ApplicationUser
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace dateManagementHTML.Controllers
{
    public class CalendarController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CalendarController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.SavedCountry = user?.PreferredCountryCode ?? "NO"; // Fallback to Norway if not set
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents(string date)
        {
            if (!DateTime.TryParse(date, out var parsedDate))
                return BadRequest("Invalid date format");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var events = await _context.Events
                .Where(e => e.StartDateTime.Date == parsedDate.Date && e.UserId == userId)
                .Select(e => new
                {
                    Title = e.EventName,
                    Time = e.StartDateTime.ToString("HH:mm"),
                    Type = e.Tag
                })
                .ToListAsync();

            return Json(events);
        }
    }
}
