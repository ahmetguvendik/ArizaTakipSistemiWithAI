using Application.Repositories;
using Application.Repositories.Master;
using Application.SemanticKernel.Services;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Repositories;
using Persistance.Repositories.Master;
using Persistance.Services;

namespace Persistance;

public static class ServiceRegistration
{
    public static void AddPersistanceService(this IServiceCollection collection)
    {
        collection.AddHttpContextAccessor(); 
        collection.AddDbContext<FaultDbContext>((serviceProvider, options) =>
        {
            var tenantProvider = serviceProvider.GetRequiredService<ITenantService>();
            var connectionString = tenantProvider.GetConnectionString();

            options.UseNpgsql(connectionString);
        });
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        collection.AddDbContext<MasterDbContext>(opt =>
            opt.UseNpgsql("User ID=postgres;Password=testtest;Host=localhost;Port=5432;Database=FaultReportMasterDb;"));
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        collection.AddIdentity<AppUser, AppRole>()
            .AddEntityFrameworkStores<FaultDbContext>()
            .AddDefaultTokenProviders(); // Bu satır şart!;     
        
        collection.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        collection.AddScoped(typeof(IFaultReportRepository), typeof(FaultReportRepository));
        collection.AddScoped(typeof(IMachineRepository), typeof(MachineRepository));    
        collection.AddScoped(typeof(IEmailService), typeof(EmailService));
        collection.AddScoped(typeof(IHangfireService), typeof(HangfireService));
        collection.AddScoped(typeof(IStatisticsRepository), typeof(StatisticsRepository));
        collection.AddScoped(typeof(IDepartmentRepository), typeof(DepartmentRepository));
        collection.AddScoped(typeof(IUserRepository), typeof(UserRepository));
        collection.AddScoped(typeof(ITenantService), typeof(TenantService));
        collection.AddScoped(typeof(ITenantRepository), typeof(TenantRepository));
        collection.AddScoped<AIService>();
        
    }
}