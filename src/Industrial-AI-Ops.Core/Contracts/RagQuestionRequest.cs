namespace Industrial_AI_Ops.Core.Contracts;

/// <summary>
/// Request model for RAG questions
/// </summary>
public record RagQuestionRequest
{
    /// <summary>
    /// The question to ask the AI assistant
    /// </summary>
    public string Question { get; init; } = string.Empty;
}