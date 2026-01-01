namespace Industrial_AI_Ops.Core.Ports.RAG;

public interface IDocumentRetriever
{
    Task<List<RetrievedChunk>> RetrieveAsync(string query, CancellationToken ct = default);
}

public sealed class RetrievedChunk
{
    public string Content { get; set; } = null!;
    public string Source { get; set; } = null!;
}