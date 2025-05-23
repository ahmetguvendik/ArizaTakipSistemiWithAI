namespace Application.Features.Results.FaultReportResults;

public class GetFaultReportByDepartmanIdQueryResult
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ReporterName { get; set; }
    public string ReporterPhone { get; set; }
    public string ReporterEmail { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime AssignedTime { get; set; }      
    public DateTime ClosedTime { get; set; }      
    public string Status { get; set; } = "Yeni"; // Yeni, Atandı, Çözülüyor, Tamamlandı
    public string? MachineId { get; set; }
    public string? MachineName { get; set; }
    public string? AssignedToId { get; set; } // Teknisyen
    public string? AssignedToName { get; set; } // Teknisyen
    public string? AssignedById { get; set; } // Supervizör
    public string? AssignedByName { get; set; } // Supervizör
    public string? ClosedById { get; set; } // Kapatan      
    public string? ClosedByName { get; set; } // Kapatan        
    public string ClosedDescription { get; set; }
    public string DepartmanId { get; set; }
    public string DepartmanName { get; set; }   
}