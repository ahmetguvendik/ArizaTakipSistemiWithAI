using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;

public class AiController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}