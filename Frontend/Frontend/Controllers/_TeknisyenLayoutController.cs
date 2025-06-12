using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;

public class _TeknisyenLayoutController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}