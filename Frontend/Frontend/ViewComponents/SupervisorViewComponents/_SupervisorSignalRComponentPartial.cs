using Microsoft.AspNetCore.Mvc;

namespace Frontend.ViewComponents.SupervisorViewComponents;

public class _SupervisorSignalRComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}