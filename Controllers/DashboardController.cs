using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using dateManagementHTML.Models;
using Microsoft.AspNetCore.Authorization;

namespace dateManagementHTML.Controllers;

public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
