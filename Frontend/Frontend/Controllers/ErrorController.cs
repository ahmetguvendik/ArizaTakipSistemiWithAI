using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;

public class ErrorController : Controller
{
    [Route("Error/404")]
    public IActionResult Error404()
    {
        return View("NotFound");
    }

    [Route("Error/403")]
    public IActionResult Error403()
    {
        return View("AccessDenied");
    }

    [Route("Error/{statusCode}")]
    public IActionResult HandleError(int statusCode)
    {
        return statusCode switch
        {
            404 => RedirectToAction("Error404"),
            403 => RedirectToAction("Error403"),
            _ => View("Error")
        };
    }
}