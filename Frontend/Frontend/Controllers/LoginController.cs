using System.Security.Claims;
using System.Text;
using Application.Features.Results.AppUserResults;
using DTO.AppUserDto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace Frontend.Controllers;

public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginController(IHttpClientFactory clientFactory)
        {
            _httpClientFactory = clientFactory;
        }

        // GET: /Login
        public IActionResult Index()
        {
            return View();
        }

        // POST: /Login
  [HttpPost]
public async Task<IActionResult> Index(LoginUserDto loginUserDto)
{
    var client = _httpClientFactory.CreateClient();

    try
    {
        var response = await client.PostAsJsonAsync("http://localhost:5164/api/Login", loginUserDto);

        if (response.IsSuccessStatusCode)
        {
            var loginResult = await response.Content.ReadFromJsonAsync<LoginUserQueryResult>();

            if (loginResult != null)
            {
                // 🔄 Önceki claim'leri al (FactoryLogin'den gelen Tenant bilgileri)
                var existingClaims = User?.Claims?.ToList() ?? new List<Claim>();

                // 🆕 Yeni claim'ler (User bilgileri)
                var newClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginUserDto.Username),
                    new Claim(ClaimTypes.NameIdentifier, loginResult.Id),
                    new Claim(ClaimTypes.Role, loginResult.Role),
                    new Claim("DepartmentId", loginResult.DepartmanId ?? "")
                };

                // 📌 Aynı claim tiplerini sil, yeni olanları ekle
                var mergedClaims = existingClaims
                    .Where(oldClaim => newClaims.All(newClaim => newClaim.Type != oldClaim.Type))
                    .Concat(newClaims)
                    .ToList();

                var identity = new ClaimsIdentity(mergedClaims, "MyCookieAuth");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("MyCookieAuth", principal);

                Log.Information($"{loginUserDto.Username} giriş yaptı (Kullanıcı)");

                // Rol bazlı yönlendirme
                return loginResult.Role switch
                {
                    "Admin" => RedirectToAction("Index", "AdminJob"),
                    "Teknisyen" => RedirectToAction("Index", "Teknisyen"),
                    "Supervisor" => RedirectToAction("Index", "Supervisor"),
                    _ => RedirectToAction("Index", "Login"),
                };
            }

            ViewBag.Error = "Kullanıcı adı ya da şifre hatalı.";
            Log.Error("Login sonucu null");
        }
        else
        {
            var errorText = await response.Content.ReadAsStringAsync();
            ViewBag.Error = "Sunucudan hata döndü: " + errorText;
            Log.Error("Login API hatası: " + errorText);
        }
    }
    catch (Exception ex)
    {
        ViewBag.Error = "İstek sırasında beklenmeyen bir hata oluştu: " + ex.Message;
        Log.Error("Login exception: " + ex.Message);
    }

    return View();
}


        // Opsiyonel: çıkış işlemi
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Index", "Ariza");    
        }
        
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(resetPasswordDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://localhost:5164/api/Login/reset-password", stringContent);
            if (response.IsSuccessStatusCode)
            {
                Log.Information("Sifre Sifirlama Istegi Yapildi" + resetPasswordDto.Email);
                TempData["SuccessMessage"] = "Sifre Sifirlama Isteginiz Basarili Bir Sekilde Olusturuldu";
                return RedirectToAction("Index", "Login");
            }

            return View();
        }
        
        public async Task<IActionResult> ResetPasswordEmail(ResetPasswordEmailDto resetPasswordEmailDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(resetPasswordEmailDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://localhost:5164/api/Login/forgot-password", stringContent);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Sifre Sifirlama Basarili Bir Sekilde Olusturuldu";
                Log.Information("Sifre Sifirlama Yapildi" + resetPasswordEmailDto.Email);
                return RedirectToAction("Index", "Login");
            }

            return View();
        }
    }