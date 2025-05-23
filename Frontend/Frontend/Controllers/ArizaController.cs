using System.Text;
using Application.Services;
using DTO.FaultReportDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Frontend.Controllers;

public class ArizaController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IEmailService  _emailService;
    

    public ArizaController(IHttpClientFactory httpClientFactory, IEmailService emailService)
    {
         _httpClientFactory = httpClientFactory;
         _emailService = emailService;
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
        StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("http://localhost:5164/api/FaultReport", stringContent);

        if (response.IsSuccessStatusCode)
        {
            TempData["SuccessMessage"] = "Ariza Kaydiniz Basarili Bir Sekilde Olusturuldu";
            await _emailService.SendFaultEmailAsync(createJobDto.ReporterEmail,"Arizaniz Basrili Bir Sekilde Olusturuldu ve Supervizore Iletildi");
            return RedirectToAction("Index", "Ariza");
        }

        var responseContent = await response.Content.ReadAsStringAsync();

        var allErrors = new List<string>();
        try
        {
            var errors = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(responseContent);
            if (errors != null)
            {
                foreach (var err in errors)
                {
                    allErrors.AddRange(err.Value);
                }
            }
            else
            {
                allErrors.Add("Bilinmeyen bir hata oluştu.");
            }
        }
        catch
        {
            allErrors.Add("Sunucudan geçersiz cevap alındı.");
            allErrors.Add(responseContent); // hata mesajını gösterelim
        }

        TempData["ErrorMessages"] = JsonConvert.SerializeObject(allErrors);
        return View();
    }

}