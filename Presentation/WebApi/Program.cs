using System.ClientModel;
using Persistance; 
using Application; 
using Application.Hubs;
using Application.SemanticKernel.Services;
using Application.SemanticKernel.Tools;
using Application.Services;
using Application.Validations.FaultValidations;
using Application.Validations.MachineValidations;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using OpenAI;
using Serilog;
using WebApi.ViewModels; 
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Gerekli servisleri ekle (mevcut kayıtlarınız)
builder.Services.AddControllers();
builder.Services.AddHttpClient(); // IHttpClientFactory için
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cookie authentication kaldırıldı, sadece JWT authentication kaldı
var jwtSettings = builder.Configuration.GetSection("Jwt").Get<Application.DTOs.JwtSettings>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
        ClockSkew = TimeSpan.Zero,
        RoleClaimType = ClaimTypes.Role,
        NameClaimType = ClaimTypes.Name
    };
});

builder.Services.AddHttpContextAccessor(); // HttpContext erişimi için

// Diğer katmanlardaki servis kayıtları (Eğer bu metodlar gerçekten varsa ve servisleri doğru ekliyorsa)
builder.Services.AddPersistanceService(builder.Configuration);
builder.Services.AddApplicationService(builder.Configuration);

builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateFaultReportValidation>()); 
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateMachineValidation>()); 

var logsDbConnection = builder.Configuration.GetSection("LoggingDbConnectionStrings")["LogsDb"];
Log.Logger = new LoggerConfiguration().WriteTo.MSSqlServer(logsDbConnection,"Logs").MinimumLevel.Information().CreateLogger();

var defaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddHangfire(configuration => configuration.UseSqlServerStorage(defaultConnection));
builder.Services.AddHangfireServer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials() // SignalR için MUTLAKA OLMALI
                  .SetIsOriginAllowed(origin => true); // Development için '*' gibi düşünebilirsiniz, ancak güvenlik için spesifik origin belirtmek daha iyidir.
        });
});

// SignalR servisini ekle
builder.Services.AddSignalR();

// FaultTools servisini kaydet (IHttpClientFactory'ye bağımlı olduğu için AddHttpClient'dan sonra olmalı)
// Kernel'e bir plugin olarak ekleneceği için Singleton olarak kaydedilebilir.
builder.Services.AddSingleton<FaultTools>();

// Kernel ve IChatCompletionService'i DI konteynerine kaydet
// AIService'in bu bağımlılıklara ihtiyacı olduğu için bu adım hayati önem taşır.
builder.Services.AddSingleton<Kernel>(serviceProvider =>
{
    var kernelBuilder = Kernel.CreateBuilder();

    // OpenAI/Gemma modeli yapılandırması
    kernelBuilder.AddOpenAIChatCompletion(
        modelId: "google/gemini-2.5-flash-preview",
        openAIClient: new OpenAIClient(
            credential: new ApiKeyCredential(
                "sk-or-v1-a13fff**f"),   
            options: new OpenAIClientOptions
            {
                Endpoint = new Uri("https://openrouter.ai/api/v1")
            })
    );

    kernelBuilder.Plugins.AddFromObject(serviceProvider.GetRequiredService<FaultTools>());

    return kernelBuilder.Build();
});

builder.Services.AddSingleton<IChatCompletionService>(serviceProvider =>
{
    var kernel = serviceProvider.GetRequiredService<Kernel>();
    return kernel.GetRequiredService<IChatCompletionService>();
});

var app = builder.Build();

// Swagger UI sadece development ortamında aktif olur
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHangfireDashboard();
app.UseHttpsRedirection();


app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

RecurringJob.AddOrUpdate<IHangfireService>(
    "send-report",
    service => service.SendDailyReportEmailAsync(),
    Cron.Daily);

app.MapControllers();

// SignalR Hub'larını ve Minimal API endpoint'lerini maple
app.MapPost("/chat", async (AIService aiService, ChatRequestVM chatRequest, CancellationToken cancellationToken) =>
    await aiService.GetMessageStreamAsync(chatRequest.Prompt, chatRequest.ConnectionId, cancellationToken));

app.MapHub<FaultHub>("/fault"); 
app.MapHub<ChatHub>("/ai-hub"); 

app.Run();
