using Microsoft.AspNetCore.Mvc;

namespace Frontend.ViewComponents.TeknisyenViewComponents;

public class _TeknisyenSidebarComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}