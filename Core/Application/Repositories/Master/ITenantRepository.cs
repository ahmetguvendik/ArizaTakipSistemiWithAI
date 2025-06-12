using Domain.Entities.Master;

namespace Application.Repositories.Master;

public interface ITenantRepository
{
    Task<Tenant> GetTenantByCompanyAndEmailAsync(string email, string password);
}