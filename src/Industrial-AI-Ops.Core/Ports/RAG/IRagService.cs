using Industrial_AI_Ops.Core.Models.RAG;

namespace Industrial_AI_Ops.Core.Ports.RAG;

public interface IRagService
{
    Task<RagAnswer> AskAsync(string question, CancellationToken ct = default);
}