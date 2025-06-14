using Application.Repositories;
using Domain.Entities.Master;

namespace Persistance.Repositories;

public class TenantRepository : ITenantRepository
{
    private readonly MasterDbContext _context;

    public TenantRepository(MasterDbContext context)
    {
         _context = context;
    }
    
    Tenant ITenantRepository.GetConnectionString(string connectionString)
    {
        var values = _context.Tenants.FirstOrDefault(x => x.ConnectionString == connectionString);
        return values;
    }
}