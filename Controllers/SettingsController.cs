using Microsoft.AspNetCore.Mvc;

namespace dateManagementHTML.Controllers
{
    public class SettingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
