using System.Security.Claims;
using System.Text;
using Application.Services;
using DTO.AppUserDto;
using DTO.FaultReportDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Frontend.Controllers;


[Authorize(Roles = "Supervisor")]
public class SupervisorController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IEmailService  _emailService;
    

    public SupervisorController(IHttpClientFactory httpClientFactory, IEmailService emailService)
    {
         _httpClientFactory = httpClientFactory;
         _emailService = emailService;
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
        var client = _httpClientFactory.CreateClient();

        // Teknisyenleri her durumda çek
        var userResponse = await client.GetAsync("http://localhost:5164/api/User");
        var jsonData2 = await userResponse.Content.ReadAsStringAsync();
        var values2 = JsonConvert.DeserializeObject<List<GetTeknisyenDto>>(jsonData2);

        ViewBag.Users = values2.Select(x => new SelectListItem
        {
            Text = x.Name,
            Value = x.Id.ToString()
        }).ToList();

        // Arıza bilgilerini çek
        var response = await client.GetAsync($"http://localhost:5164/api/FaultReport/" + id);
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<GetFaultReportDto>(jsonData);
            return View(values); // artık ViewBag dolu
        }

        return NotFound(); // veya boş bir View de döndürebilirsin ama NotFound daha iyi
    }

    [HttpPost]
    public async Task<IActionResult> TeknisyenAta(string faultReportId, string assignedToId)
    {
        var supervisorId = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
        var client = _httpClientFactory.CreateClient();
        var atamaDto = new TeknisyenAtamaDto()
        {
            id = faultReportId,
            AssignnedToId = assignedToId,
            AssignnedById = supervisorId,   
            Statues = "Atandı",
            AssignedTime = DateTime.Now,
        };
        
            var response2 = await client.GetAsync($"http://localhost:5164/api/FaultReport/" + faultReportId);
            var jsonData = await response2.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<GetFaultReportDto>(jsonData);
            

            var json = JsonConvert.SerializeObject(atamaDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("http://localhost:5164/api/FaultReport", content);

            if (response.IsSuccessStatusCode)
            {
                await _emailService.SendSupervisorToTeknisyenEmailAsync(values.ReporterEmail,
                    "Arizasiniz Supervizor Tarafindan Ilgili Teknisyene Atanmistir");
                return RedirectToAction("Index"); 
            }
           

        return BadRequest("Atama başarısız.");
    }


}