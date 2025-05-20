namespace Domain.Entities;

public class Machine  : BaseEntity
{
    public string Name { get; set; }
    public string SerialNumber { get; set; }
    public int DepartmentId { get; set; }
    public Department Department { get; set; }
    public ICollection<FaultReport> FaultReports { get; set; }  
}