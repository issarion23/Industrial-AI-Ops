using Industrial_AI_Ops.Core.Common.Result;

namespace Industrial_AI_Ops.Api.Common;

public static class ErrorParser
{
    public static Error Parse(string error)
    {
        var parts = error.Split(':', 2);

        if (parts.Length != 2)
            return new Error(ErrorCode.Unknown, error);

        return new Error(
            Enum.Parse<ErrorCode>(parts[0]),
            parts[1]);
    }
}
