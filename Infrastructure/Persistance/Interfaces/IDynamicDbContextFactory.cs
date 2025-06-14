namespace Persistance.Interfaces;

public interface IDynamicDbContextFactory
{
    FaultDbContext CreateDbContext(string connectionString);
}