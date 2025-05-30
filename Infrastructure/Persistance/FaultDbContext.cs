using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistance;

public class FaultDbContext : IdentityDbContext<AppUser, AppRole, string>
{
    public FaultDbContext(DbContextOptions<FaultDbContext> options) : base(options) { }


    public DbSet<FaultReport> FaultReports { get; set; }
    public DbSet<Machine> Machines { get; set; }
    public DbSet<Department> Departments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); 

        modelBuilder.Entity<FaultReport>()
            .HasOne(f => f.AssignedTo)
            .WithMany(u => u.AssignedFaultReports)
            .HasForeignKey(f => f.AssignedToId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FaultReport>()
            .HasOne(f => f.AssignedBy)
            .WithMany(u => u.AssignedByReports)
            .HasForeignKey(f => f.AssignedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<FaultReport>()
            .HasOne(f => f.ClosedBy)
            .WithMany(u => u.ClosedByReports)
            .HasForeignKey(f => f.ClosedById)   
            .OnDelete(DeleteBehavior.Restrict);
    }
    

}