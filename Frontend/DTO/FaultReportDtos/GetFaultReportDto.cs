using System.Security.AccessControl;

namespace DTO.FaultReportDtos;

public class GetFaultReportDto
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
    public string? MachineId { get; set; }
    public string? AssignedToName { get; set; } // Teknisyen
    public string? AssignedByName { get; set; } // Supervizör   
    public string DepartmanName { get; set; }
    public string AssignedToId { get; set; }
    public string DepartmanId { get; set; }
    
    
}