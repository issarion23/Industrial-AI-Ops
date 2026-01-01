namespace Industrial_AI_Ops.Core.Models.LLM;

public record Message(ChatRole Role, string? Content, Tool? FunctionCall = null, bool ContentIsBase64File = false);

public record Tool(string Id, string Name, string? Arguments);

public enum ChatRole
{
    Assistant,
    User,
    System
}