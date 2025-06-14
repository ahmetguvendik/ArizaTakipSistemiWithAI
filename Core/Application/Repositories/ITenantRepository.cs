using Domain.Entities.Master;

namespace Application.Repositories;

public interface ITenantRepository
{
    Tenant GetConnectionString(string connectionString);
}