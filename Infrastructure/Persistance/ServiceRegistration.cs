using Application.Repositories;
using Application.SemanticKernel.Services;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Repositories;
using Persistance.Services;
using System;

namespace Persistance
{
    public static class ServiceRegistration
    {
        public static void AddPersistanceService(this IServiceCollection collection)
        {
            collection.AddHttpContextAccessor();
            

            collection.AddDbContext<FaultDbContext>(opt =>
                opt.UseSqlServer("Server=localhost;Database=FaultReportDb;User Id=SA;Password=Ahmet.123;Encrypt=True;TrustServerCertificate=True;"));

            
            collection.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<FaultDbContext>() 
                .AddDefaultTokenProviders();
            
            collection.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            collection.AddScoped<IFaultReportRepository, FaultReportRepository>();
            collection.AddScoped<IMachineRepository, MachineRepository>();
            collection.AddScoped<IEmailService, EmailService>();
            collection.AddScoped<IHangfireService, HangfireService>();
            collection.AddScoped<IStatisticsRepository, StatisticsRepository>();
            collection.AddScoped<IDepartmentRepository, DepartmentRepository>();
            collection.AddScoped<IUserRepository, UserRepository>();
            collection.AddScoped<AIService>();
        }
    }
}