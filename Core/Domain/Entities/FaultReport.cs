namespace Domain.Entities;

public class FaultReport : BaseEntity
{
        
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
        public Machine Machine { get; set; }

        public string? AssignedToId { get; set; } // Teknisyen
        public AppUser AssignedTo { get; set; }

        public string? AssignedById { get; set; } // Supervizör
        public AppUser AssignedBy { get; set; }
        
        public string? ClosedById { get; set; } // Kapatan      
        public AppUser ClosedBy { get; set; }
        public string? ClosedDescription { get; set; }                          
}