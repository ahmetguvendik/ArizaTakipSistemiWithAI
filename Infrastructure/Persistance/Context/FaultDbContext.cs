using Domain.Entities;
using Microsoft.AspNetCore.Identity;
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

    // Identity Role ayarları
    modelBuilder.Entity<AppRole>(b =>
    {
        b.Property(r => r.Id).HasMaxLength(450);
        b.Property(r => r.Name).HasMaxLength(256);
        b.Property(r => r.NormalizedName).HasMaxLength(256);
    });

    // Identity User ayarları
    modelBuilder.Entity<AppUser>(b =>
    {
        b.Property(u => u.Id).HasMaxLength(450);
        b.Property(u => u.UserName).HasMaxLength(256);
        b.Property(u => u.NormalizedUserName).HasMaxLength(256);
        b.Property(u => u.Email).HasMaxLength(256);
        b.Property(u => u.NormalizedEmail).HasMaxLength(256);
    });

    // Identity tablolarındaki composite key alanları
    modelBuilder.Entity<IdentityUserRole<string>>(b =>
    {
        b.Property(ur => ur.UserId).HasMaxLength(450);
        b.Property(ur => ur.RoleId).HasMaxLength(450);
    });

    modelBuilder.Entity<IdentityUserLogin<string>>(b =>
    {
        b.Property(l => l.LoginProvider).HasMaxLength(450);
        b.Property(l => l.ProviderKey).HasMaxLength(450);
    });

    modelBuilder.Entity<IdentityUserToken<string>>(b =>
    {
        b.Property(t => t.UserId).HasMaxLength(450);
        b.Property(t => t.LoginProvider).HasMaxLength(450);
        b.Property(t => t.Name).HasMaxLength(450);
    });

    modelBuilder.Entity<IdentityUserClaim<string>>(b =>
    {
        b.Property(c => c.UserId).HasMaxLength(450);
    });

    modelBuilder.Entity<IdentityRoleClaim<string>>(b =>
    {
        b.Property(c => c.RoleId).HasMaxLength(450);
    });

    // Senin tabloların
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