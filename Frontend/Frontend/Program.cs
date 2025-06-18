using System.ClientModel;
using Application.SemanticKernel.Tools;
using Application.Services;
using Microsoft.SemanticKernel;
using OpenAI;
using Persistance.Services;
using Serilog;
using Serilog.Sinks.MSSqlServer;

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
       
    });

string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var columnOptions = new ColumnOptions();
columnOptions.Store.Remove(StandardColumn.Properties);
columnOptions.Store.Add(StandardColumn.LogEvent); // LogEvent kolonunu sakla

Log.Logger = new LoggerConfiguration()
    .WriteTo.MSSqlServer(
        connectionString: "Server=localhost;Database=LogsDb;User Id=SA;Password=Ahmet.123;Encrypt=True;TrustServerCertificate=True;",
        sinkOptions: new MSSqlServerSinkOptions
        {
            AutoCreateSqlTable = true,
            TableName = "Logs"
        },
        columnOptions: columnOptions
    )
    .Enrich.WithProperty("Environment", env)
    .MinimumLevel.Information()
    .CreateLogger();

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

app.UseStatusCodePagesWithReExecute("/Error/{0}");

app.UseStaticFiles();

app.UseAuthentication();    
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Ariza}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();