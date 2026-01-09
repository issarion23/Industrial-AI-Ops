using Industrial_AI_Ops.Core.Models.LLM;
using Industrial_AI_Ops.Core.Models.RAG;
using Industrial_AI_Ops.Core.Ports.LLM;
using Industrial_AI_Ops.Core.Ports.RAG;

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
        // Step 1: Retrieve relevant documents
        var retrievedChunks = await _retriever.RetrieveAsync(question, ct);

        if (retrievedChunks.Count == 0)
        {
            return new RagAnswer
            {
                HasAnswer = false,
                Answer = "No relevant information found in the knowledge base to answer your question.",
                Sources = new List<RagCitation>()
            };
        }

        // Step 2: Convert retrieved chunks to LLM context format
        var llmRequest = new LlmRequest
        {
            Question = question,
            Context = retrievedChunks.Select(chunk => new RetrievedChunk
            {
                Content = chunk.Content,
                Source = chunk.Source
            }).ToList()
        };

        // Step 3: Get answer from LLM
        var llmResult = await _llm.AskAsync(llmRequest, ct);

        // Step 4: Build RAG response with citations
        return new RagAnswer
        {
            HasAnswer = llmResult.HasAnswer,
            Answer = llmResult.Answer,
            Sources = retrievedChunks.Select(chunk => new RagCitation
            {
                Source = chunk.Source,
                Snippet = chunk.Content.Length > 300
                    ? chunk.Content[..300] + "..."
                    : chunk.Content
            }).ToList()
        };
    }
}