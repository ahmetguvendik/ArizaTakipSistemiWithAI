using DTO.FaultReportDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Frontend.Controllers;

public class SupervisorController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public SupervisorController(IHttpClientFactory httpClientFactory)
    {
         _httpClientFactory = httpClientFactory;
    }
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync("http://localhost:5164/api/FaultReport");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<GetFaultReportDto>>(jsonData);
            return View(values);
        }
        return View();
    }

    public async Task<IActionResult> ArizaDetay(string id)
    {
        return View();
    }
    

}