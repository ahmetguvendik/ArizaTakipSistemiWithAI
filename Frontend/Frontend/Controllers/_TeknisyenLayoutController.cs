using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;

[Authorize(Roles = "Teknisyen")]
public class _TeknisyenLayoutController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}