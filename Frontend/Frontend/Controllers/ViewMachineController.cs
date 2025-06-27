using System.Security.Claims;
using DTO.MachuneDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace Frontend.Controllers;

[Authorize(Roles = "Teknisyen")]
public class ViewMachineController : Controller
{
    private readonly IHttpClientFactory _clientFactory;

    public ViewMachineController(IHttpClientFactory clientFactory)
    {
         _clientFactory = clientFactory;
    }
    
    public async Task<IActionResult> Index()
    {
        var departmanId = User.Identity.IsAuthenticated ? User.FindFirstValue("DepartmentId") : null;       
        ViewBag.DepartmentId = departmanId;
        var client = _clientFactory.CreateClient();
        var response = await client.GetAsync($"http://testapi.solfix.help:5164/api/Machine/GetMachineByDepartmanId/{departmanId}");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<GetAllMachineByDepartmanIdDto>>(json);
            return View(values);
        }
        return View();
    }
}