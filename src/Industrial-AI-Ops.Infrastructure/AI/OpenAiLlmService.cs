using Industrial_AI_Ops.Core.Models.LLM;
using Industrial_AI_Ops.Core.Ports.LLM;
using Industrial_AI_Ops.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using OpenAI;

namespace Industrial_AI_Ops.Infrastructure.AI;

public sealed class OpenAiLlmService : ILlmService
{
    private readonly OpenAIClient _client;
    private readonly string _model;

    public OpenAiLlmService(IConfiguration config)
    {
        var openAIOptions = config.GetSection(OpenAiOptions.SectionName).Get<OpenAiOptions>();
        var apiKey = openAIOptions.ApiKey;
        _model = openAIOptions.Model;

        _client = new OpenAIClient(apiKey);
    }

    public async Task<LlmAnswer> AskAsync(LlmRequest request, CancellationToken ct = default)
    {
        var chat = _client.GetChatClient(_model);

        var messages = new List<Message>();

        messages.Add(new Message(ChatRole.System, PromptBuilder.BuildRagPrompt()));

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