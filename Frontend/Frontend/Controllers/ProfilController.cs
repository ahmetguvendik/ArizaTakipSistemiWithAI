using System.Security.Claims;
using DTO.AppUserDto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Frontend.Controllers;

public class ProfilController : Controller
{
    private readonly IHttpClientFactory _clientFactory;

    public ProfilController(IHttpClientFactory clientFactory)
    {
         _clientFactory = clientFactory;
    }
    
    public async Task<IActionResult> Index()
    {
        var userid = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null;
        var client = _clientFactory.CreateClient();
        var response = await client.GetAsync($"https://testapi.solfix.help:444/api/User/GetUserById/{userid}");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<GetUserByIdDto>(json);
            return View(value);
        }
        return View();
    }
}