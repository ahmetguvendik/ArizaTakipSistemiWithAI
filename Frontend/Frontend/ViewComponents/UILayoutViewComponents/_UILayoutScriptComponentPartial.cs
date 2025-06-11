using Microsoft.AspNetCore.Mvc;

namespace Frontend.ViewComponents.UILayoutViewComponents;

public class _UILayoutScriptComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}