using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;

public class _SupervisorLayoutController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}