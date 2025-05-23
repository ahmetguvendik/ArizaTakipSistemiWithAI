using System.ClientModel;
using Persistance; // Eğer Persistance katmanında bir ServiceRegistration varsa
using Application; // Eğer Application katmanında bir ServiceRegistration varsa
using Application.Hubs;
using Application.SemanticKernel.Services;
using Application.SemanticKernel.Tools;
using Application.Validations.FaultValidations; // Eğer kullanılıyorsa
using FluentValidation.AspNetCore; // Eğer kullanılıyorsa
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using OpenAI; // Eğer OpenAIClient kullanıyorsanız
using WebApi.Hubs; // Eğer WebApi.Hubs içinde özel bir Hub varsa (FaultHub gibi)
using WebApi.ViewModels; // Eğer ViewModels kullanılıyorsa

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
                "sk-or-v1-***179"),
            options: new OpenAIClientOptions
            {
                Endpoint = new Uri("https://openrouter.ai/api/v1")
            })
    );

    // FaultTools plugin'ini Kernel'e ekle.
    // FaultTools'un bir örneğini DI konteynerinden alıyoruz.
    kernelBuilder.Plugins.AddFromObject(serviceProvider.GetRequiredService<FaultTools>());

    return kernelBuilder.Build();
});

// IChatCompletionService'i Kernel'den alarak DI'a kaydet
builder.Services.AddSingleton<IChatCompletionService>(serviceProvider =>
{
    var kernel = serviceProvider.GetRequiredService<Kernel>();
    return kernel.GetRequiredService<IChatCompletionService>();
});

// AIService'in zaten Persistance katmanında AddScoped olarak kaydedildiğini biliyoruz.
// builder.Services.AddScoped<AIService>(); // Bu satırı tekrar yazmanıza gerek yok, Persistance.ServiceRegistration zaten ekliyor.

var app = builder.Build();

// Swagger UI sadece development ortamında aktif olur
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware sıralaması önemlidir:
// UseCors, UseRouting'den önce gelmeli
app.UseCors("AllowAll");

app.UseHttpsRedirection(); // HTTPS yönlendirme için (LaunchSettings.json'da HTTPS portu tanımlı olmalı)

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); // API Controller'larını mapler

// SignalR Hub'larını ve Minimal API endpoint'lerini maple
app.MapPost("/chat", async (AIService aiService, ChatRequestVM chatRequest, CancellationToken cancellationToken) =>
    await aiService.GetMessageStreamAsync(chatRequest.Prompt, chatRequest.ConnectionId, cancellationToken));

app.MapHub<FaultHub>("/fault"); // Eğer bu Hub'ı kullanmıyorsanız kaldırabilirsiniz.
app.MapHub<ChatHub>("/ai-hub"); // Chat uygulaması için gerekli Hub

app.Run(); // Uygulamayı başlat