using System.Security.Claims;
using DTO.AppRoleDTOs;
using DTO.AppUserDto;
using DTO.DepartmentDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Serilog;

namespace Frontend.Controllers;

[Authorize(Roles = "Supervisor")]
public class CreateUserController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public CreateUserController(IHttpClientFactory httpClientFactory)
    {
         _httpClientFactory = httpClientFactory;
    }
    
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync("https://testapi.solfix.help:444/api/Department/GetAll");
        if (response.IsSuccessStatusCode)
        {
            var values = JsonConvert.DeserializeObject<List<GetAllDepartmentDto>>(await response.Content.ReadAsStringAsync());
            ViewBag.Department = values.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();
        }
        
        var client2 = _httpClientFactory.CreateClient();
        var response2 = await client2.GetAsync("https://testapi.solfix.help:444/api/Role");
        if (response2.IsSuccessStatusCode)
        {
            var values2 = JsonConvert.DeserializeObject<List<GetAllRoleDto>>(await response2.Content.ReadAsStringAsync());
            ViewBag.Role = values2.Select(x => new SelectListItem { Text = x.Name, Value = x.Name.ToString() }).ToList();
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(CreateUserDto createUserDto)
    {
        var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync("https://testapi.solfix.help:444/api/Register", createUserDto);
        if (response.IsSuccessStatusCode)
        {
            TempData["SuccessMessage"] = "Kişi Kaydiniz Basarili Bir Sekilde Olusturuldu";
            Log.Information($"{createUserDto.Username} Sisteme eklendi <---> Ekleyen kişi : {userid}");
            return RedirectToAction("Index","CreateUser");
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
                    Log.Error($"{err.Key}: {err.Value}");
                }
            }
            else
            {
                allErrors.Add("Bilinmeyen bir hata oluştu.");
                Log.Error("Bilinmeyen bir hata oluştu");
            }
        }
        catch
        {
            allErrors.Add("Sunucudan geçersiz cevap alındı.");
            allErrors.Add(responseContent);
            Log.Error(responseContent);
        }

        TempData["ErrorMessages"] = JsonConvert.SerializeObject(allErrors);
        Log.Error(JsonConvert.SerializeObject(allErrors));
        return View();
    }
}