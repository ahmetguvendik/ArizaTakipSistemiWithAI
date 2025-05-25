using DTO.StatisticDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Frontend.Controllers;

[Authorize(Roles = "Supervisor")]
public class IstatistikController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public IstatistikController(IHttpClientFactory httpClientFactory)
    {
         _httpClientFactory = httpClientFactory;
    }
    
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient();
        
        var response = await client.GetAsync("http://localhost:5164/api/Statistics/GetTotalFaultCount");
        if (response.IsSuccessStatusCode)
        {
            var jsonData = await response.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<ResultStatisticsDto>(jsonData);
            ViewBag.v1 = values.GetTotalFaultCount;
        }
        
        var response1 = await client.GetAsync("http://localhost:5164/api/Statistics/GetAssignedlFaultCount");
        if (response1.IsSuccessStatusCode)
        {
            var jsonData1 = await response1.Content.ReadAsStringAsync();
            var values1 = JsonConvert.DeserializeObject<ResultStatisticsDto>(jsonData1);
            ViewBag.v2 = values1.GetAssignedFaultCount; 
        }
        
        var response2 = await client.GetAsync("http://localhost:5164/api/Statistics/GetClosedFaultCount");   
        if (response2.IsSuccessStatusCode)
        {
            var jsonData2 = await response2.Content.ReadAsStringAsync();
            var values2 = JsonConvert.DeserializeObject<ResultStatisticsDto>(jsonData2);
            ViewBag.v3 = values2.GetClosedFaultCount; 
        }
        
        var response3 = await client.GetAsync("http://localhost:5164/api/Statistics/GetNewFaultCount");   
        if (response3.IsSuccessStatusCode)
        {
            var jsonData3 = await response3.Content.ReadAsStringAsync();
            var values3 = JsonConvert.DeserializeObject<ResultStatisticsDto>(jsonData3);
            ViewBag.v4 = values3.GetNewFaultCount; 
        }
        
        var response4 = await client.GetAsync("http://localhost:5164/api/Statistics/GetAverageAssignmentTimeInMinutes");   
        if (response4.IsSuccessStatusCode)
        {
            var jsonData4 = await response4.Content.ReadAsStringAsync();
            var values4 = JsonConvert.DeserializeObject<ResultStatisticsDto>(jsonData4);
            ViewBag.v5 = values4.GetTimeFaultAssignedToTeknosyenCountTime; 
        }

        
        var response5 = await client.GetAsync("http://localhost:5164/api/Statistics/GetAverageClosedTimeInMinutes");   
        if (response5.IsSuccessStatusCode)
        {
            var jsonData5 = await response5.Content.ReadAsStringAsync();
            var values5 = JsonConvert.DeserializeObject<ResultStatisticsDto>(jsonData5);
            ViewBag.v6 = values5.GetAverageClosedTimeInMinutes; 
        }

        return View();
    }
}