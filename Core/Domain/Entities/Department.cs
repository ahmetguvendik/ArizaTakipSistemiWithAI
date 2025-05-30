namespace Domain.Entities;

public class Department : BaseEntity    
{
    public string Name { get; set; }
    public ICollection<AppUser> Users { get; set; }
    public ICollection<Machine> Machines { get; set; }
}