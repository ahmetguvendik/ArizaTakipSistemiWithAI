using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Repositories;

public class MachineRepository  : IMachineRepository
{
    private readonly FaultDbContext _context;

    public MachineRepository(FaultDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Machine>> GetMachineByDepartmanIdAsync(string departmanId)
    {
        var values = await _context.Machines.Include(x =>x.Department).Where(y=>y.DepartmentId == departmanId).AsNoTracking().ToListAsync();
        return values;
    }
}