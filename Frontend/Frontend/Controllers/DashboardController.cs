using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;

[Authorize(Roles = "Supervisor")]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}