using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace DTO.FaultReportDtos;

public class CreateFaultReportDto
{
    [Required(ErrorMessage = "Başlık zorunludur")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Açıklama zorunludur")]
    public string Description { get; set; }
    [Required(ErrorMessage = "Ad ve Soyad zorunludur")]
    public string ReporterName { get; set; }
    
    [Required(ErrorMessage = "Telefon numarası zorunludur")]
    [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
    public string ReporterPhone { get; set; }
    
    [Required(ErrorMessage = "E-posta adresi zorunludur")] 
    [EmailAddress(ErrorMessage = "Geçerli bir e-posta giriniz")]
    public string ReporterEmail { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Yeni"; // Yeni, Atandı, Çözülüyor, Tamamlandı
    public string? MachineId { get; set; }
    public string? AssignedToId { get; set; } // Teknisyen
    public string? AssignedById { get; set; } // Supervizör
    public IFormFile? FaultFire { get; set; }
    public string? FaultFirePath { get; set; }      
}