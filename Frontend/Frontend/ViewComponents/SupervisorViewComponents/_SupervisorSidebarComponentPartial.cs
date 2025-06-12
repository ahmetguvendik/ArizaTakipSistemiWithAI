using Microsoft.AspNetCore.Mvc;

namespace Frontend.ViewComponents.SupervisorViewComponents;

public class _SupervisorSidebarComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}