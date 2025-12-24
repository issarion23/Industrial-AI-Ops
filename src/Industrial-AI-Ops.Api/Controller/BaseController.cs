using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Industrial_AI_Ops.Api.Controller;

/// <summary>
/// 
/// </summary>
[ApiController]
[Produces("application/json")]
[ProducesResponseType(statusCode: (int)HttpStatusCode.ServiceUnavailable, type: typeof(ProblemDetails))]
[ProducesResponseType(statusCode: (int)HttpStatusCode.InternalServerError, type: typeof(ProblemDetails))]
public class BaseController : ControllerBase { }