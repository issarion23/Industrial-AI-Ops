using Industrial_AI_Ops.Api.Common;
using Industrial_AI_Ops.Core.Contracts.Response;
using Industrial_AI_Ops.Core.Ports.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace Industrial_AI_Ops.Api.Controller;

/// <summary>
/// 
/// </summary>
[Route("api/analytics")]
[ApiVersion("1.0")]
public class AnalyticsController : BaseController
{
    private readonly IAnalyticsService _service;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="service"></param>
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
        string equipmentId, 
        int days = 30)
    {
        var result = await _service.GetEquipmentHealthTrend(equipmentId, days);

        return result.ToActionResult();
    }
}