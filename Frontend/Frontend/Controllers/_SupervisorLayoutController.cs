using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;

[Authorize(Roles = "Supervisor")]   
public class _SupervisorLayoutController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}