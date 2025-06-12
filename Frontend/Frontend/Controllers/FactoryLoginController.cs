using System.Security.Claims;
using Application.Features.Results.TenantResults;
using DTO.TenantDTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Frontend.Controllers;

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
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoginTenantDto loginTenantDto)
    {
        var client = _httpClientFactory.CreateClient();

        try
        {
            var response = await client.PostAsJsonAsync("http://localhost:5164/api/Tenant/login", loginTenantDto);

            if (response.IsSuccessStatusCode)
            {
                var loginResult = await response.Content.ReadFromJsonAsync<LoginTenantUserQueryResult>();

                if (loginResult != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, loginTenantDto.Email),
                        new Claim("TenantId", loginResult.Id),
                        new Claim("CompanyName", loginResult.CompanyName),
                        new Claim("ConnectionString", loginResult.ConnectionString ?? "")
                    };

                    var identity = new ClaimsIdentity(claims, "TenantIdentity");
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("MyCookieAuth", principal);

                    Log.Information($"{loginTenantDto.Email} giriş yaptı (Tenant)");
                    return RedirectToAction("Index", "Login"); // Kullanıcı login ekranına yönlendir
                }

                ViewBag.Error = "Yanıt çözümlenemedi.";
                Log.Error("Login sonucu boş geldi");
            }
            else
            {
                var errorText = await response.Content.ReadAsStringAsync();
                ViewBag.Error = "Sunucudan hata döndü: " + errorText;
                Log.Error("Giriş sırasında hata: " + errorText);
            }
        }
        catch (Exception ex)
        {
            ViewBag.Error = "İstek sırasında beklenmeyen bir hata oluştu.";
            Log.Error("Exception oluştu: " + ex.Message);
        }

        return View();
    }
}
