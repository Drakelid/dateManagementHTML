using Microsoft.AspNetCore.Mvc;

namespace dateManagementHTML.Controllers
{
    public class CalendarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
