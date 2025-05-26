using DTO.FaultReportDtos;
using Frontend.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Frontend.Controllers;

[Authorize(Roles = "Supervisor")]
public class DashboardController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DashboardController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient();

        // Aylık arıza verisi
        var responseMonth = await client.GetAsync("http://localhost:5164/api/FaultReport/GetFaultByMonth");

        // Departman bazlı arıza verisi
        var responseDepartment = await client.GetAsync("http://localhost:5164/api/FaultReport/GetFaultByDepartman");    

        var viewModel = new DashboardViewModel();

        if (responseMonth.IsSuccessStatusCode)
        {
            var jsonData = await responseMonth.Content.ReadAsStringAsync();
            viewModel.FaultsByMonth = JsonConvert.DeserializeObject<List<GetFaultByMonthDto>>(jsonData);    
        }

        if (responseDepartment.IsSuccessStatusCode)
        {
            var jsonData = await responseDepartment.Content.ReadAsStringAsync();
            viewModel.FaultsByDepartment = JsonConvert.DeserializeObject<List<GetFaultByDepartmentDto>>(jsonData);
        }

        return View(viewModel);
    }
}