using Domain.Entities;

namespace Application.Repositories;

public interface IDepartmentRepository
{
    Task<List<Department>> GetAllByUserIdAsync(string userId);
    
}