using Microsoft.AspNetCore.Mvc;

namespace Frontend.ViewComponents.TeknisyenViewComponents;

public class _TeknisyenTopBarComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}