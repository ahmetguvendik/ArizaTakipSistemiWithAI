using Persistance;
using Application;
using Application.Validations.FaultValidations;
using FluentValidation.AspNetCore;
using WebApi.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Gerekli servisleri ekle
builder.Services.AddControllers();            // ← Controller desteği
builder.Services.AddAuthorization();          // ← Authorization middleware çalışsın diye
builder.Services.AddEndpointsApiExplorer();   // ← Swagger için
builder.Services.AddSwaggerGen();             // ← Swagger için
builder.Services.AddPersistanceService();  
builder.Services.AddApplicationService(builder.Configuration);
builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateFaultReportValidation>());

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .SetIsOriginAllowed(origin => true);
        });
});

builder.Services.AddSignalR();

var app = builder.Build();

// Swagger UI sadece development ortamında aktif olur
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");    

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<FaultHub>("/fault"); 

app.Run();