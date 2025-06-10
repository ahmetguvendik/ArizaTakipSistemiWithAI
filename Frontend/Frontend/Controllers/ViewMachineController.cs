using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;

public class ViewMachineController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}