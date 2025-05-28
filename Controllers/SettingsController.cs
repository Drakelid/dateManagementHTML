using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using dateManagementHTML.Models.Entities;

namespace dateManagementHTML.Controllers
{
    public class SettingsController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.SavedCountry = user?.PreferredCountryCode; // e.g., "US"
            return View();
        }

        private readonly UserManager<ApplicationUser> _userManager;

        public SettingsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveCountry(string countryCode)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null && !string.IsNullOrEmpty(countryCode))
            {
                user.PreferredCountryCode = countryCode;
                await _userManager.UpdateAsync(user);
            }

            TempData["Message"] = "Preferred country saved.";
            return RedirectToAction("Index", "Settings");
        }
    }
}