using DTO.AppUserDto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Frontend.ViewComponents.SupervisorViewComponents;

public class _SupervisorTopbarComponentPartial : ViewComponent
{
    private readonly IHttpClientFactory _httpClientFactory;

    public _SupervisorTopbarComponentPartial(IHttpClientFactory httpClientFactory)  
    {
         _httpClientFactory = httpClientFactory;
    }
    public async Task<IViewComponentResult> InvokeAsync(string id)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"http://localhost:5164/api/User/GetUserById/{id}");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<GetUserByIdDto>(json);
            return View(value);
        }
        return View();
    }
}