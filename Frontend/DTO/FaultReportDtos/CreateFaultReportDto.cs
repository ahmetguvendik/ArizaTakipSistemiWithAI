namespace DTO.FaultReportDtos;

public class CreateFaultReportDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ReporterName { get; set; }
    public string ReporterPhone { get; set; }
    public string ReporterEmail { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Yeni"; // Yeni, Atandı, Çözülüyor, Tamamlandı
    public string? MachineId { get; set; }
    public string? AssignedToId { get; set; } // Teknisyen
    public string? AssignedById { get; set; } // Supervizör
}