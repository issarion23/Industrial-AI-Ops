using Industrial_AI_Ops.Core.Models.LLM;

namespace Industrial_AI_Ops.Core.Ports.LLM;

public interface ILlmService
{
    Task<LlmAnswer> AskAsync(LlmRequest request, CancellationToken ct = default);
    // IAsyncEnumerable<string> StreamAsync(LlmRequest request, CancellationToken ct = default);
}
