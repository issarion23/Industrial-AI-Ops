using Industrial_AI_Ops.Core.Contracts.Response;
using Industrial_AI_Ops.Core.Ports.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace Industrial_AI_Ops.Api.Controller;

[ApiController]
[Route("api/v{version:apiVersion}/dashboard")]
[Produces("application/json")]
[ApiVersion("1.0")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _service;
    
    public DashboardController(IDashboardService service)
    {
        _service = service;
    }
    
    /// <summary>
    /// Get dashboard summary
    /// </summary>
    [HttpGet("summary")]
    [ProducesResponseType(typeof(DashboardSummaryResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<DashboardSummaryResponse>> GetDashboardSummary()
    {
        var result = await _service.GetDashboardSummary();

        return Ok(result);
    }
}