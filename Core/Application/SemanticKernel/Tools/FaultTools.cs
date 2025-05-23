using System.ComponentModel;
using Application.Features.Commands.FaultReportComamnds;
using DTO.FaultReportDtos;
using Microsoft.SemanticKernel;
using Newtonsoft.Json;

namespace Application.SemanticKernel.Tools;

public class FaultTools
{
    private readonly IHttpClientFactory _clientFactory;

    public FaultTools(IHttpClientFactory clientFactory)
    {
         _clientFactory = clientFactory;
    }

    [KernelFunction, Description("Sistemdeki en yeni hata raporunu API'den çeker ve JSON formatında döndürür.")]
    public async Task<string> GetNewestFault()
    {
        var client = _clientFactory.CreateClient();
        try
        {
            var response = await client.GetAsync("http://localhost:5164/api/FaultReport");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<GetFaultReportDto>>(jsonData);

                if (values != null && values.Any())
                {
                    // "GetNewestFault" adına uygun olarak, rapor tarihine göre en yenisini bul.
                    var newestFault = values.OrderByDescending(f => f.CreatedAt).FirstOrDefault();  

                    if (newestFault != null)
                    {
                        // En yeni hata raporunu JSON string olarak döndür.
                        return JsonConvert.SerializeObject(newestFault, Formatting.Indented);
                    }
                    else
                    {
                        return "API'den herhangi bir hata raporu bulunamadı veya en yeni rapor tespit edilemedi.";
                    }
                }
                else
                {
                    return "API'den herhangi bir hata raporu alınamadı.";
                }
            }
            else
            {
                // API çağrısı başarısız olursa, durumu ve içeriği döndür.
                return $"API çağrısı başarısız oldu. Durum Kodu: {response.StatusCode}, Mesaj: {await response.Content.ReadAsStringAsync()}";
            }
        }
        catch (Exception ex)
        {
            // İstisna durumunda hata mesajını döndür.
            return $"Hata raporu alınırken bir istisna oluştu: {ex.Message}";
        }
    }
}
    
    
