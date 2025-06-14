using DTO.TenantDTOs;
using Microsoft.AspNetCore.Mvc;
using Serilog;

public class FactoryLoginController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public FactoryLoginController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IActionResult Index()
    {
        if (!string.IsNullOrEmpty(HttpContext.Session.GetString("DynamicConnectionStringFrontend")))
        {
            Log.Information("Frontend Session'da ConnectionString bulundu, ana sayfaya yönlendiriliyor.");
            return RedirectToAction("Index", "Ariza");
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoginTenantDto loginTenantDto)
    {
        var client = _httpClientFactory.CreateClient();

        try
        {
            // 1. Adım: Master API'ye ConnectionString'i doğrulama isteği gönder
            var masterApiResponse =
                await client.PostAsJsonAsync("http://localhost:5164/api/Tenant/login", loginTenantDto);

            if (!masterApiResponse.IsSuccessStatusCode)
            {
                var errorText = await masterApiResponse.Content.ReadAsStringAsync();
                ViewBag.Error = "Şirket bağlantısı doğrulanamadı: " + errorText;
                Log.Error($"Master API'den hata döndü: {errorText}");
                return View(loginTenantDto);
            }

            var masterApiContent = await masterApiResponse.Content.ReadFromJsonAsync<LoginTenantDto>();

            if (masterApiContent == null || string.IsNullOrEmpty(masterApiContent.ConnectionString))
            {
                ViewBag.Error = "Şirket bağlantısı başarılı ancak bağlantı bilgisi alınamadı.";
                Log.Warning("Master API'den boş veya null ConnectionString döndü.");
                return View(loginTenantDto);
            }

            // ConnectionString'i Frontend'in kendi Session'ına kaydedin.
            HttpContext.Session.SetString("DynamicConnectionStringFrontend", masterApiContent.ConnectionString);

            Log.Information("Bağlantı anahtarı Master API'den alındı ve Frontend Session'a kaydedildi.");

            // 2. Adım: Backend'e, bu ConnectionString'i kendi Session'ına kaydetmesi için ayrı bir istek gönder
            var backendSetConnectionStringApiUrl = "http://localhost:5164/api/Login/SetTenantConnectionString";
            // Burası da harika, LoginTenantDto'yu direkt olarak JSON payload olarak gönderiyorsunuz.
            var setConnectionStringResponse =
                await client.PostAsJsonAsync(backendSetConnectionStringApiUrl,
                    masterApiContent); // <-- Burada masterApiContent'i direkt gönderin

            if (!setConnectionStringResponse.IsSuccessStatusCode)
            {
                var errorText = await setConnectionStringResponse.Content.ReadAsStringAsync();
                ViewBag.Error = "Bağlantı bilgisi sunucuya aktarılamadı: " + errorText;
                Log.Error($"Backend ConnectionString Set API'den hata döndü: {errorText}");
                HttpContext.Session.Remove("DynamicConnectionStringFrontend");
                return View(loginTenantDto);
            }

            Log.Information("Backend Session'a ConnectionString başarıyla aktarıldı.");

            // 3. Adım: Her şey yolundaysa, kullanıcıyı doğrudan uygulamanın ana sayfasına yönlendirin.
            return RedirectToAction("Index", "Login");
        }
        catch (Exception ex)
        {
            ViewBag.Error = "İstek sırasında beklenmeyen bir hata oluştu.";
            Log.Error($"Exception oluştu: {ex.Message}", ex);
            return View(loginTenantDto);
        }
    }
}