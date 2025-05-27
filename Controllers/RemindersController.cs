using Microsoft.AspNetCore.Mvc;

namespace dateManagementHTML.Controllers
{
    public class RemindersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
