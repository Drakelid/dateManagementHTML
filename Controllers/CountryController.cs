using dateManagementHTML.Data;
using dateManagementHTML.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dateManagementHTML.Controllers
{
    public class CountryController : Controller
    {
        private readonly CountrySeeder _countrySeeder;
        private readonly ApplicationDbContext _context;

        public CountryController(CountrySeeder seeder, ApplicationDbContext context)
        {
            _countrySeeder = seeder;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
