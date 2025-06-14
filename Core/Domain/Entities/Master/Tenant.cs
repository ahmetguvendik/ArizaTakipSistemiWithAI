namespace Domain.Entities.Master;

public class Tenant
{
    public string Id { get; set; }
    public string CompanyName { get; set; }
    public string Email { get; set; }
    public string ConnectionString { get; set; }
}