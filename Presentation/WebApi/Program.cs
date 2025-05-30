using System.ClientModel;
using Persistance; 
using Application; 
using Application.Hubs;
using Application.SemanticKernel.Services;
using Application.SemanticKernel.Tools;
using Application.Validations.FaultValidations; 
using FluentValidation.AspNetCore; 
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using OpenAI; 
using WebApi.ViewModels; 

var builder = WebApplication.CreateBuilder(args);

// Gerekli servisleri ekle (mevcut kayıtlarınız)
builder.Services.AddControllers();
builder.Services.AddHttpClient(); // IHttpClientFactory için
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Diğer katmanlardaki servis kayıtları (Eğer bu metodlar gerçekten varsa ve servisleri doğru ekliyorsa)
builder.Services.AddPersistanceService();
builder.Services.AddApplicationService(builder.Configuration);

builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateFaultReportValidation>()); // Eğer kullanılıyorsa

// CORS ayarları: SignalR ve AJAX istekleri için kritik
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

app.UseHttpsRedirection(); 

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// SignalR Hub'larını ve Minimal API endpoint'lerini maple
app.MapPost("/chat", async (AIService aiService, ChatRequestVM chatRequest, CancellationToken cancellationToken) =>
    await aiService.GetMessageStreamAsync(chatRequest.Prompt, chatRequest.ConnectionId, cancellationToken));

app.MapHub<FaultHub>("/fault"); 
app.MapHub<ChatHub>("/ai-hub"); 

app.Run(); // Uygulamayı başlat