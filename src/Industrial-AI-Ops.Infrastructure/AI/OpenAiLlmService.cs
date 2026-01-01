using System.Runtime.CompilerServices;
using Industrial_AI_Ops.Core.Models.LLM;
using Industrial_AI_Ops.Core.Ports;
using Industrial_AI_Ops.Core.Ports.LLM;
using Industrial_AI_Ops.Infrastructure.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;

namespace IndustrialAIOps.Infrastructure.AI;

public sealed class OpenAiLlmService : ILlmService
{
    private readonly OpenAIClient _client;
    private readonly string _model;

    public OpenAiLlmService(IConfiguration config)
    {
        var apiKey = config["OpenAI:ApiKey"];
        _model = config["OpenAI:Model"] ?? "gpt-4.1";

        _client = new OpenAIClient(apiKey);
    }

    public async Task<LlmAnswer> AskAsync(LlmRequest request, CancellationToken ct = default)
    {
        var chat = _client.GetChatClient(_model);

        var messages = new List<Message>();

        messages.Add(new Message(ChatRole.System, "You are an Industrial AI Ops assistant. Answer strictly based on provided context."));

        if (request.Context?.Any() == true)
        {
            var contextText = string.Join("\n\n", request.Context.Select(c => c.Content));
            messages.Add(new Message(ChatRole.User, $"Context:\n{contextText}"));
        }

        messages.Add(new Message(ChatRole.User, $"Question: {request.Question}"));

        var mappedMessages = OpenAiRequestBuilder.Convert(messages);

        var response = await chat.CompleteChatAsync(mappedMessages, cancellationToken: ct);

        return new LlmAnswer
        {
            HasAnswer = true,
            Answer = response.Value.Content[0].Text
        };
    }
    
    // public async IAsyncEnumerable<string> StreamAsync(LlmRequest request, [EnumeratorCancellation] CancellationToken ct = default)
    // {
    //     var prompt = PromptBuilder.BuildRagPrompt(request.Question, request.Context);
    //
    //     // Create ChatMessage directly from SDK
    //     var messages = new List<ChatMessage>
    //     {
    //         new ChatMessage(ChatMessageRole.User, prompt) // ChatMessageRole replaces ChatRole
    //     };
    //
    //     // Create streaming options
    //     var options = new ChatCompletionsOptions
    //     {
    //         Messages = messages
    //     };
    //
    //     // Streaming call
    //     var result = await _client.GetChatCompletionsStreamingAsync(_model, options, ct);
    //
    //     await foreach (var message in result)
    //     {
    //         var chunk = message.ContentUpdate;
    //         if (!string.IsNullOrEmpty(chunk))
    //             yield return chunk;
    //     }
    // }
}