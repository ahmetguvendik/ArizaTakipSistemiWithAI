namespace Application.Features.Results.FaultReportResults;

public class GetFaultReportResult
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ReporterName { get; set; }
    public string ReporterPhone { get; set; }
    public string ReporterEmail { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } // Yeni, Atandı, Çözülüyor, Tamamlandı
    public string? MachineName { get; set; }
    public string? AssignedToName { get; set; } // Teknisyen
    public string? AssignedByName { get; set; } // Supervizör   

}