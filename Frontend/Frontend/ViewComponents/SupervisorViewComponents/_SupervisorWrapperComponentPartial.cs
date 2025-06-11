using Microsoft.AspNetCore.Mvc;

namespace Frontend.ViewComponents.SupervisorViewComponents;

public class _SupervisorWrapperComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}