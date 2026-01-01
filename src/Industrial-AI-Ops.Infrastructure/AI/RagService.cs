using Industrial_AI_Ops.Core.Models.RAG;
using Industrial_AI_Ops.Core.Ports.LLM;
using Industrial_AI_Ops.Core.Ports.RAG;
using RetrievedChunk = Industrial_AI_Ops.Core.Ports.RAG.RetrievedChunk;

namespace Industrial_AI_Ops.Infrastructure.AI;

public sealed class RagService : IRagService
{
    private readonly IDocumentRetriever _retriever;
    private readonly ILlmService _llm;

    public RagService(
        IDocumentRetriever retriever,
        ILlmService llm)
    {
        _retriever = retriever;
        _llm = llm;
    }

    public async Task<RagAnswer> AskAsync(string question, CancellationToken ct = default)
    {
        var chunks = await _retriever.RetrieveAsync(question, ct);

        if (chunks.Count == 0)
        {
            return new RagAnswer
            {
                HasAnswer = false,
                Answer = "Нет данных для точного ответа.",
                Sources = new List<RagCitation>()
            };
        }

        var llmRequest = new LlmRequest
        {
            Question = question,
            Context = chunks.Select(c => new RetrievedChunk
            {
                Content = c.Content,
                Source = c.Source
            }).ToList()
        };

        var llmResult = await _llm.AskAsync(llmRequest, ct);

        return new RagAnswer
        {
            HasAnswer = llmResult.HasAnswer,
            Answer = llmResult.Answer,
            Sources = chunks.Select(c => new RagCitation
            {
                Source = c.Source,
                Snippet = c.Content.Length > 300
                    ? c.Content[..300] + "..."
                    : c.Content
            }).ToList()
        };
    }
}
