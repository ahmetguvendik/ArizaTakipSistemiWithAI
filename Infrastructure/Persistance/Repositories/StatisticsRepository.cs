using Application.Repositories;

namespace Persistance.Repositories;

public class StatisticsRepository : IStatisticsRepository
{
    private readonly FaultDbContext _context;

    public StatisticsRepository(FaultDbContext context)
    {
         _context = context;
    }
    
    public int GetTotalFaultCount()
    {
       var value = _context.FaultReports.Count();
       return value;
    }

    public int GetNewFaultCount()
    {
        var value = _context.FaultReports.Count(x => x.Status == "Yeni");
        return value;
    }

    public int GetAssignedFaultCount()
    {
        var value = _context.FaultReports.Count(x => x.Status == "Atandı"); 
        return value;
    }

    public int GetClosedFaultCount()
    {
        var value = _context.FaultReports.Count(x => x.Status == "Kapandı");
        return value;
    }

    public double GetAverageAssignmentTimeInMinutes()
    {
        var assignedFaults = _context.FaultReports
            .Where(f => f.AssignedToId != null && f.AssignedTime > f.CreatedAt)
            .ToList();

        if (!assignedFaults.Any())
            return 0;

        var averageMinutes = assignedFaults
            .Select(f => (f.AssignedTime - f.CreatedAt).TotalMinutes)
            .Average();

        return Math.Round(averageMinutes, 2);
    }

    public double GetAverageClosedTimeInMinutes()
    {
            var closedFaults = _context.FaultReports
                .Where(f => f.ClosedTime > f.CreatedAt)
                .ToList();

            if (!closedFaults.Any())
                return 0;

            var averageMinutes = closedFaults
                .Select(f => (f.ClosedTime - f.CreatedAt).TotalMinutes)
                .Average();

            return Math.Round(averageMinutes, 2);
            
    }
}