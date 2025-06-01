using dateManagementHTML.Data;
using dateManagementHTML.Models.Entities;
using dateManagementHTML.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace dateManagementHTML.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var today = DateTime.Today;

            var todayEvents = await _context.Events
            .Where(e => e.UserId == userId
                     && e.StartDateTime.Date == today
                     && !e.IsCompleted)
            .ToListAsync();


            var todayReminders = await _context.Reminders
                .Where(r => r.UserId == userId && r.Date.Date == today)
                .ToListAsync();


            var completedCount = todayEvents.Count(e => e.IsCompleted);
            var totalCount = todayEvents.Count;

            var viewModel = new DashboardViewModel
            {
                TodayEvents = todayEvents,
                TodayReminders = todayReminders,
                CompletedEventCount = completedCount,
                TotalEventCount = totalCount
            };

            return View(viewModel);
        }
    }
}