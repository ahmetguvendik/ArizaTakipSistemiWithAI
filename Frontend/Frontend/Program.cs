using System.ClientModel;
using Application.SemanticKernel.Tools;
using Application.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.SemanticKernel;
using OpenAI;
using Persistance.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddScoped(typeof(IEmailService), typeof(EmailService));

builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MyCookieAuth", options =>
    {
        options.LoginPath = "/Login/Index";
        options.AccessDeniedPath = "/Login/AccessDenied";

        // İstersen cookie adını özelleştir
        options.Cookie.Name = "MyCookieAuthCookie";
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
    });



Log.Logger = new LoggerConfiguration().WriteTo.PostgreSQL("User ID=postgres;Password=testtest;Host=localhost;Port=5432;Database=logs_db;","Logs",needAutoCreateTable:true).MinimumLevel.Information().CreateLogger();

builder.Services
    .AddKernel()
    .AddOpenAIChatCompletion(
        modelId: "google/gemini-2.5-flash-preview", 
        openAIClient: new OpenAIClient(
            credential: new ApiKeyCredential(
                "sk-or-v1-a13fff**f"),   
            options: new OpenAIClientOptions
            {
                Endpoint = new Uri("https://openrouter.ai/api/v1")
            })
    ).Plugins.AddFromType<FaultTools>();

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

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");


app.UseStaticFiles();

app.UseAuthentication();    
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Ariza}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();