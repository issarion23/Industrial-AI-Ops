using Industrial_AI_Ops.Core.Models.LLM;

namespace Industrial_AI_Ops.Core.Ports.RAG;

public interface IDocumentRetriever
{
    Task<List<RetrievedChunk>> RetrieveAsync(string query, CancellationToken ct = default);
}