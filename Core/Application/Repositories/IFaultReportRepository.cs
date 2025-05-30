using Application.Features.Results.FaultReportResults;
using Domain.Entities;

namespace Application.Repositories;

public interface IFaultReportRepository
{
    Task<List<FaultReport>> GetAllAsync();
    Task<FaultReport> GetFaultByIdAsync(string id);
    Task<List<FaultReport>> GetFaultByDepartmanIdAsync(string departmanId);
    Task<List<int>> GetFaultByMonthAsync();
    Task<List<GetFaultByDepartmanQueryResult>> GetFaultByDepartmanAsync(); 
    
}