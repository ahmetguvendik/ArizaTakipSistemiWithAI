using Application.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly FaultDbContext _dbContext;

    public Repository(FaultDbContext dbContext)
    {
        _dbContext = dbContext;
    }  
    public async Task<List<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync(); 
    }

    public async Task<T> GetByIdAsync(string id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public async Task CreateAsync(T entity)
    {
        _dbContext.Add(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbContext.Set<T>().Update(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}