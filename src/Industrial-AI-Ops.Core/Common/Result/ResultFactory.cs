using CSharpFunctionalExtensions;

namespace Industrial_AI_Ops.Core.Common.Result;


public static class ResultFactory
{
    public static CSharpFunctionalExtensions.Result Success()
    {
        return CSharpFunctionalExtensions.Result.Success();
    }

    public static Result<T> Success<T>(T value)
    {
        return CSharpFunctionalExtensions.Result.Success(value);
    }
    
    public static CSharpFunctionalExtensions.Result Failure(
        ErrorCode code,
        string message)
    {
        return CSharpFunctionalExtensions.Result.Failure(new Error(code, message).ToString());
    }

    public static Result<T> Failure<T>(
        ErrorCode code,
        string message)
    {
        return CSharpFunctionalExtensions.Result.Failure<T>(new Error(code, message).ToString());
    }
}