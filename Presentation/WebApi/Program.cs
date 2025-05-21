using Persistance;
using Application;
using Application.Validations.FaultValidations;
using FluentValidation.AspNetCore;

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


var app = builder.Build();

// Swagger UI sadece development ortamında aktif olur
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthorization();

app.MapControllers();  

app.Run();