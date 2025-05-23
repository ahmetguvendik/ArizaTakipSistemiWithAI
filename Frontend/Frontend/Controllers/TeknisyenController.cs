using System.Security.Claims;
using Application.Services;
using DTO.FaultReportDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Frontend.Controllers;

[Authorize(Roles = "Teknisyen")]
public class TeknisyenController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IEmailService  _emailService;

    public TeknisyenController(IHttpClientFactory httpClientFactory, IEmailService emailService)
    {
         _httpClientFactory = httpClientFactory;
         _emailService = emailService;
    }
    
    public async Task<IActionResult> Index()
    {
        var departmentId = User.FindFirstValue("DepartmentId");
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"http://localhost:5164/api/FaultReport/GetByDepartmanId/{departmentId}"); 
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<GetFaultReportByDepartmanIdDto>>(jsonData);
            return View(values);
        }
        return View();  
    }
    
    public async Task<IActionResult> ArizaDetay(string id)          
    {
        var userid = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
        ViewBag.UserId = userid;
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"http://localhost:5164/api/FaultReport/" + id);
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<GetFaultReportDto>(jsonData);
            ViewBag.DepartmanId = User.FindFirstValue("DepartmentId");
            return View(values); // artık ViewBag dolu
        }
        
        return NotFound(); 
    }
}