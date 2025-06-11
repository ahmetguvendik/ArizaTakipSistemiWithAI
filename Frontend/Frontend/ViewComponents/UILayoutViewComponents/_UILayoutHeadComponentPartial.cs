using Microsoft.AspNetCore.Mvc;

namespace Frontend.ViewComponents.UILayoutViewComponents;

public class _UILayoutHeadComponentPartial  : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}