namespace Industrial_AI_Ops.Core.Models.LLM;

public sealed class LlmRequest
{
    public string Question { get; set; } = null!;
    public List<RetrievedChunk> Context { get; set; } = new();
}