using Domain.Entities;

namespace Application.Repositories;

public interface IMachineRepository
{
    Task<List<Machine>> GetMachineByDepartmanIdAsync(string departmanId);   
}