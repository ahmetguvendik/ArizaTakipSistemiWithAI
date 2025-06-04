using System.Net.Http.Headers;
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
    using var content = new MultipartFormDataContent();

    // String alanları ekle
    if (!string.IsNullOrEmpty(createJobDto.ReporterName))
        content.Add(new StringContent(createJobDto.ReporterName), "ReporterName");

    if (!string.IsNullOrEmpty(createJobDto.ReporterEmail))
        content.Add(new StringContent(createJobDto.ReporterEmail), "ReporterEmail");

    content.Add(new StringContent(createJobDto.ReporterPhone ?? ""), "ReporterPhone");
    content.Add(new StringContent(createJobDto.Title ?? ""), "Title");
    content.Add(new StringContent(createJobDto.Description ?? ""), "Description");

    // 1. JSON veriyi ekle (ad: "jsonData")
    var jsonData = JsonConvert.SerializeObject(createJobDto);
    var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
    content.Add(stringContent, "jsonData"); 

    // 2. Dosya varsa ekle
    if (createJobDto.FaultFire != null && createJobDto.FaultFire.Length > 0)
    {
        var fileStream = createJobDto.FaultFire.OpenReadStream();
        var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(createJobDto.FaultFire.ContentType);
        content.Add(fileContent, "FaultFire", createJobDto.FaultFire.FileName);
    }

    // 3. POST isteği gönder
    var response = await client.PostAsync("http://localhost:5164/api/FaultReport", content);

    if (response.IsSuccessStatusCode)
    {
        TempData["SuccessMessage"] = "Ariza Kaydiniz Basarili Bir Sekilde Olusturuldu";
        await _emailService.SendFaultEmailAsync(createJobDto.ReporterEmail, "Arizaniz Basrili Bir Sekilde Olusturuldu ve Supervizore Iletildi");
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
        allErrors.Add(responseContent);
    }

    TempData["ErrorMessages"] = JsonConvert.SerializeObject(allErrors);
    return View();
}



}