using Domain.Entities.Master;
using Microsoft.EntityFrameworkCore;

public class MasterDbContext : DbContext
{
    public MasterDbContext(DbContextOptions<MasterDbContext> options) : base(options) {}

    public DbSet<Tenant> Tenants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tenant>()
            .HasIndex(t => t.Email)
            .IsUnique();
        
    }
}