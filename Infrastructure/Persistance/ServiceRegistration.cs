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
using Domain.Entities.Master; 

namespace Persistance
{
    public static class ServiceRegistration
    {
        public static void AddPersistanceService(this IServiceCollection collection)
        {
            collection.AddHttpContextAccessor();
            
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            // MasterDbContext sabit connection string ile kalabilir
            collection.AddDbContext<MasterDbContext>(opt =>
                opt.UseNpgsql("User ID=postgres;Password=testtest;Host=localhost;Port=5432;Database=FaultReportMasterDb;"));
            
            // --- FaultDbContext için Dinamik ConnectionString Yönetimi (Session Tabanlı) ---
            collection.AddDbContext<FaultDbContext>((serviceProvider, opt) =>
            {
                var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                
                // *** BU SATIRI DEĞİŞTİRİN ***
                // ConnectionString'i Çerezden DEĞİL, Session'dan okuyun
                string connectionString = httpContextAccessor.HttpContext?.Session?.GetString("DynamicConnectionString");
                // **************************

                // Eğer connectionString Session'da bulunamazsa veya boşsa
                if (string.IsNullOrEmpty(connectionString))
                {
                    // Bu senaryoda hata fırlatmak en doğru yaklaşımdır.
                    // Kullanıcının önce geçerli bir kiracı bağlantısı kurması gerekir.
                    throw new InvalidOperationException("DynamicConnectionString not found in Session. Please establish a tenant connection first.");
                }

                opt.UseNpgsql(connectionString);    
            });
            
            collection.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<FaultDbContext>() 
                .AddDefaultTokenProviders();
            
            // Diğer scoped servisleriniz aynı kalır
            collection.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            collection.AddScoped<IFaultReportRepository, FaultReportRepository>();
            collection.AddScoped<IMachineRepository, MachineRepository>();
            collection.AddScoped<IEmailService, EmailService>();
            collection.AddScoped<IHangfireService, HangfireService>();
            collection.AddScoped<IStatisticsRepository, StatisticsRepository>();
            collection.AddScoped<IDepartmentRepository, DepartmentRepository>();
            collection.AddScoped<IUserRepository, UserRepository>();
            collection.AddScoped<ITenantRepository, TenantRepository>();

            collection.AddScoped<AIService>();
        }
    }
}