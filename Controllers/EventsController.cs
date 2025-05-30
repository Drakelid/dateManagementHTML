using dateManagementHTML.Data;
using dateManagementHTML.Models;
using dateManagementHTML.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dateManagementHTML.Controllers
{
    [Authorize]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                // Not logged in or user can't be resolved
                return Unauthorized();
            }

            model.UserId = userId;
            model.CreatedAt = DateTime.UtcNow;

            _context.Events.Add(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }



        public async Task<IActionResult> Index(string filter = "all", string tag = "all")
        {
            var userId = _userManager.GetUserId(User);

            var eventsQuery = _context.Events
                .Where(e => e.UserId == userId);

            var today = DateTime.Today;

            // Filter by date
            eventsQuery = filter switch
            {
                "today" => eventsQuery.Where(e => e.StartDateTime.Date == today),
                "upcoming" => eventsQuery.Where(e => e.StartDateTime.Date > today),
                "past" => eventsQuery.Where(e => e.StartDateTime.Date < today),
                _ => eventsQuery
            };

            if (tag != "all")
            {
                eventsQuery = eventsQuery.Where(e => e.Tag.ToLower() == tag.ToLower());
            }

            var events = await eventsQuery
                .OrderBy(e => e.StartDateTime)
                .ToListAsync();

            return View(events);
        }

        // GET: Events/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            var evt = await _context.Events.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (evt == null)
                return NotFound();

            return View(evt);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Event model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = _userManager.GetUserId(User);
            var existing = await _context.Events.AsNoTracking().FirstOrDefaultAsync(e => e.Id == model.Id && e.UserId == userId);

            if (existing == null)
                return NotFound();

            model.UserId = userId;
            _context.Events.Update(model);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // POST: Events/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var evt = await _context.Events.FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (evt == null)
                return NotFound();

            _context.Events.Remove(evt);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> GetWorkingDays(DateTime start, DateTime end)
        {
            if (end < start)
                return Json(new { workingDays = 0, totalDays = 0, weekends = 0, holidays = 0, excludedHolidayList = new List<object>() });

            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null || string.IsNullOrWhiteSpace(user.PreferredCountryCode))
                return BadRequest("Preferred country not set for this user.");

            var countryCode = user.PreferredCountryCode;

            var allHolidays = await _context.Holidays
                .Where(h => h.CountryCode == countryCode)
                .ToListAsync();

            var holidaysInRange = allHolidays
                .Where(h => h.Date.Date >= start.Date && h.Date.Date <= end.Date)
                .ToList();

            Console.WriteLine("Holiday entries returned:");
            foreach (var h in holidaysInRange)
            {
                Console.WriteLine($"{h.Name} - {h.Date}");
            }


            var days = Enumerable
                .Range(0, (end.Date - start.Date).Days + 1)
                .Select(offset => start.Date.AddDays(offset))
                .ToList();

            int totalDays = days.Count;
            int weekendDays = days.Count(d => d.DayOfWeek == DayOfWeek.Saturday || d.DayOfWeek == DayOfWeek.Sunday);
            int holidayDays = holidaysInRange.Count(d => !IsWeekend(d.Date));
            int workingDays = days.Count(d =>
                d.DayOfWeek != DayOfWeek.Saturday &&
                d.DayOfWeek != DayOfWeek.Sunday &&
                !holidaysInRange.Any(h => h.Date.Date == d));

            return Json(new
            {
                workingDays,
                totalDays,
                weekends = weekendDays,
                holidays = holidayDays,
                excludedHolidayList = holidaysInRange.Select(h => new
                {
                    h.Name,
                    Date = h.Date.ToString("yyyy-MM-dd")
                })
            });
        }

        private bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

    }
}
