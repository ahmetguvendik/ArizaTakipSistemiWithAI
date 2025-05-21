using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;

public class TeknisyenController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}