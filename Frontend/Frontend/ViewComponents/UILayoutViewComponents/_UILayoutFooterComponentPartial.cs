using Microsoft.AspNetCore.Mvc;

namespace Frontend.ViewComponents.UILayoutViewComponents;

public class _UILayoutFooterComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}