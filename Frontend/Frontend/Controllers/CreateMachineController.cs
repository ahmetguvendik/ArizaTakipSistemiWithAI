using System.Security.Claims;
using DTO.DepartmentDTOs;
using DTO.MachuneDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Serilog;

namespace Frontend.Controllers;

public class CreateMachineController : Controller
{
   private readonly IHttpClientFactory _clientFactory;

   public CreateMachineController(IHttpClientFactory clientFactory)
   {
        _clientFactory = clientFactory;
   }
   
    public async Task<IActionResult> Index()
    {
        var userId = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
        var client = _clientFactory.CreateClient();
        var response = await client.GetAsync($"http://localhost:5164/api/Department?id={userId}");
        var values = JsonConvert.DeserializeObject<List<GetDepartmentByUserIdDto>>(await response.Content.ReadAsStringAsync());
        ViewBag.Department = values.Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }).ToList();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(CreateMachineDto createMachine)
    {
        var client = _clientFactory.CreateClient();
        var response = await client.PostAsJsonAsync("http://localhost:5164/api/Machine", createMachine);
        if (response.IsSuccessStatusCode)
        {
            TempData["SuccessMessage"] = "Makine Kaydiniz Basarili Bir Sekilde Olusturuldu";
            return RedirectToAction("Index","CreateMachine");
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