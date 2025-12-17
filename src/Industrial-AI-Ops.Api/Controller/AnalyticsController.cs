using Industrial_AI_Ops.Core.Contracts.Response;
using Industrial_AI_Ops.Core.Ports.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace Industrial_AI_Ops.Api.Controller;

[ApiController]
[Route("api/v{version:apiVersion}/analytics")]
[Produces("application/json")]
[ApiVersion("1.0")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _service;
    
    public AnalyticsController(IAnalyticsService service)
    {
        _service = service;
    }
    /// <summary>
    /// Get equipment health trends
    /// </summary>
    [HttpGet("equipment/{equipmentId}/health-trend")]
    [ProducesResponseType(typeof(EquipmentHealthTrendResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EquipmentHealthTrendResponse>> GetEquipmentHealthTrend(
        int equipmentId, 
        [FromQuery] int days = 30)
    {
        var result = await _service.GetEquipmentHealthTrend();

        return Ok(result);
    }
}