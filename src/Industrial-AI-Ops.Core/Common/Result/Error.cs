namespace Industrial_AI_Ops.Core.Common.Result;

public sealed record Error(
    ErrorCode Code,
    string Message)
{
    public override string ToString()
        => $"{Code}:{Message}";
}
