using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class AppUser : IdentityUser<string>
{
    public string NameSurname { get; set; }
    public int? DepartmentId { get; set; }
    public Department Department { get; set; }
    public ICollection<FaultReport> AssignedFaultReports { get; set; }
    public ICollection<FaultReport> AssignedByReports { get; set; } 
}