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
                var response = await client.PostAsJsonAsync("http://testapi.solfix.help:5164/api/Login", loginUserDto);

                if (response.IsSuccessStatusCode)
                {
                    var loginResult = await response.Content.ReadFromJsonAsync<LoginUserQueryResult>();

                    if (loginResult != null)
                    {
                        // 🔐 Claims oluşturuluyor
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, loginUserDto.Username),
                            new Claim(ClaimTypes.NameIdentifier, loginResult.Id), // Burada UserId'yi ekliyoruz
                            new Claim(ClaimTypes.Role, loginResult.Role),
                            new Claim("DepartmentId", loginResult.DepartmanId ?? "") // DepartmanId claim olarak

                        };

                        var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                        var principal = new ClaimsPrincipal(identity);

                        // 🔐 Kullanıcı oturum açıyor
                        await HttpContext.SignInAsync("MyCookieAuth", principal);
                        Log.Information(loginUserDto.Username + "Giris Yapti");
                        // Rol bazlı yönlendirme
                        return loginResult.Role switch
                        {
                            "Admin" => RedirectToAction("Index", "AdminJob"),   
                            "Teknisyen" => RedirectToAction("Index", "Teknisyen"),
                            "Supervisor" => RedirectToAction("Index", "Supervisor"),   
                            _ => RedirectToAction("Index", "Login"),
                            
                        };
                       
                    }
                    else
                    {
                        Log.Error("Giris Yapilirken Hata Olustu");
                        ViewBag.Error = "Kullanıcı adı ya da şifre hatalı.";
                    }
                }
                else
                {
                    var errorText = await response.Content.ReadAsStringAsync();
                    Log.Error("Giris Yapilirken Hata Olustu" + errorText);
                    ViewBag.Error = "Sunucudan hata döndü: " + errorText;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Giris Yapilirken Hata Olustu" + ex.Message);
                ViewBag.Error = "İstek sırasında beklenmeyen bir hata oluştu: " + ex.Message;
            }

            return View();
        }

        // Opsiyonel: çıkış işlemi
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Index", "Login");    
        }
        
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(resetPasswordDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://testapi.solfix.help:5164/api/Login/reset-password", stringContent);
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
            var response = await client.PostAsync("http://testapi.solfix.help:5164/api/Login/forgot-password", stringContent);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Sifre Sifirlama Basarili Bir Sekilde Olusturuldu";
                Log.Information("Sifre Sifirlama Yapildi" + resetPasswordEmailDto.Email);
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }