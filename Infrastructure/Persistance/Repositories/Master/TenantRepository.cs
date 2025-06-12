using Application.Repositories.Master;
using Domain.Entities.Master;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Repositories.Master;

public class TenantRepository : ITenantRepository
{
    private readonly MasterDbContext _context;

    public TenantRepository(MasterDbContext context)
    {
         _context = context;
    }
    
    public async Task<Tenant> GetTenantByCompanyAndEmailAsync(string email, string password)
    {
        return await _context.Tenants
            .FirstOrDefaultAsync(x => x.Email == email && x.PasswordHash == password);
    }
}