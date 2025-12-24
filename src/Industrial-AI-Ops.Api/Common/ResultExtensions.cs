using CSharpFunctionalExtensions;
using Industrial_AI_Ops.Core.Common.Result;
using Microsoft.AspNetCore.Mvc;

namespace Industrial_AI_Ops.Api.Common;

public static class ResultExtensions
{
    public static ActionResult ToActionResult<T>(
        this Result<T> result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(result.Value);

        var error = ErrorParser.Parse(result.Error);

        return error.Code switch
        {
            ErrorCode.NotFound =>
                new NotFoundObjectResult(error.Message),

            ErrorCode.Validation =>
                new BadRequestObjectResult(error.Message),

            ErrorCode.Forbidden =>
                new ForbidResult(),

            _ =>
                new ObjectResult(error.Message)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                }
        };
    }
    
    public static ActionResult ToActionResult(
        this Result result)
    {
        if (result.IsSuccess)
            return new OkObjectResult(result);

        var error = ErrorParser.Parse(result.Error);

        return error.Code switch
        {
            ErrorCode.NotFound =>
                new NotFoundObjectResult(error.Message),

            ErrorCode.Validation =>
                new BadRequestObjectResult(error.Message),

            ErrorCode.Forbidden =>
                new ForbidResult(),

            _ =>
                new ObjectResult(error.Message)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                }
        };
    }
}
