using Industrial_AI_Ops.Api.Common;
using Industrial_AI_Ops.Core.Contracts.Response;
using Industrial_AI_Ops.Core.Ports.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace Industrial_AI_Ops.Api.Controller;
/// <summary>
/// 
/// </summary>
[Route("api/dashboard")]
[ApiVersion("1.0")]
public class DashboardController : BaseController
{
    private readonly IDashboardService _service;
    
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="service"></param>
    public DashboardController(IDashboardService service) => _service = service;
    
    /// <summary>
    /// Get dashboard summary
    /// </summary>
    [HttpGet("summary")]
    [ProducesResponseType(typeof(DashboardSummaryResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<DashboardSummaryResponse>> GetDashboardSummary()
    {
        var result = await _service.GetDashboardSummary();

        return result.ToActionResult();
    }
}