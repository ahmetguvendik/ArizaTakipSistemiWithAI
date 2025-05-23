using Application.Repositories;
using Application.Services;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Repositories;
using Persistance.Services;

namespace Persistance;

public static class ServiceRegistration
{
    public static void AddPersistanceService(this IServiceCollection collection)
    {
        collection.AddDbContext<FaultDbContext>(opt =>
            opt.UseNpgsql("User ID=postgres;Password=testtest;Host=localhost;Port=5432;Database=FaultReportDb;"));  
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        collection.AddIdentity<AppUser, AppRole>()
            .AddEntityFrameworkStores<FaultDbContext>();     
        
        collection.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        collection.AddScoped(typeof(IFaultReportRepository), typeof(FaultReportRepository));
        collection.AddScoped(typeof(IMachineRepository), typeof(MachineRepository));    
        collection.AddScoped(typeof(IEmailService), typeof(EmailService));

        
    }
}