using Application.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Repositories;

public class FaultReportRepository : IFaultReportRepository
{
    private readonly FaultDbContext _context;

    public FaultReportRepository(FaultDbContext context )
    {
         _context = context;
    }
    
    public async Task<List<FaultReport>> GetAllAsync()
    {
        var values = await _context.FaultReports.Include(x=>x.AssignedBy).Include(x=>x.ClosedBy).Include(x=>x.AssignedTo).ThenInclude(y=>y.Department).Include(x=>x.Machine).OrderByDescending(x=>x.CreatedAt).ToListAsync();
        return values;
    }

    public async Task<FaultReport> GetFaultByIdAsync(string id)
    {
        var values = await _context.FaultReports.Include(x=>x.AssignedBy).Include(x=>x.AssignedTo).Include(x=>x.ClosedBy).ThenInclude(y=>y.Department).Include(x=>x.Machine).Where(x=>x.Id == id).FirstOrDefaultAsync();
        return values;
    }

    public async Task<List<FaultReport>> GetFaultByDepartmanIdAsync(string departmanId)
    {
        var values = await _context.FaultReports.Include(x=>x.AssignedBy).Include(x=>x.AssignedTo).Include(x=>x.ClosedBy).ThenInclude(y=>y.Department).Include(x=>x.Machine).Where(x=> x.AssignedTo.DepartmentId == departmanId).ToListAsync();
        return values;
    }
}