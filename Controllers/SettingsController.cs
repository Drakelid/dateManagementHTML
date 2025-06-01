using dateManagementHTML.Data;
using dateManagementHTML.Models.Entities;
using dateManagementHTML.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace dateManagementHTML.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public SettingsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            ViewData["ProfileModel"] = new UpdateProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            ViewData["PasswordModel"] = new UpdatePasswordViewModel();

            ViewBag.SavedCountry = await GetCountrySelectList(user.PreferredCountryCode);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateProfileViewModel model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Index");

            var user = await _userManager.GetUserAsync(User);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.Email;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["Message"] = "Profile updated successfully.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Password change failed: Invalid data.";
                return RedirectToAction("Index");
            }

            var user = await _userManager.GetUserAsync(User);
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["Message"] = "Password changed successfully.";
            }
            else
            {
                TempData["Message"] = "Password change failed: " + string.Join(", ", result.Errors.Select(e => e.Description));
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> SaveCountry(string preferredCountryCode)
        {
            var user = await _userManager.GetUserAsync(User);
            user.PreferredCountryCode = preferredCountryCode;
            await _userManager.UpdateAsync(user);

            TempData["Message"] = "Country updated successfully.";
            return RedirectToAction("Index");
        }

        private async Task<List<SelectListItem>> GetCountrySelectList(string selectedCode)
        {
            return await _context.Countries
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.CountryCode,
                    Text = $"{c.Name} ({c.CountryCode})",
                    Selected = c.CountryCode == selectedCode
                }).ToListAsync();
        }
    }
}