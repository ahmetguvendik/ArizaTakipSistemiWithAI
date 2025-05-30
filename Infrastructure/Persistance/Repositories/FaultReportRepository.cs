using Application.Features.Results.FaultReportResults;
using Application.Repositories;
using Domain.Entities;
using DTO.FaultReportDtos;
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

    public async Task<List<int>> GetFaultByMonthAsync()
    {
        var currentYear = DateTime.UtcNow.Year; 

        // CreatedAt yılı currentYear olan kayıtları ay bazında grupla
        var query = await _context.FaultReports
            .Where(fr => fr.CreatedAt.Year == currentYear)
            .GroupBy(fr => fr.CreatedAt.Month)
            .Select(g => new
            {
                Month = g.Key,
                Count = g.Count()
            })
            .ToListAsync();

   
        int[] counts = new int[12];
        foreach (var item in query)
        {
            // item.Month 1-12 arasında, dizi indeksi 0-11
            counts[item.Month - 1] = item.Count;
        }

        return counts.ToList();
    }

    public async Task<List<GetFaultByDepartmanQueryResult>> GetFaultByDepartmanAsync()
    {
        var result = await _context.FaultReports
            .Where(f =>  f.Machine.Department != null)
            .GroupBy(f => f.Machine.Department.Name)
            .Select(g => new GetFaultByDepartmanQueryResult   
            {
                DepartmanName = g.Key,
                Count = g.Count()
            })
            .ToListAsync();
        
        return result;  
    }

    public async Task<List<FaultReport>> GetFaultByDepartmanIdAsync(string departmanId)
    {
        var values = await _context.FaultReports.Include(x=>x.AssignedBy).Include(x=>x.AssignedTo).Include(x=>x.ClosedBy).ThenInclude(y=>y.Department).Include(x=>x.Machine).Where(x=> x.AssignedTo.DepartmentId == departmanId).ToListAsync();
        return values;
    }
}