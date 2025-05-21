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
        var values = await _context.FaultReports.Include(x=>x.AssignedBy).Include(x=>x.AssignedTo).Include(x=>x.Machine).ToListAsync();
        return values;
    }
}