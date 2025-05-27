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
                return Unauthorized(); // or RedirectToAction("Login", "Account");
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

            // Filter by tag
            if (tag != "all")
            {
                eventsQuery = eventsQuery.Where(e => e.Tag.ToLower() == tag.ToLower());
            }

            var events = await eventsQuery
                .OrderBy(e => e.StartDateTime)
                .ToListAsync();

            return View(events);
        }
    }
}
