using Domain.Entities;

namespace Application.Repositories;

public interface IFaultReportRepository
{
    Task<List<FaultReport>> GetAllAsync();
}