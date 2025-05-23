using Application.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace Application.SemanticKernel.Services;

public class AIService
{
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly IChatCompletionService _chatCompletionService;
    private readonly Kernel _kernel;

    public AIService(IHubContext<ChatHub> hubContext, IChatCompletionService chatCompletionService, Kernel kernel)
    {
        _hubContext = hubContext;
        _chatCompletionService = chatCompletionService;
        _kernel = kernel;
    }

    public async Task GetMessageStreamAsync(string prompt, string connectionId, CancellationToken cancellationToken = default)
    {
        var openAIPromptExecutionSettings = new OpenAIPromptExecutionSettings
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };

        var history = HistoryService.GetChatHistory(connectionId);

        history.AddUserMessage(prompt);
        string responseContent = "";
        try
        {
            await foreach (var response in _chatCompletionService.GetStreamingChatMessageContentsAsync(history, executionSettings: openAIPromptExecutionSettings, kernel: _kernel))
            {
                cancellationToken.ThrowIfCancellationRequested();

                await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveMessage", response.ToString());
                responseContent += response.ToString();
            }
        }
        catch (Exception ex)
        {
            // Hata loglanabilir, örn:
            Console.WriteLine($"AIService error: {ex.Message}");
        }
        history.AddAssistantMessage(responseContent);
    }
}