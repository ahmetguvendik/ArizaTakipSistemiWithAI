using System.Text;
using DTO.FaultReportDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Frontend.Controllers;

public class ArizaController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ArizaController(IHttpClientFactory httpClientFactory)
    {
         _httpClientFactory = httpClientFactory;
    }
    
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Index(CreateFaultReportDto createJobDto)
    {
        createJobDto.CreatedAt = DateTime.Now;
        var client = _httpClientFactory.CreateClient();
        var jsonData = JsonConvert.SerializeObject(createJobDto);
        StringContent stringContent = new StringContent(jsonData, encoding: Encoding.UTF8, "application/json");
        var response = await client.PostAsync("http://localhost:5214/api/Job", stringContent);
        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index", "Company");    
        }

        return View();
           
    }
}