using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly FaultDbContext _context;

    public DepartmentRepository(FaultDbContext dbContext)
    {
         _context = dbContext;
    }
    
    public async Task<List<Department>> GetAllByUserIdAsync(string userId)
    {
        var department = await _context.Departments
            .Include(x => x.Users)
            .Where(x => x.Users.Any(y => y.Id == userId))
            .ToListAsync();
        return department;
    }
}