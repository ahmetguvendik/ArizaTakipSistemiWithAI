using System.ClientModel;
using Application.SemanticKernel.Tools;
using Application.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.SemanticKernel;
using OpenAI;
using Persistance.Services;
using Serilog;
using Persistance; // AddPersistanceService uzantı metodu için gerekli
using Application.Hubs; // ChatHub için gerekli using

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped(typeof(IEmailService), typeof(EmailService));
builder.Services.AddHttpContextAccessor(); 
builder.Services.AddDistributedMemoryCache(); // <-- BU SATIRI EKLEYİN!

// Oturum servisini ekle
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturumun süresi
    options.Cookie.HttpOnly = true; // Sadece HTTP üzerinden erişilebilir çerez
    options.Cookie.IsEssential = true; // GDPR uyumluluğu için gerekli
});

// SignalR hizmetlerini ekle
builder.Services.AddSignalR(); // Bu satırı ekleyin!

// Persistance servislerinizi ekleyin.
builder.Services.AddPersistanceService(); 


// Authentication scheme'lerini tanımla
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "MyCookieAuth";
    })
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/Login/Index";
        options.AccessDeniedPath = "/Login/AccessDenied";
        options.Cookie.Name = "MyCookieAuth";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
    });


Log.Logger = new LoggerConfiguration()
    .WriteTo.PostgreSQL(
        "User ID=postgres;Password=testtest;Host=localhost;Port=5432;Database=logs_db;",
        "Logs",
        needAutoCreateTable: true)
    .MinimumLevel.Information()
    .CreateLogger();

builder.Services
    .AddKernel()
    .AddOpenAIChatCompletion(
        modelId: "google/gemini-2.5-flash-preview",
        openAIClient: new OpenAIClient(
            credential: new ApiKeyCredential("sk-or-v1-a13fff**f"),
            options: new OpenAIClientOptions
            {
                Endpoint = new Uri("https://openrouter.ai/api/v1")
            })
    )
    .Plugins.AddFromType<FaultTools>();

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

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting(); 

app.UseCors("AllowAll");

app.UseSession(); 

app.UseAuthentication();
app.UseAuthorization();

// SignalR Hub'larını haritalayın
app.MapHub<ChatHub>("/chatHub"); // Bu satırı ekleyin! (veya Hub'ınızın adını doğru girin)

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Ariza}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();