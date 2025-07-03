using System.Security.Claims;
using Application.Services;
using DTO.FaultReportDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace Frontend.Controllers;

[Authorize(Roles = "Teknisyen")]
public class TeknisyenController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;


    public TeknisyenController(IHttpClientFactory httpClientFactory)
    {
         _httpClientFactory = httpClientFactory;
       
    }
    
    public async Task<IActionResult> Index()
    {
        var departmentId = User.FindFirstValue("DepartmentId");
        ViewBag.DepartmentId = departmentId;
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"https://testapi.solfix.help:444/api/FaultReport/GetByDepartmanId/{departmentId}"); 
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<GetFaultReportByDepartmanIdDto>>(jsonData);
            return View(values);
        }
       
        Log.Error("Teknsiyen Veri Cekerken Hata Olustu");
        return View();  
    }
    
    public async Task<IActionResult> ArizaDetay(string id)          
    {
        var userid = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
        ViewBag.UserId = userid;
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"https://testapi.solfix.help:444/api/FaultReport/" + id);
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<GetFaultReportDto>(jsonData);
            ViewBag.DepartmanId = User.FindFirstValue("DepartmentId");
            return View(values); // artıhttp://testapi.solfix.help
        }
        
        Log.Error("Teknsiyen Ariza Detaya Bakarken Hata Olustu");
        return NotFound(); 
    }
}