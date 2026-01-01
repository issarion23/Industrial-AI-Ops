
using Industrial_AI_Ops.Core.Ports;
using Industrial_AI_Ops.Core.Ports.RAG;

public sealed class LlmRequest
{
    public string Question { get; set; } = null!;
    public List<RetrievedChunk> Context { get; set; } = new();
}