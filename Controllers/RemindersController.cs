using dateManagementHTML.Data;
using dateManagementHTML.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dateManagementHTML.Controllers
{
    [Authorize]
    public class RemindersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RemindersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
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
        public async Task<IActionResult> Create(Reminder model)
        {
            Console.WriteLine("📩 Entered POST /Reminders/Create");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("❌ Reminder form failed validation:");
                foreach (var key in ModelState.Keys)
                {
                    foreach (var error in ModelState[key].Errors)
                    {
                        Console.WriteLine($" - {key}: {error.ErrorMessage}");
                    }
                }
                return View(model);
            }

            var userId = _userManager.GetUserId(User);
            Console.WriteLine($"👤 Attempting to assign UserId: '{userId}'");

            if (string.IsNullOrWhiteSpace(userId))
            {
                Console.WriteLine("❌ No logged-in user found.");
                return Unauthorized();
            }



            if (string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("❌ UserId is null — user not authenticated?");
                return Unauthorized(); // or Redirect to Login
            }

            model.UserId = userId;
            model.CreatedAt = DateTime.UtcNow;

            _context.Reminders.Add(model);
            await _context.SaveChangesAsync();

            Console.WriteLine("✅ Reminder saved!");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var nextThursday = today.AddDays(((int)DayOfWeek.Thursday - (int)today.DayOfWeek + 7) % 7);

            var reminders = await _context.Reminders
                .Where(r => r.UserId == userId)
                .OrderBy(r => r.Date)
                .ThenBy(r => r.Time) // ✅ This line ensures morning-first sorting
                .ToListAsync();


            ViewBag.Today = reminders.Where(r => r.Date.Date == today).ToList();
            ViewBag.Tomorrow = reminders.Where(r => r.Date.Date == tomorrow).ToList();
            ViewBag.Thursday = reminders.Where(r => r.Date.DayOfWeek == DayOfWeek.Thursday).ToList();

            return View(reminders);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleComplete(int id)
        {
            var reminder = await _context.Reminders.FindAsync(id);
            if (reminder == null) return NotFound();

            // Flip the status
            reminder.IsCompleted = !reminder.IsCompleted;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
