namespace Industrial_AI_Ops.Core.Models.RAG;


public sealed class RagAnswer
{
    public string Answer { get; set; } = null!;
    public bool HasAnswer { get; set; }
    public List<RagCitation> Sources { get; set; } = new();
}

public sealed class RagCitation
{
    public string Source { get; set; } = null!;
    public string Snippet { get; set; } = null!;
}