using System.Net.Http.Headers;
using System.Text;
using Application.Services;
using DTO.FaultReportDtos;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using Newtonsoft.Json;
using Serilog;

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
        Log.Information(createJobDto.ReporterName + " " + " Ariza olusturdu");
        TempData["SuccessMessage"] = "Ariza Kaydiniz Basarili Bir Sekilde Olusturuldu";
        string body = $@"
<div style='font-family:Arial,sans-serif; font-size:15px; color:#333;'>
    <p>Sayın {createJobDto.ReporterName},</p>

    <p>
        Tarafımıza iletmiş olduğunuz <strong>{createJobDto.Title} başlıklı arıza bildirimi</strong> başarılı bir şekilde alınmış ve ilgili <strong>süpervizöre</strong> yönlendirilmiştir.
    </p>

    <p>
        En kısa sürede sizinle iletişime geçilecek ve gerekli müdahale sağlanacaktır.
    </p>

    <p>
        Destek talebinizin durumu hakkında gelişmeleri tarafınıza bildirmeye devam edeceğiz.
    </p>

    <br/>
    <p>İyi günler dileriz,</p>
    <p style='color:#4a90e2; font-weight:bold;'>Solfix Destek Ekibi</p>
</div>";

        await _emailService.SendFaultEmailAsync(createJobDto.ReporterEmail, body);
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
             
                Log.Error(createJobDto.ReporterName + " " + err.Value);
                allErrors.AddRange(err.Value);
            }
        }
        else
        {
            Log.Error(createJobDto.ReporterName +" " + errors.First().Value);
            allErrors.Add("Bilinmeyen bir hata oluştu.");
        }
    }
    catch
    {
        allErrors.Add("Sunucudan geçersiz cevap alındı.");
        Log.Error(createJobDto.ReporterName +" " + allErrors.First());
        allErrors.Add(responseContent);
        
    }

    TempData["ErrorMessages"] = JsonConvert.SerializeObject(allErrors);
    return View();
}



}